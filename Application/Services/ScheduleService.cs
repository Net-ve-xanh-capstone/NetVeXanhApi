using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Schedule;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.ScheduleViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Services;

public class ScheduleService : IScheduleService
{
    private readonly IExcelService _excelService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;
    private readonly IMailService _mailService;

    public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper, IValidatorFactory validatorFactory,
        IExcelService excelService, IMailService mailService)
    {
        _mailService = mailService;
        _excelService = excelService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validatorFactory = validatorFactory;
    }

    #region Get For Website

    public async Task<List<ListScheduleViewModel>> GetListSchedule(Guid id)
    {
        var rounds = await _unitOfWork.RoundRepo.GetRoundByContestId(id);
        return _mapper.Map<List<ListScheduleViewModel>>(rounds);
    }

    #endregion

    #region Get All

    public async Task<(List<ScheduleRatingViewModel>, int)> GetListSchedule(ListModels listModels)
    {
        var list = await _unitOfWork.ScheduleRepo.GetAllAsync();
        if (list.Count == 0) throw new Exception("Khong tim thay Schedule nao");
        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        var result = list.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (_mapper.Map<List<ScheduleRatingViewModel>>(result), totalPages);
    }

    #endregion

    #region Update

    public async Task<bool> UpdateSchedule(ScheduleUpdateRequest updateSchedule)
    {
        var validationResult = await ValidateScheduleUpdateRequest(updateSchedule);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        var schedule = await _unitOfWork.ScheduleRepo.GetByIdAsync(updateSchedule.Id);
        if (schedule == null) throw new Exception("Khong tim thay Schedule");
        _mapper.Map(updateSchedule, schedule);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Delete

    public async Task<bool> DeleteSchedule(Guid id)
    {
        var schedule = await _unitOfWork.ScheduleRepo.GetByIdAsync(id);
        if (schedule == null) throw new Exception("Khong tim thay Schedule");
        schedule.Status = ScheduleStatus.Delete.ToString();
        schedule.AwardSchedule.ToList().ForEach(src => { src.Status = AwardScheduleStatus.Delete.ToString(); });
        schedule.Painting.ToList().ForEach(src => src.ScheduleId = null);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.ScheduleRepo.IsExistIdAsync(id);
    }

    public async Task<(byte[], string)> GetListCompetitorPass(Guid roundId)
    {
        var round = await _unitOfWork.RoundRepo.GetByIdAsync(roundId);
        var list = await _unitOfWork.ScheduleRepo.GetListByRoundId(roundId);
        var name = "";
        if (round!.Name == "Vòng Chung Kết")
            name = "FinalRound";
        else
            name = "PreliminaryRound";

        if (round!.EducationalLevel.Description == "Mầm Non")
            name = name + "_A";
        else
            name = name + "_B";
        var result = await _excelService.GenerateExcel(_mapper.Map<List<CompetitorViewModel>>(list), name);
        return (result, name);
    }

    public async Task<List<CompetitorViewModel>> GetListCompetitorFinalRound(Guid roundId)
    {
        var finalRound = await _unitOfWork.RoundRepo.GetByIdAsync(roundId);
        var preliminaryRound = finalRound!.EducationalLevel.Round.FirstOrDefault(src => src.Name == "Vòng Sơ Khảo");
        var list = await _unitOfWork.ScheduleRepo.GetListByRoundId(preliminaryRound!.Id);
        return _mapper.Map<List<CompetitorViewModel>>(list);
    }

    #region Create

    public async Task<bool> CreateScheduleForPreliminaryRound(ScheduleRequest schedule)
    {
        var validationResult = await ValidateScheduleRequest(schedule);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        //Get Paintings Of Preliminary round
        var listPainting = await _unitOfWork.RoundTopicRepo.ListPaintingForPreliminaryRound(schedule.RoundId);
        var round = await _unitOfWork.RoundRepo.GetRoundDetail(schedule.RoundId);
        if (round.Status != RoundStatus.Complete.ToString())
            throw new Exception("Bạn Không Thể Lên Lịch Chấm Khi Cuộc Thi Chưa Kết Thúc");
        var award = round?.EducationalLevel.Award
            .FirstOrDefault(a => a.Rank == RankAward.Preliminary.ToString());

        if (listPainting == null || listPainting!.Count < award!.Quantity)
            throw new Exception("Số Lượng Tranh Không Đủ !");

        if (award == null) throw new Exception("Award not found.");

        var quantityAward = award.Quantity;

        var result = SplitList(listPainting, schedule.ListExaminer.Count);

        //Create Schedule by number of Examiner

        for (var i = 0; i < schedule.ListExaminer.Count; i++)
        {
            var newSchedule = new Schedule();
            newSchedule.Id = Guid.NewGuid();
            newSchedule.ExaminerId = schedule.ListExaminer[i];
            newSchedule.EndDate = schedule.EndDate;
            newSchedule.RoundId = schedule.RoundId;
            newSchedule.Description = schedule.Description;
            newSchedule.Status = ScheduleStatus.Rating.ToString();

            //Add award schudele
            var newAwardSchedule = new AwardSchedule();
            newAwardSchedule.Id = Guid.NewGuid();
            newAwardSchedule.ScheduleId = newSchedule.Id;
            newAwardSchedule.AwardId = award.Id;
            newAwardSchedule.Status = AwardScheduleStatus.Rating.ToString();

            if (i == schedule.ListExaminer.Count - 1)
            {
                newAwardSchedule.Quantity = quantityAward;
            }
            else
            {
                newAwardSchedule.Quantity = (int)Math.Ceiling(award.Quantity / (double)schedule.ListExaminer.Count);
                quantityAward -= newAwardSchedule.Quantity;
            }

            newSchedule.AwardSchedule = new List<AwardSchedule>();
            newSchedule.AwardSchedule.Add(newAwardSchedule);

            await _unitOfWork.ScheduleRepo.AddAsync(newSchedule);

            //Change ScheduleID in Paiting
            result[i].ForEach(item => item.ScheduleId = newSchedule.Id);
        }

        var listExaminer = await _unitOfWork.AccountRepo.GetByIdsAsync(schedule.ListExaminer);
        foreach (var account in listExaminer)
        {
            await _mailService.SendScheduleToExaminer(account);
        }

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<bool> CreateScheduleForFinalRound(ScheduleRequest schedule)
    {
        try
        {
            var round = await _unitOfWork.RoundRepo.GetRoundDetail(schedule.RoundId);
            if (round.Status != RoundStatus.Complete.ToString())
                throw new Exception("Bạn Không Thể Lên Lịch Chấm Khi Cuộc Thi Chưa Kết Thúc");

            //Get Paintings Of Preliminary round
            var listPainting = await _unitOfWork.RoundTopicRepo.ListPaintingForFinalRound(schedule.RoundId);
            var result = SplitList(listPainting, schedule.ListExaminer.Count);

            //Get all award of educationLevel
            var award = round?.EducationalLevel.Award
                .Where(a => a.Rank != RankAward.Preliminary.ToString())
                .OrderBy(a => (RankAward)Enum.Parse(typeof(RankAward), a.Rank)).ToList();
            if (award == null) throw new Exception("Award not found.");

            var listQuantity = award.OrderBy(a => (RankAward)Enum.Parse(typeof(RankAward), a.Rank))
                .Select(a => a.Quantity).ToList();

            //Create Schedule 

            var listSchedule = new List<Schedule>();
            for (var i = 0; i < schedule.ListExaminer.Count; i++)
            {
                var newSchedule = new Schedule();
                newSchedule.Id = Guid.NewGuid();
                newSchedule.ExaminerId = schedule.ListExaminer[i];
                newSchedule.EndDate = schedule.EndDate;
                newSchedule.RoundId = schedule.RoundId;
                newSchedule.Description = schedule.Description;
                newSchedule.Status = ScheduleStatus.Rating.ToString();

                //Create AwardSchedule
                var listAwardSchedule = new List<AwardSchedule>();
                for (var j = 0; j < award.Count; j++)
                {
                    if (listQuantity[j] == 0) continue;

                    var newAwardSchedule = new AwardSchedule();
                    //In this case, the quantity of award just have 1
                    if (listQuantity[j] == 1)
                    {
                        newAwardSchedule.ScheduleId = newSchedule.Id;
                        newAwardSchedule.AwardId = award[j].Id;
                        newAwardSchedule.Quantity = listQuantity[j];
                        newAwardSchedule.Status = AwardScheduleStatus.Rating.ToString();
                        listQuantity[j] = 0;
                        listAwardSchedule.Add(newAwardSchedule);
                    }
                    else
                    {
                        newAwardSchedule.ScheduleId = newSchedule.Id;
                        newAwardSchedule.AwardId = award[j].Id;
                        newAwardSchedule.Status = AwardScheduleStatus.Rating.ToString();
                        //In this case, this is the last loop
                        if (i == schedule.ListExaminer.Count - 1)
                        {
                            newAwardSchedule.Quantity = listQuantity[j];
                        }
                        else
                        {
                            newAwardSchedule.Quantity =
                                (int)Math.Ceiling(award[j].Quantity / (double)schedule.ListExaminer.Count);
                            listQuantity[j] -= newAwardSchedule.Quantity;
                        }

                        listAwardSchedule.Add(newAwardSchedule);
                    }
                }

                newSchedule.AwardSchedule = new List<AwardSchedule>();
                newSchedule.AwardSchedule = listAwardSchedule;

                //Change ScheduleID in Paiting
                result[i].ForEach(item => item.ScheduleId = newSchedule.Id);

                //Add to list
                listSchedule.Add(newSchedule);
            }

            listSchedule.Count();
            await _unitOfWork.ScheduleRepo.AddRangeAsync(listSchedule);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public List<List<Painting>> SplitList(List<Painting> list, int n)
    {
        var result = new List<List<Painting>>();
        var chunkSize = (int)Math.Ceiling(list.Count / (double)n);

        for (var i = 0; i < n; i++)
        {
            var chunk = list.Skip(i * chunkSize).Take(chunkSize).ToList();
            if (chunk.Any()) // Nếu chunk có phần tử thì mới thêm vào result
                result.Add(chunk);
        }

        return result;
    }

    #endregion

    #region Get By Id

    public async Task<ScheduleRatingViewModel?> GetScheduleById(Guid id)
    {
        var Schedule = await _unitOfWork.ScheduleRepo.GetByIdAsync(id);
        if (Schedule == null) throw new Exception("Khong tim thay Schedule");
        return _mapper.Map<ScheduleRatingViewModel>(Schedule);
    }

    public async Task<List<ScheduleViewModel?>> GetScheduleByExaminerId(Guid id)
    {
        var schedule = await _unitOfWork.ScheduleRepo.GetByExaminerId(id);
        if (schedule == null) throw new Exception("Khong tim thay Schedule");
        return _mapper.Map<List<ScheduleViewModel>>(schedule);
    }

    public async Task<List<ScheduleWebViewModel?>> GetScheduleForWeb(Guid examinerId)
    {
        var contest = await _unitOfWork.ContestRepo.GetNearestContestInformationAsync();
        if (contest == null) throw new Exception("Khong tim thay");
        var educationalLevel = await _unitOfWork.EducationalLevelRepo.GetEducationalLevelByContestId(contest!.Id);
        foreach (var level in educationalLevel)
        foreach (var round in level.Round)
        foreach (var schedule in round.Schedule)
            if (schedule.ExaminerId != examinerId)
                round.Schedule.Remove(schedule);

        if (educationalLevel == null) throw new Exception("Khong tim thay");

        return _mapper.Map<List<ScheduleWebViewModel>>(educationalLevel);
    }

    #endregion

    #region Rating

    public async Task<bool> RatingPreliminaryRound(RatingRequest ratingPainting)
    {
        var validationResult = await ValidateRatingRequest(ratingPainting);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        //Get schedule with list painting 
        var schedules = await _unitOfWork.ScheduleRepo.GetByIdAsync(ratingPainting.ScheduleId);
        if (schedules.Painting.Any(p => p.Status != PaintingStatus.Accepted.ToString())) return false;

        if (ratingPainting.Paintings.Except(schedules.Painting.Select(p => p.Id)).ToList().Any())
            throw new Exception("Have ID not Exist In schedule");

        //Get painting have status is FinalRound
        var listPass = schedules.Painting.Where(p => ratingPainting.Paintings.Contains(p.Id)).ToList();
        var listNotPass = schedules.Painting.Where(p => !ratingPainting.Paintings.Contains(p.Id)).ToList();

        //Get Award from Award schedule
        var awardSchedule = schedules.AwardSchedule.FirstOrDefault();

        listPass.ForEach(p => p.Status = PaintingStatus.Pass.ToString());
        listPass.ForEach(p => p.AwardId = awardSchedule!.AwardId);
        listNotPass.ForEach(p => p.Status = PaintingStatus.NotPass.ToString());
        schedules.Painting.ToList().ForEach(p => p.FinalDecisionTimestamp = DateTime.Now);
        schedules.AwardSchedule.First().Status = AwardScheduleStatus.Done.ToString();
        schedules.Status = ScheduleStatus.Done.ToString();
        if (listPass.Count != schedules.AwardSchedule.First().Quantity)
            throw new Exception("The Quantity of paiting is wrong");


        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RatingFirstPrize(RatingRequest ratingPainting)
    {
        var validationResult = await ValidateRatingRequest(ratingPainting);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        //Get schedule with list painting 
        var schedules = await _unitOfWork.ScheduleRepo.GetByIdAsync(ratingPainting.ScheduleId);

        if (schedules!.Status == ScheduleStatus.Done.ToString()) throw new Exception("This schedules has Done");

        //Get painting have status is FinalRound
        var listPainting = schedules.Painting.Where(p => p.Status == PaintingStatus.FinalRound.ToString()).ToList();
        //Get Award from Award schedule
        var awardSchedule =
            schedules.AwardSchedule.FirstOrDefault(a => a.Award.Rank == RankAward.FirstPrize.ToString());

        if (awardSchedule!.Status == AwardScheduleStatus.Done.ToString()) throw new Exception("This Prize has Done");

        //Check Have any id from request don't exist in schedule
        if (ratingPainting.Paintings.Except(listPainting.Select(p => p.Id)).ToList().Any())
            throw new Exception("Have ID not Exist In schedule");


        //Create var to call all rated painting
        var listPass = schedules.Painting.Where(p => ratingPainting.Paintings.Contains(p.Id)).ToList();

        listPass.ForEach(p => p.Status = PaintingStatus.HasPrizes.ToString());
        listPass.ForEach(p => p.AwardId = awardSchedule.Award.Id);
        schedules.Painting.ToList().ForEach(p => p.FinalDecisionTimestamp = DateTime.Now);
        if (listPass.Count != awardSchedule.Quantity)
            throw new Exception($"The Quantity of First Prize is {awardSchedule.Quantity}");

        awardSchedule.Status = AwardScheduleStatus.Done.ToString();

        if (!schedules.AwardSchedule.Any(a => a.Status == AwardScheduleStatus.Rating.ToString()))
            schedules.Status = ScheduleStatus.Done.ToString();

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RatingSecondPrize(RatingRequest ratingPainting)
    {
        var validationResult = await ValidateRatingRequest(ratingPainting);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        //Get schedule with list painting 
        var schedules = await _unitOfWork.ScheduleRepo.GetByIdAsync(ratingPainting.ScheduleId);

        if (schedules!.Status == ScheduleStatus.Done.ToString()) throw new Exception("This schedules has Done");

        //Get painting have status is FinalRound
        var listPainting = schedules.Painting.Where(p => p.Status == PaintingStatus.FinalRound.ToString()).ToList();
        //Get Award from Award schedule
        var awardSchedule =
            schedules.AwardSchedule.FirstOrDefault(a => a.Award.Rank == RankAward.SecondPrize.ToString());

        if (awardSchedule!.Status == AwardScheduleStatus.Done.ToString()) throw new Exception("This Prize has Done");

        //Check Have any id from request don't exist in schedule
        if (ratingPainting.Paintings.Except(listPainting.Select(p => p.Id)).ToList().Any())
            throw new Exception("Have ID not Exist In schedule");


        //Create var to call all rated painting
        var listPass = schedules.Painting.Where(p => ratingPainting.Paintings.Contains(p.Id)).ToList();

        listPass.ForEach(p => p.Status = PaintingStatus.HasPrizes.ToString());
        listPass.ForEach(p => p.AwardId = awardSchedule.Award.Id);
        schedules.Painting.ToList().ForEach(p => p.FinalDecisionTimestamp = DateTime.Now);
        if (listPass.Count != awardSchedule.Quantity)
            throw new Exception($"The Quantity of Second Prize is {awardSchedule.Quantity}");

        awardSchedule.Status = AwardScheduleStatus.Done.ToString();

        if (!schedules.AwardSchedule.Any(a => a.Status == AwardScheduleStatus.Rating.ToString()))
            schedules.Status = ScheduleStatus.Done.ToString();

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RatingThirdPrize(RatingRequest ratingPainting)
    {
        var validationResult = await ValidateRatingRequest(ratingPainting);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        //Get schedule with list painting 
        var schedules = await _unitOfWork.ScheduleRepo.GetByIdAsync(ratingPainting.ScheduleId);

        if (schedules.Status == ScheduleStatus.Done.ToString()) throw new Exception("This schedules has Done");

        //Get painting have status is FinalRound
        var listPainting = schedules.Painting.Where(p => p.Status == PaintingStatus.FinalRound.ToString()).ToList();
        //Get Award from Award schedule
        var awardSchedule =
            schedules.AwardSchedule.FirstOrDefault(a => a.Award.Rank == RankAward.ThirdPrize.ToString());

        if (awardSchedule.Status == AwardScheduleStatus.Done.ToString()) throw new Exception("This Prize has Done");

        //Check Have any id from request don't exist in schedule
        if (ratingPainting.Paintings.Except(listPainting.Select(p => p.Id)).ToList().Any())
            throw new Exception("Have ID not Exist In schedule");


        //Create var to call all rated painting
        var listPass = schedules.Painting.Where(p => ratingPainting.Paintings.Contains(p.Id)).ToList();

        listPass.ForEach(p => p.Status = PaintingStatus.HasPrizes.ToString());
        listPass.ForEach(p => p.AwardId = awardSchedule.Award.Id);
        schedules.Painting.ToList().ForEach(p => p.FinalDecisionTimestamp = DateTime.Now);
        if (listPass.Count != awardSchedule.Quantity)
            throw new Exception($"The Quantity of Third Prize is {awardSchedule.Quantity}");

        awardSchedule.Status = AwardScheduleStatus.Done.ToString();

        if (!schedules.AwardSchedule.Any(a => a.Status == AwardScheduleStatus.Rating.ToString()))
            schedules.Status = ScheduleStatus.Done.ToString();

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RatingConsolationPrize(RatingRequest ratingPainting)
    {
        var validationResult = await ValidateRatingRequest(ratingPainting);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        //Get schedule with list painting 
        var schedules = await _unitOfWork.ScheduleRepo.GetByIdAsync(ratingPainting.ScheduleId);

        if (schedules.Status == ScheduleStatus.Done.ToString()) throw new Exception("This schedules has Done");

        //Get painting have status is FinalRound
        var listPainting = schedules.Painting.Where(p => p.Status == PaintingStatus.FinalRound.ToString()).ToList();
        //Get Award from Award schedule
        var awardSchedule =
            schedules.AwardSchedule.FirstOrDefault(a => a.Award.Rank == RankAward.ConsolationPrize.ToString());

        if (awardSchedule.Status == AwardScheduleStatus.Done.ToString()) throw new Exception("This Prize has Done");

        //Check Have any id from request don't exist in schedule
        if (ratingPainting.Paintings.Except(listPainting.Select(p => p.Id)).ToList().Any())
            throw new Exception("Have ID not Exist In schedule");


        //Create var to call all rated painting
        var listPass = schedules.Painting.Where(p => ratingPainting.Paintings.Contains(p.Id)).ToList();

        listPass.ForEach(p => p.Status = PaintingStatus.HasPrizes.ToString());
        listPass.ForEach(p => p.AwardId = awardSchedule.Award.Id);
        schedules.Painting.ToList().ForEach(p => p.FinalDecisionTimestamp = DateTime.Now);
        if (listPass.Count != awardSchedule.Quantity)
            throw new Exception($"The Quantity of Consolation Prize is {awardSchedule.Quantity}");

        awardSchedule.Status = AwardScheduleStatus.Done.ToString();

        if (!schedules.AwardSchedule.Any(a => a.Status == AwardScheduleStatus.Rating.ToString()))
            schedules.Status = ScheduleStatus.Done.ToString();

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    #endregion

    #region Validate

    public async Task<ValidationResult> ValidateScheduleRequest(ScheduleRequest schedule)
    {
        return await _validatorFactory.ScheduleRequestValidator.ValidateAsync(schedule);
    }

    public async Task<ValidationResult> ValidateScheduleUpdateRequest(ScheduleUpdateRequest scheduleUpdate)
    {
        return await _validatorFactory.ScheduleUpdateRequestValidator.ValidateAsync(scheduleUpdate);
    }

    public async Task<ValidationResult> ValidateRatingRequest(RatingRequest painting)
    {
        return await _validatorFactory.RatingRequestValidator.ValidateAsync(painting);
    }

    #endregion
}