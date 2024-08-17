using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Notification;
using Application.SendModels.Schedule;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.ScheduleViewModels;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class ScheduleService : IScheduleService
{
    private readonly IExcelService _excelService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;
    private readonly IMailService _mailService;
    private readonly INotificationService _notificationService;

    public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper, IValidatorFactory validatorFactory,
        IExcelService excelService, IMailService mailService, INotificationService notificationService)
    {
        _mailService = mailService;
        _excelService = excelService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validatorFactory = validatorFactory;
        _notificationService = notificationService;
    }

    #region Get For Website

    public async Task<List<ListScheduleViewModel>> GetListScheduleByContestId(Guid id)
    {
        var listSchedule = await _unitOfWork.RoundRepo.GetScheduleByContestId(id);
        var result = _mapper.Map<List<ListScheduleViewModel>>(listSchedule);
        foreach (var s in result)
        {
            s.TotalPainting = await _unitOfWork.PaintingRepo.GetNumPaintingInRound(s.RoundId);
            s.PaintingNoSchedule = await _unitOfWork.PaintingRepo.GetNumPaintingInRoundIsNotHaveSchedule(s.RoundId);
            s.PaintingWithSchedule = await _unitOfWork.PaintingRepo.GetNumPaintingInRoundIsHaveSchedule(s.RoundId);
            foreach (var l in s.Schedules)
            {
                l.JudgeCount = await _unitOfWork.PaintingRepo.GetNumPaintingInSchedule(l.Id);
            }
        }
        return result;
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
            throw new ValidationException(validationResult.Errors);
        //Get Painting 
        foreach (var e in schedule.ListExaminer)
        {
            var listPainting = await _unitOfWork.RoundTopicRepo.ListPaintingForPreliminaryRound(schedule.RoundId, schedule.JudgedCount);
            var round = await _unitOfWork.RoundRepo.GetByIdAsync(schedule.RoundId);
            var award = round?.Award.ToList();
            if (award == null) throw new Exception("Không có giải nào để lên lịch chấm.");

            var newSchedule = new Schedule();
            newSchedule.Id = Guid.NewGuid();
            newSchedule.ExaminerId = e;
            newSchedule.EndDate = schedule.EndDate;
            newSchedule.RoundId = schedule.RoundId;
            newSchedule.Description = schedule.Description;
            newSchedule.Status = ScheduleStatus.Rating.ToString();
            newSchedule.CreatedBy = schedule.CurrentUserId;

            //Add award schudele
            var listAwardSchedule = new List<AwardSchedule>();
            foreach (var a in schedule.Awards)
            {
                var newAwardSchedule = new AwardSchedule();
                newAwardSchedule.ScheduleId = newSchedule.Id;
                newAwardSchedule.AwardId = a.AwardId;
                newAwardSchedule.Quantity = a.AwardCount;
                newAwardSchedule.Status = AwardScheduleStatus.Rating.ToString();
                newAwardSchedule.CreatedBy = schedule.CurrentUserId;
                listAwardSchedule.Add(newAwardSchedule);
            }
            newSchedule.AwardSchedule = listAwardSchedule;

            foreach (var p in listPainting)
            {
                p.ScheduleId = newSchedule.Id;
            }

            await _unitOfWork.ScheduleRepo.AddAsync(newSchedule);
            var examiner = await _unitOfWork.AccountRepo.GetByIdAsync(e);
            await _mailService.SendScheduleToExaminer(examiner);
            await _unitOfWork.SaveChangesAsync();
        }


        return true;
    }

    public async Task<bool> CreateSchedule(ScheduleForFinalRequest schedule)
    {

        /*var validationResult = await ValidateScheduleRequest(schedule);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);*/

        var round = await _unitOfWork.RoundRepo.GetByIdAsync(schedule.RoundId);
        foreach (var e in schedule.ListExaminer)
        {
            //Get Painting 
            var listPainting = await _unitOfWork.RoundTopicRepo.ListPaintingForFinalRound(schedule.RoundId, schedule.JudgeCount);
            var award = round?.Award.ToList();
            if (award == null) throw new Exception("Không có giải nào để lên lịch chấm.");

            //Create new Schedule
            var newSchedule = new Schedule();
            newSchedule.Id = Guid.NewGuid();
            newSchedule.ExaminerId = e;
            newSchedule.EndDate = schedule.EndDate;
            newSchedule.RoundId = schedule.RoundId;
            newSchedule.Description = schedule.Description;
            newSchedule.Status = ScheduleStatus.Rating.ToString();
            newSchedule.CreatedBy = schedule.CurrentUserId;

            var listAwardSchedule = new List<AwardSchedule>();
            foreach (var a in schedule.Awards)
            {
                var newAwardSchedule = new AwardSchedule();
                newAwardSchedule.ScheduleId = newSchedule.Id;
                newAwardSchedule.AwardId = a.AwardId;
                newAwardSchedule.Quantity = a.AwardCount;
                newAwardSchedule.Status = AwardScheduleStatus.Rating.ToString();
                newAwardSchedule.CreatedBy = schedule.CurrentUserId;
                listAwardSchedule.Add(newAwardSchedule);
            }
            newSchedule.AwardSchedule = listAwardSchedule;

            foreach (var p in listPainting)
            {
                p.ScheduleId = newSchedule.Id;
                p.RatingStatus = RatingStatus.InProcess.ToString();
            }

            await _unitOfWork.ScheduleRepo.AddAsync(newSchedule);
            var examiner = await _unitOfWork.AccountRepo.GetByIdAsync(e);
            await _mailService.SendScheduleToExaminer(examiner);

            //Create Notification
            NotificationRequest notification = new NotificationRequest("Lịch Chấm Mới", "Bạn có lịch chấm thi mới xin hãy vào Phần Lịch Chấm để xem chi tiết!", e);

            await _notificationService.CreateNotification(notification);
            await _unitOfWork.SaveChangesAsync();
        }
        
        return true;

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

    public async Task<List<ScheduleWebViewModel?>> GetScheduleForWeb(Guid examinerId/*,Guid contestId*/)
    {
        var contest = await _unitOfWork.ContestRepo.GetNearestContestInformationAsync();
        if (contest == null) throw new Exception("Không tìm thấy Contest");
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

    #region New Rating

    public async Task<bool> RatingFinalRound(RatingRequest ratingPainting)
    {
        var validationResult = await ValidateRatingRequest(ratingPainting);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        //Get schedule 
        var schedules = await _unitOfWork.ScheduleRepo.GetByIdAsync(ratingPainting.ScheduleId);

        if (schedules!.Status == ScheduleStatus.Done.ToString()) throw new Exception("Đã chấm bài");

        //Get painting have status is FinalRound
        // var listPainting = schedules.Painting.Where(p => p.Status == PaintingStatus.FinalRound.ToString()).ToList();
        //Get Award from Award schedule
        //var awardSchedule = schedules.AwardSchedule.FirstOrDefault(a => a.AwardId == ratingPainting.AwardId);

        //if (awardSchedule!.Status == AwardScheduleStatus.Done.ToString()) throw new Exception("Đã chấm bài");

        //Check Have any id from request don't exist in schedule
        /* if (ratingPainting.Paintings.Select(p => p.PaintingId).Except(listPainting.Select(p => p.Id)).ToList().Any())
             throw new Exception("Có tranh không tồn tại");*/


        //Create var to call all rated painting
        var listPass = schedules.Painting.Where(p => ratingPainting.Paintings.Select(r => r.PaintingId).Contains(p.Id)).ToList();

        /*if (listPass.Count != awardSchedule.Quantity)
            throw new Exception($"Số lượng giải thưởng: {awardSchedule.Quantity}! Vui lòng kiểm tra lại đúng số lượng");
*/
        foreach (var p in listPass)
        {
            p.Status = PaintingStatus.HasPrizes.ToString();
           // p.AwardId = ratingPainting.AwardId;
        }
        schedules.Painting.ToList().ForEach(p => p.FinalDecisionTimestamp = DateTime.Now);


       // awardSchedule.Status = AwardScheduleStatus.Done.ToString();

        if (!schedules.AwardSchedule.Any(a => a.Status == AwardScheduleStatus.Rating.ToString()))
            schedules.Status = ScheduleStatus.Done.ToString();

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    #endregion
    
    #region Rating 

    public async Task<bool> RatingPainting(RatingRequest ratingPainting)
    {
        /*var validationResult = await ValidateRatingRequest(ratingPainting);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);*/

        //Get schedule 
        var schedules = await _unitOfWork.ScheduleRepo.GetByIdAsync(ratingPainting.ScheduleId);

        if (schedules!.Status == ScheduleStatus.Done.ToString()) throw new Exception("Đã chấm bài");

        //Get painting have status is FinalRound
        // var listPainting = schedules.Painting.Where(p => p.Status == PaintingStatus.FinalRound.ToString()).ToList();
        foreach (var p in ratingPainting.Paintings)
        {
            AwardSchedule awardSchedule = new AwardSchedule();
            //Get Award from Award schedule
            if (p.AwardId.HasValue)
            {
                awardSchedule = schedules.AwardSchedule.FirstOrDefault(a => a.AwardId == p.AwardId);
                if (awardSchedule == null) throw new Exception($"Không tìm thấy giải thường {p.AwardId}");
                if (awardSchedule!.Status == AwardScheduleStatus.Done.ToString()) throw new Exception($"Đã hết giải {p.AwardId}");
            }

            //Check Have any id from request don't exist in schedule
            /* if (ratingPainting.Paintings.Select(p => p.PaintingId).Except(listPainting.Select(p => p.Id)).ToList().Any())
                 throw new Exception("Có tranh không tồn tại");*/


            //Create var to call all rated painting
            var painting = schedules.Painting.FirstOrDefault(x => x.Id == p.PaintingId);

            if (painting == null)
                throw new Exception($"Không tìm thấy bài dự thi {p.PaintingId} trong danh sách những bài được chấm");


            painting.Status = PaintingStatus.HasPrizes.ToString();
            painting.AwardId = p.AwardId;
            painting.JudgementReason = p.Reason;
            painting.FinalDecisionTimestamp = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();

            if (p.AwardId.HasValue)
            {
                var paintingAwardCount = await _unitOfWork.PaintingRepo.CountPaintingHaveAward(ratingPainting.ScheduleId, p.AwardId.Value);
                if (awardSchedule.Quantity == paintingAwardCount)
                    awardSchedule.Status = AwardScheduleStatus.Done.ToString();
            }

            if (!schedules.AwardSchedule.Any(a => a.Status == AwardScheduleStatus.Rating.ToString()))
                schedules.Status = ScheduleStatus.Done.ToString();

            
            await _unitOfWork.SaveChangesAsync();
        }

        return true;
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
        if (schedules.Painting.Any(p => p.Status != PaintingStatus.Accepted.ToString()))
            throw new Exception("Có tranh đang ở trạng thái không hợp lệ");

        if (ratingPainting.Paintings.Select(p => p.PaintingId).Except(schedules.Painting.Select(p => p.Id)).ToList().Any())
            throw new Exception("Có tranh không tồn tại trong lịch chấm");


        /*//Get painting have status is Accepted
        var listPass = schedules.Painting
            .Where(p => ratingPainting.Paintings
                .Where(r => r.IsPass) // Lọc những painting có IsPassed = true
                .Select(r => r.PaintingId) // Chọn các Id
                .Contains(p.Id)) // Kiểm tra xem Id của painting có trong danh sách không
            .ToList();
        var listNotPass = schedules.Painting
            .Where(p => ratingPainting.Paintings
                .Where(r => !r.IsPass) // Lọc những painting có IsPassed = false
                .Select(r => r.PaintingId) // Chọn các Id
                .Contains(p.Id)) // Kiểm tra xem Id của painting có trong danh sách không
            .ToList();*/

        //Get Award from Award schedule
        var awardSchedule = schedules.AwardSchedule.FirstOrDefault();

        /*listPass.ForEach(p =>
        {
            p.Status = PaintingStatus.Pass.ToString();
            p.AwardId = ratingPainting.AwardId;
            p.JudgementReason = ratingPainting.Paintings.FirstOrDefault(r => r.PaintingId == p.Id)?.Reason;
        });
        listNotPass.ForEach(p =>
        {
            p.Status = PaintingStatus.NotPass.ToString();
            p.JudgementReason = ratingPainting.Paintings.FirstOrDefault(r => r.PaintingId == p.Id)?.Reason;
        });
        schedules.Painting.ToList().ForEach(p => p.FinalDecisionTimestamp = DateTime.Now);
        schedules.AwardSchedule.First().Status = AwardScheduleStatus.Done.ToString();
        schedules.Status = ScheduleStatus.Done.ToString();
        if (listPass.Count != schedules.AwardSchedule.First().Quantity)
            throw new Exception("Số lượng tranh không đúng");*/


        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RatingFirstPrize(RatingRequest ratingPainting)
    {
        var validationResult = await ValidateRatingRequest(ratingPainting);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        //Get painting with schedule 
        var schedules = await _unitOfWork.ScheduleRepo.GetByIdAsync(ratingPainting.ScheduleId);

        if (schedules!.Status == ScheduleStatus.Done.ToString()) throw new Exception("This schedules has Done");

        //Get painting have status is FinalRound
        var listPainting = schedules.Painting.Where(p => p.Status == PaintingStatus.FinalRound.ToString()).ToList();
        //Get Award from Award schedule
        var awardSchedule =
            schedules.AwardSchedule.FirstOrDefault(a => a.Award.Rank == RankAward.FirstPrize.ToString());

        if (awardSchedule!.Status == AwardScheduleStatus.Done.ToString()) throw new Exception("This Prize has Done");

        //Check Have any id from request don't exist in schedule
        if (ratingPainting.Paintings.Select(p => p.PaintingId).Except(listPainting.Select(p => p.Id)).ToList().Any())
            throw new Exception("Have ID not Exist In schedule");


        //Create var to call all rated painting
        var listPass = schedules.Painting.Where(p => ratingPainting.Paintings.Select(r => r.PaintingId).Contains(p.Id)).ToList();

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
        if (ratingPainting.Paintings.Select(p => p.PaintingId).Except(listPainting.Select(p => p.Id)).ToList().Any())
            throw new Exception("Have ID not Exist In schedule");


        //Create var to call all rated painting
        var listPass = schedules.Painting.Where(p => ratingPainting.Paintings.Select(p => p.PaintingId).Contains(p.Id)).ToList();

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
        if (ratingPainting.Paintings.Select(p => p.PaintingId).Except(listPainting.Select(p => p.Id)).ToList().Any())
            throw new Exception("Have ID not Exist In schedule");


        //Create var to call all rated painting
        var listPass = schedules.Painting.Where(p => ratingPainting.Paintings.Select(p => p.PaintingId).Contains(p.Id)).ToList();

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
        if (ratingPainting.Paintings.Select(p => p.PaintingId).Except(listPainting.Select(p => p.Id)).ToList().Any())
            throw new Exception("Have ID not Exist In schedule");


        //Create var to call all rated painting
        var listPass = schedules.Painting.Where(p => ratingPainting.Paintings.Select(p => p.PaintingId).Contains(p.Id)).ToList();

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