using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Notification;
using Application.SendModels.Schedule;
using Application.ViewModels.AwardViewModels;
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
    private readonly IMailService _mailService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;
    private readonly ISortAwardService _sortAwardService;

    public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper, IValidatorFactory validatorFactory,
        IExcelService excelService, IMailService mailService, INotificationService notificationService, ISortAwardService sortAwardService)
    {
        _mailService = mailService;
        _excelService = excelService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validatorFactory = validatorFactory;
        _notificationService = notificationService;
        _sortAwardService = sortAwardService;
    }

    #region Get For Website

    public async Task<List<ListScheduleResponse>> GetListScheduleByContestId(Guid id)
    {
        var round = await _unitOfWork.RoundRepo.GetScheduleByContestId(id);
        var result = _mapper.Map<List<ListScheduleResponse>>(round);
        foreach (var s in result)
        {
            s.TotalPainting = await _unitOfWork.PaintingRepo.GetNumPaintingInRound(s.RoundId);
            s.PaintingNoSchedule = await _unitOfWork.PaintingRepo.GetNumPaintingInRoundIsNotHaveSchedule(s.RoundId);
            s.PaintingWithSchedule = await _unitOfWork.PaintingRepo.GetNumPaintingInRoundIsHaveSchedule(s.RoundId);
            foreach (var l in s.Schedules)
            {
                l.JudgeCount = await _unitOfWork.PaintingRepo.GetNumPaintingInSchedule(l.Id);
                var awards = _sortAwardService.SortAwards(_mapper.Map<List<Award>>(l.Awards));
                l.Awards = _mapper.Map<List<ListAwardInScheduleResponse>>(awards);
            }
        }

        return result;
    }

    #endregion

    #region Get All

    public async Task<(List<ScheduleRatingResponse>, int)> GetListSchedule(ListModels listModels)
    {
        var list = await _unitOfWork.ScheduleRepo.GetAllAsync();
        if (list.Count == 0) throw new Exception("Không tìm thấy lịch chấm nào");
        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        var result = list.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (_mapper.Map<List<ScheduleRatingResponse>>(result), totalPages);
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
        if (schedule == null) throw new Exception("Không tìm thấy lịch chấm");
        _mapper.Map(updateSchedule, schedule);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Delete

    public async Task<bool> DeleteSchedule(Guid id)
    {
        var schedule = await _unitOfWork.ScheduleRepo.GetByIdAsync(id);
        if (schedule == null) throw new Exception("Không tìm thấy lịch chấm");
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

    #region Confirm Rating

    public async Task<bool> ConfirmRating(Guid id)
    {
        var schedule = await _unitOfWork.ScheduleRepo.GetByIdAsync(id);
        if (schedule == null) throw new Exception("Không tìm thấy lịch chấm.");
        /*        if (schedule.AwardSchedule.Any(a => a.Status == AwardScheduleStatus.Rating.ToString()))
        {
            throw new Exception("Còn giải chưa được chấm hết.");
        }*/
        schedule.Status = ScheduleStatus.Done.ToString();
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Rating

    public async Task<bool> RatingPainting(RatingSendModel ratingPainting)
    {
        var validationResult = await ValidateRatingRequest(ratingPainting);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);

        //Get schedule 
        var schedule = await _unitOfWork.ScheduleRepo.GetByIdAsync(ratingPainting.ScheduleId);

        if (schedule!.Status == ScheduleStatus.Done.ToString()) throw new Exception("Đã chấm bài");

        foreach (var p in ratingPainting.Paintings)
        {
            var awardSchedule = new AwardSchedule();
            //Get Award from Award schedule
            if (p.AwardId.HasValue)
            {
                awardSchedule = schedule.AwardSchedule.FirstOrDefault(a => a.AwardId == p.AwardId);
                if (awardSchedule == null) throw new Exception($"Không tìm thấy giải thường. Vui lòng thử lại");
                if (awardSchedule!.Status == AwardScheduleStatus.Done.ToString())
                    throw new Exception($"Có giải thưởng đã hết");
            }


            //Create var to call all rated painting
            var painting = schedule.Painting.FirstOrDefault(x => x.Id == p.PaintingId);

            if (painting == null)
                throw new Exception($"Có bài dự thi không nằm trong lịch chấm");

            if (painting.AwardId.HasValue)
                if (painting.AwardId != p.AwardId)
                    if (painting.Award.AwardSchedule.FirstOrDefault(a => a.AwardId == painting.AwardId).Status ==
                        AwardScheduleStatus.Done.ToString())
                        painting.Award.AwardSchedule.FirstOrDefault(a => a.AwardId == painting.AwardId).Status =
                            AwardScheduleStatus.Rating.ToString();

            if (schedule!.Round!.Name!.Contains("Vòng Chung Kết"))
            {
                if (p.AwardId != null)
                {
                    painting.RatingStatus = PaintingStatus.HasPrizes.ToString();
                    painting.AwardId = p.AwardId;
                }
                else
                {
                    painting.RatingStatus = PaintingStatus.FinalRound.ToString();
                }
            }
            else
            {
                if (p.AwardId != null)
                {
                    painting.RatingStatus = PaintingStatus.Pass.ToString();
                    painting.AwardId = p.AwardId;
                }
                else
                {
                    painting.RatingStatus = PaintingStatus.NotPass.ToString();
                }
            }

            painting.JudgementReason = p.Reason;
            painting.FinalDecisionTimestamp = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();

            if (p.AwardId.HasValue)
            {
                var paintingAwardCount =
                    await _unitOfWork.PaintingRepo.CountPaintingHaveAward(ratingPainting.ScheduleId, p.AwardId.Value);
                if (awardSchedule.Quantity == paintingAwardCount)
                    awardSchedule.Status = AwardScheduleStatus.Done.ToString();
            }

            await _unitOfWork.SaveChangesAsync();
        }

        return true;
    }

    #endregion

    #region Create

    public async Task<bool> CreateScheduleForQualifyingRound(ScheduleForPreliminaryRequest schedule)
    {
        var validationResult = await ValidateScheduleForPreliminaryRequest(schedule);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        //Get Painting 
        foreach (var e in schedule.ListExaminer)
        {
            var listPainting =
                await _unitOfWork.RoundTopicRepo.ListPaintingForQualifyingRound(schedule.RoundId, schedule.JudgedCount);
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

            foreach (var p in listPainting) p.ScheduleId = newSchedule.Id;

            await _unitOfWork.ScheduleRepo.AddAsync(newSchedule);
            var examiner = await _unitOfWork.AccountRepo.GetByIdAsync(e);
            await _mailService.SendScheduleToExaminer(examiner);
            await _unitOfWork.SaveChangesAsync();
        }


        return true;
    }

    public async Task<bool> CreateScheduleForFinal(ScheduleForFinalRequest schedule)
    {
        var validationResult = await ValidateScheduleForFinalRequest(schedule);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var round = await _unitOfWork.RoundRepo.GetByIdAsync(schedule.RoundId);
        foreach (var e in schedule.ListExaminer)
        {
            //Get Painting 
            var listPainting =
                await _unitOfWork.RoundTopicRepo.ListPaintingForFinalRound(schedule.RoundId, schedule.JudgedCount);
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
            var notification = new NotificationRequest("Lịch Chấm Mới",
                "Bạn có lịch chấm thi mới xin hãy vào Phần Lịch Chấm để xem chi tiết!", e);

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

    public async Task<ScheduleRatingResponse?> GetScheduleById(Guid id)
    {
        var Schedule = await _unitOfWork.ScheduleRepo.GetByIdAsync(id);
        if (Schedule == null) throw new Exception("Không tìm thấy lịch chấm");
        return _mapper.Map<ScheduleRatingResponse>(Schedule);
    }

    public async Task<List<ScheduleResponse?>> GetScheduleByExaminerId(Guid id)
    {
        var schedule = await _unitOfWork.ScheduleRepo.GetByExaminerId(id);
        if (schedule == null) throw new Exception("Không tìm thấy lịch chấm");
        return _mapper.Map<List<ScheduleResponse>>(schedule);
    }

    public async Task<List<ScheduleWebResponse?>> GetScheduleForWeb(Guid examinerId /*,Guid contestId*/)
    {
        var contest = await _unitOfWork.ContestRepo.GetNearestContestInformationAsync();
        if (contest == null) throw new Exception("Không tìm thấy cuộc thi");
        var educationalLevel = await _unitOfWork.EducationalLevelRepo.GetEducationalLevelByContestId(contest!.Id);
        foreach (var level in educationalLevel)
        foreach (var round in level.Round)
        foreach (var schedule in round.Schedule)
            if (schedule.ExaminerId != examinerId)
                round.Schedule.Remove(schedule);

        if (educationalLevel == null) throw new Exception("Hệ thống bị lỗi vui lòng thử lại");

        return _mapper.Map<List<ScheduleWebResponse>>(educationalLevel);
    }

    #endregion

    #region CreateScheduleForQualifyingRound2 Auto assign

    public async Task<bool> CreateScheduleForQualifyingRound2(CreateScheduleAutoAssignRequest schedule)
    {
        /*var validationResult = await ValidateScheduleForPreliminaryRequest(schedule);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);*/
        var listPainting = await _unitOfWork.RoundTopicRepo.ListPaintingForScheduleQualifyingRound(schedule.RoundId);

        var listAward = await _unitOfWork.AwardRepo.GetAwardsByRoundId(schedule.RoundId);
        var listAwardSchedule = await _unitOfWork.AwardScheduleRepo.GetAwardScheduleByRoundId(schedule.RoundId);
        
        if (listPainting.Count == 0) throw new Exception("Không có tranh nào để lên lịch chấm");

        var finalAward = DistributeAwardsToExaminers(listAward, listAwardSchedule, schedule.ListExaminer);
        var paintingAssignments = AssignPaintingsToExaminers(listPainting, schedule.ListExaminer);
        //Get Painting 
        foreach (var e in paintingAssignments)
        {
            
            var round = await _unitOfWork.RoundRepo.GetByIdAsync(schedule.RoundId);
            var award = round?.Award.ToList();
            if (award == null) throw new Exception("Không có giải để lên lịch chấm.");

            var newSchedule = new Schedule();
            newSchedule.Id = Guid.NewGuid();
            newSchedule.ExaminerId = e.Key;
            newSchedule.EndDate = schedule.EndDate;
            newSchedule.RoundId = schedule.RoundId;
            newSchedule.Description = schedule.Description;
            newSchedule.Status = ScheduleStatus.Rating.ToString();
            newSchedule.CreatedBy = schedule.CurrentUserId;

            //Add award schudele
            var listAwardScheduleForExaminer = new List<AwardSchedule>();
            if (finalAward.ContainsKey(e.Key))
            {
                foreach (var a in finalAward[e.Key])
                {
                    var newAwardSchedule = new AwardSchedule
                    {
                        ScheduleId = newSchedule.Id,
                        AwardId = a.AwardId,
                        Quantity = a.AwardCount,
                        Status = AwardScheduleStatus.Rating.ToString(),
                        CreatedBy = schedule.CurrentUserId
                    };
                    listAwardScheduleForExaminer.Add(newAwardSchedule);
                }
            }
            newSchedule.AwardSchedule = listAwardScheduleForExaminer;

            foreach (var p in e.Value) p.ScheduleId = newSchedule.Id;

            await _unitOfWork.ScheduleRepo.AddAsync(newSchedule);
            var examiner = await _unitOfWork.AccountRepo.GetByIdAsync(e.Key);
            await _mailService.SendScheduleToExaminer(examiner);
            await _unitOfWork.SaveChangesAsync();
        }


        return true;
    }
    #endregion

    #region CreateScheduleForFinalRound2

    public async Task<bool> CreateScheduleForFinalRound2(CreateScheduleAutoAssignRequest schedule)
    {
        /*var validationResult = await ValidateScheduleForFinalRequest(schedule);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);*/
        var listPainting = await _unitOfWork.RoundTopicRepo.ListPaintingForScheduleFinalRound(schedule.RoundId);

        if (listPainting.Count == 0) throw new Exception("Không có tranh nào để lên lịch chấm");
        var listAward = await _unitOfWork.AwardRepo.GetAwardsByRoundId(schedule.RoundId);
        var listAwardSchedule = await _unitOfWork.AwardScheduleRepo.GetAwardScheduleByRoundId(schedule.RoundId);

        var finalAward = DistributeAwardsToExaminers(listAward, listAwardSchedule, schedule.ListExaminer);
        var paintingAssignments = AssignPaintingsToExaminers(listPainting, schedule.ListExaminer);
        //Get Painting 
        foreach (var e in paintingAssignments)
        {

            var round = await _unitOfWork.RoundRepo.GetByIdAsync(schedule.RoundId);
            var award = round?.Award.ToList();
            if (award == null) throw new Exception("Không có giải để lên lịch chấm.");

            var newSchedule = new Schedule();
            newSchedule.Id = Guid.NewGuid();
            newSchedule.ExaminerId = e.Key;
            newSchedule.EndDate = schedule.EndDate;
            newSchedule.RoundId = schedule.RoundId;
            newSchedule.Description = schedule.Description;
            newSchedule.Status = ScheduleStatus.Rating.ToString();
            newSchedule.CreatedBy = schedule.CurrentUserId;

            //Add award schudele
            var listAwardScheduleForExaminer = new List<AwardSchedule>();
            if (finalAward.ContainsKey(e.Key))
            {
                foreach (var a in finalAward[e.Key])
                {
                    var newAwardSchedule = new AwardSchedule
                    {
                        ScheduleId = newSchedule.Id,
                        AwardId = a.AwardId,
                        Quantity = a.AwardCount,
                        Status = AwardScheduleStatus.Rating.ToString(),
                        CreatedBy = schedule.CurrentUserId
                    };
                    listAwardScheduleForExaminer.Add(newAwardSchedule);
                }
            }
            newSchedule.AwardSchedule = listAwardScheduleForExaminer;

            foreach (var p in e.Value) p.ScheduleId = newSchedule.Id;

            await _unitOfWork.ScheduleRepo.AddAsync(newSchedule);
            var examiner = await _unitOfWork.AccountRepo.GetByIdAsync(e.Key);
            await _mailService.SendScheduleToExaminer(examiner);
            await _unitOfWork.SaveChangesAsync();
        }


        return true;
    }
    #endregion

    #region CreateScheduleForQualifyingRound3

    public async Task<bool> CreateScheduleForQualifyingRound3(CreateScheduleManualAssignRequest schedule)
    {
        /*var validationResult = await ValidateScheduleForPreliminaryRequest(s);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);*/
        var round = await _unitOfWork.RoundRepo.GetByIdAsync(schedule.RoundId);
        foreach (var s in schedule.listScheduleSingleExaminer)
        {
            
            //Get Painting 
            
                var listPainting =
                    await _unitOfWork.RoundTopicRepo.ListPaintingForQualifyingRound(schedule.RoundId, s.JudgedCount);
                
                var award = round?.Award.ToList();
                if (award == null) throw new Exception("Không có giải nào để lên lịch chấm.");

                var newSchedule = new Schedule();
                newSchedule.Id = Guid.NewGuid();
                newSchedule.ExaminerId = s.ExaminerId;
                newSchedule.EndDate = s.EndDate;
                newSchedule.RoundId = schedule.RoundId;
                newSchedule.Description = s.Description;
                newSchedule.Status = ScheduleStatus.Rating.ToString();
                newSchedule.CreatedBy = schedule.CurrentUserId;

                //Add award schudele
                var listAwardSchedule = new List<AwardSchedule>();
                foreach (var a in s.Awards)
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

                foreach (var p in listPainting) p.ScheduleId = newSchedule.Id;

                await _unitOfWork.ScheduleRepo.AddAsync(newSchedule);
                var examiner = await _unitOfWork.AccountRepo.GetByIdAsync(s.ExaminerId);
                await _mailService.SendScheduleToExaminer(examiner);
                await _unitOfWork.SaveChangesAsync();
            
        }


        return true;
    }
    #endregion

    #region CreateScheduleForFinalRound3

    public async Task<bool> CreateScheduleForFinalRound3(CreateScheduleManualAssignRequest schedule)
    {
        /*var validationResult = await ValidateScheduleForFinalRequest(s);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);*/

        var round = await _unitOfWork.RoundRepo.GetByIdAsync(schedule.RoundId);
        foreach (var s in schedule.listScheduleSingleExaminer)
        {
            
                //Get Painting 
                var listPainting =
                    await _unitOfWork.RoundTopicRepo.ListPaintingForFinalRound(schedule.RoundId, s.JudgedCount);
                var award = round?.Award.ToList();
                if (award == null) throw new Exception("Không có giải nào để lên lịch chấm.");

                //Create new Schedule
                var newSchedule = new Schedule();
                newSchedule.Id = Guid.NewGuid();
                newSchedule.ExaminerId = s.ExaminerId;
                newSchedule.EndDate = s.EndDate;
                newSchedule.RoundId = schedule.RoundId;
                newSchedule.Description = s.Description;
                newSchedule.Status = ScheduleStatus.Rating.ToString();
                newSchedule.CreatedBy = schedule.CurrentUserId;

                var listAwardSchedule = new List<AwardSchedule>();
                foreach (var a in s.Awards)
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
                var examiner = await _unitOfWork.AccountRepo.GetByIdAsync(s.ExaminerId);
                await _mailService.SendScheduleToExaminer(examiner);

                //Create Notification
                var notification = new NotificationRequest("Lịch Chấm Mới",
                    "Bạn có lịch chấm thi mới xin hãy vào Phần Lịch Chấm để xem chi tiết!", s.ExaminerId);

                await _notificationService.CreateNotification(notification);
                await _unitOfWork.SaveChangesAsync();
            
        }

        return true;
    }
    #endregion

    #region Validate

    public async Task<ValidationResult> ValidateScheduleForPreliminaryRequest(ScheduleForPreliminaryRequest schedule)
    {
        return await _validatorFactory.ScheduleRequestValidator.ValidateAsync(schedule);
    }

    public async Task<ValidationResult> ValidateScheduleUpdateRequest(ScheduleUpdateRequest scheduleUpdate)
    {
        return await _validatorFactory.ScheduleUpdateRequestValidator.ValidateAsync(scheduleUpdate);
    }

    public async Task<ValidationResult> ValidateScheduleForFinalRequest(ScheduleForFinalRequest schedule)
    {
        return await _validatorFactory.ScheduleForFinalRequestValidator.ValidateAsync(schedule);
    }

    public async Task<ValidationResult> ValidateRatingRequest(RatingSendModel rating)
    {
        return await _validatorFactory.RatingRequestValidator.ValidateAsync(rating);
    }

    #endregion

    public Dictionary<Guid, List<Painting>> AssignPaintingsToExaminers(List<Painting> paintings, List<Guid> examiners)
    {
        var paintingPerExaminer = paintings.Count / examiners.Count;
        var remainder = paintings.Count % examiners.Count;

        // Dictionary để lưu kết quả với key là examinerId và value là danh sách các tranh được chia cho examiner
        var result = new Dictionary<Guid, List<Painting>>();

        int currentIndex = 0;

        // Vòng lặp qua từng giám khảo
        for (int i = 0; i < examiners.Count; i++)
        {
            // Nếu có dư tranh, phân cho một số giám khảo nhiều hơn một tranh
            int paintingsToAssign = paintingPerExaminer + (i < remainder ? 1 : 0);

            // Lấy ra số tranh cần phân chia
            var assignedPaintings = paintings.Skip(currentIndex).Take(paintingsToAssign).ToList();
            result.Add(examiners[i], assignedPaintings);

            currentIndex += paintingsToAssign;
        }

        return result;
    }

    public Dictionary<Guid, List<PrizeWithCountViewModel>> DistributeAwardsToExaminers(List<Award> awards, List<AwardSchedule> awardSchedules, List<Guid> listExaminer)
    {
        var result = new Dictionary<Guid, List<PrizeWithCountViewModel>>();
        int examinerCount = listExaminer.Count;

        foreach (var award in awards)
        {
            // Tính tổng số lượng đã sử dụng từ AwardSchedule
            var usedQuantity = awardSchedules
                .Where(asch => asch.AwardId == award.Id)
                .Sum(asch => asch.Quantity);

            // Tính toán số lượng còn lại
            var remainingQuantity = award.Quantity - usedQuantity;

            // Nếu không còn giải thưởng để chia, tiếp tục vòng lặp
            if (remainingQuantity <= 0)
                continue;

            // Tính số lượng mỗi giám khảo sẽ chấm
            int quantityPerExaminer = remainingQuantity / examinerCount;
            int remainder = remainingQuantity % examinerCount;  // Phần dư sẽ được chia cho một số giám khảo

            // Phân phối giải thưởng cho từng giám khảo
            for (int i = 0; i < examinerCount; i++)
            {
                var examinerId = listExaminer[i];
                int quantityForThisExaminer = quantityPerExaminer + (i < remainder ? 1 : 0);  // Phân bổ dư cho những giám khảo đầu tiên

                // Nếu giám khảo chưa có trong dictionary, tạo mới danh sách giải thưởng
                if (!result.ContainsKey(examinerId))
                {
                    result[examinerId] = new List<PrizeWithCountViewModel>();
                }

                // Tạo một bản sao của Award với số lượng giám khảo sẽ chấm
                result[examinerId].Add(new PrizeWithCountViewModel
                {
                    AwardId = award.Id,
                    AwardCount = quantityForThisExaminer,  // Gán số lượng giải thưởng mà giám khảo này sẽ chấm
                    // Các thuộc tính khác của Award nếu có...
                });
            }
        }

        return result;
    }
}