using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Notification;
using Application.SendModels.Painting;
using Application.ViewModels.PaintingViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Infracstructures.SendModels.Painting;

namespace Application.Services;

public class PaintingService : IPaintingService
{
    private readonly IMailService _mailService;
    private readonly IAuthentication _authentication;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;


    public PaintingService(IUnitOfWork unitOfWork, IAuthentication authentication, IMapper mapper, INotificationService notificationService,
        IMailService mailService, IValidatorFactory validatorFactory)
    {
        _authentication = authentication;
        _mailService = mailService;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validatorFactory = validatorFactory;
    }

    #region Draft Painting Preliminary Round

    public async Task<bool> DraftPaintingForPreliminaryRound(CompetitorCreatePaintingRequest request)
    {
        var validationResult = await ValidateCompetitorCreateRequest(request);
        if (!validationResult.IsValid)
        {
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        }
        var painting = _mapper.Map<Painting>(request);
        var rt = await _unitOfWork.RoundTopicRepo.GetByIdAsync(request.RoundTopicId);
        var check = await _unitOfWork.RoundRepo.CheckSubmitValidDate(rt!.RoundId);
        if (check)
        {
            painting.Status = PaintingStatus.Draft.ToString();
            painting.Code = "";
            painting.CreatedBy = request.AccountId;
            painting.RoundTopicId = request.RoundTopicId;
            await _unitOfWork.PaintingRepo.AddAsync(painting);
            await _unitOfWork.SaveChangesAsync();
            var roundTopic = await _unitOfWork.RoundTopicRepo.GetByIdAsync(request.RoundTopicId);
            painting.Code = await GeneratePaintingCode(painting.Id, roundTopic!.RoundId);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        throw new Exception("Khong trong thoi gian nop bai");
    }

    #endregion

    #region Submit Painting Preliminary Round

    public async Task<bool> SubmitPaintingForPreliminaryRound(CompetitorCreatePaintingRequest request)
    {
        // Validate the request
        var validationResult = await ValidateCompetitorCreateRequest(request);
        if (!validationResult.IsValid)
        {
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        }

        var roundTopic = await _unitOfWork.RoundTopicRepo.GetByIdAsync(request.RoundTopicId);
        var check = await _unitOfWork.RoundRepo.CheckSubmitValidDate(roundTopic!.RoundId);
        if (check)
        {
            var painting = _mapper.Map<Painting>(request);
            painting.Status = PaintingStatus.Submitted.ToString();
            painting.Code = ""; // Sửa Db thì xóa
            painting.CreatedBy = request.AccountId;
            painting.SubmittedTimestamp = DateTime.Now;
            painting.RoundTopicId = request.RoundTopicId;
            await _unitOfWork.PaintingRepo.AddAsync(painting);
            await _unitOfWork.SaveChangesAsync();

            painting.Code = await GeneratePaintingCode(painting.Id, roundTopic.RoundId);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                var notification = new NotificationRequest();
                notification.Message = "Bạn đã nột bài thông công";
                notification.Title = "Nét Vẽ Xanh 2024";
                notification.AccountId = request.AccountId;
                await _notificationService.CreateNotification(notification);
            }

            return true;
        }

        throw new Exception("Khong trong thoi gian nop bai");
    }

    #endregion

    #region Staff Submit Painting Preliminary

    public async Task<bool> StaffSubmitPaintingForPreliminaryRound(StaffCreatePaintingRequest request)
    {
        var validationResult = await ValidateStaffCreateRequest(request);
        if (!validationResult.IsValid)
        {
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        }
        var roundTopic = await _unitOfWork.RoundTopicRepo.GetByIdAsync(request.RoundTopicId);
        var check = await _unitOfWork.RoundRepo.CheckSubmitValidDate(roundTopic!.RoundId);
        if (check)
        {
            //Check Age
            var yearOld = DateTime.Today.Year - request.Birthday.Year;
            var level = roundTopic.Round.EducationalLevel.Level;
            if (2 <= yearOld && yearOld <= 5)
            {
                if (level != "Bảng A") throw new Exception("Độ tuổi của bạn không hợp lệ cho vòng thi này !");
            }
            else if (6 <= yearOld && yearOld <= 10)
            {
                if (level != "Bảng B") throw new Exception("Độ tuổi của bạn không hợp lệ cho vòng thi này !");
            }
            else
            {
                new Exception("Độ tuổi của bạn không hợp lệ cho vòng thi này !");
            }

            //Add DB
            if (request.Status == PaintingStatus.Submitted.ToString() ||
                request.Status == PaintingStatus.Rejected.ToString() ||
                request.Status == PaintingStatus.Accepted.ToString())
            {
                // map account
                var competitor = _mapper.Map<Account>(request);
                Random rnd = new Random();
                var password = rnd.Next(1000000, 9999999).ToString();
                competitor.Password = _authentication.Hash(password);
                //map painting
                var painting = _mapper.Map<Painting>(request);
                painting.CreatedBy = request.CurrentUserId;
                painting.AccountId = competitor.Id;
                painting.Code = ""; // Sửa Db thì xóa
                painting.SubmittedTimestamp = DateTime.Now;
                painting.Status = request.Status;
                painting.RoundTopicId = roundTopic.Id;
                competitor.Painting = new List<Painting>();
                competitor.Painting.Add(painting);
                await _unitOfWork.AccountRepo.AddAsync(competitor);
                await _unitOfWork.SaveChangesAsync();
                painting.Code = await GeneratePaintingCode(painting.Id, roundTopic.RoundId);
                competitor.Code = await GenerateAccountCode(Role.Competitor);
                competitor.Username = competitor.Code;

                _unitOfWork.AccountRepo.Update(competitor);
                var result = await _unitOfWork.SaveChangesAsync() > 0;

                await _mailService.SendAccountInformation(competitor, password);
                return result;
            }

            throw new Exception("Trang Thai Khong Hop Le");
        }

        throw new Exception("Khong trong thoi gian nop bai");
    }

    #endregion

    #region Add Painting Final Round

    public async Task<bool> StaffSubmitPaintingForFinalRound(StaffCreatePaintingFinalRoundRequest request)
    {
        var roundTopic = await _unitOfWork.RoundTopicRepo.GetByIdAsync(request.RoundTopicId);
        var painting = _mapper.Map<Painting>(request);
        painting.Status = PaintingStatus.FinalRound.ToString();
        painting.Code = ""; // Sửa Db thì xóa
        painting.CreatedBy = request.CurrentUserId;
        painting.SubmittedTimestamp = DateTime.Now;
        painting.RoundTopicId = request.RoundTopicId;
        await _unitOfWork.PaintingRepo.AddAsync(painting);

        await _unitOfWork.SaveChangesAsync();

        painting.Code = await GeneratePaintingCode(painting.Id, roundTopic!.RoundId);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get List Painting

    public async Task<(List<PaintingViewModel>, int)> GetListPainting(ListModels listPaintingModel)
    {
        var paintingList = await _unitOfWork.PaintingRepo.GetAllAsync();
        if (paintingList.Count == 0) throw new Exception("Khong tim thay Painting nao");
        var result = _mapper.Map<List<PaintingViewModel>>(paintingList);

        var totalPages = (int)Math.Ceiling((double)result.Count / listPaintingModel.PageSize);
        int? itemsToSkip = (listPaintingModel.PageNumber - 1) * listPaintingModel.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listPaintingModel.PageSize)
            .ToList();
        return (result, totalPages);
    }

    #endregion

    #region Delete Painting

    public async Task<bool> DeletePainting(Guid paintingId)
    {
        var painting = await _unitOfWork.PaintingRepo.GetByIdAsync(paintingId);
        if (painting == null) throw new Exception("Không tìm thấy Painting");

        if (painting.Status != PaintingStatus.Draft.ToString())
        {
            throw new Exception("Bài thi đã nộp không đươc xóa");
        }

        painting.Status = PaintingStatus.Delete.ToString();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Update Painting

    public async Task<bool> UpdatePainting(UpdatePaintingRequest updatePainting)
    {
        var validationResult = await ValidateUpdatePaintingRequest(updatePainting);
        if (!validationResult.IsValid)
        {
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        }
        var painting = await _unitOfWork.PaintingRepo.GetByIdAsync(updatePainting.Id);

        if (painting == null) throw new Exception("Không tìm thấy Painting");
        painting.UpdatedBy = updatePainting.CurrentUserId;
        painting.UpdatedTime = DateTime.Now;
        if (painting.Status != PaintingStatus.Draft.ToString()) throw new Exception("Khong duoc sua");

        _mapper.Map(updatePainting, painting);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Update Painting

    public async Task<bool> UpdatePaintingStaffPermission(StaffUpdatePaintingRequest updatePainting)
    {
        var painting = await _unitOfWork.PaintingRepo.GetByIdAsync(updatePainting.Id);
        if (painting == null) throw new Exception("Không tìm thấy Painting");
        painting.UpdatedBy = updatePainting.CurrentUserId;
        painting.UpdatedTime = DateTime.Now;
        _mapper.Map(updatePainting, painting);


        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Review Decision of Painting

    public async Task<PaintingViewModel?> ReviewDecisionOfPainting(PaintingUpdateStatusRequest request)
    {
        var validationResult = await ValidatePaintingUpdateStatusRequest(request);
        if (!validationResult.IsValid)
        {
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        }
        var painting = await _unitOfWork.PaintingRepo.GetByIdAsync(request.Id);
        if (painting == null) return null;

        if (painting.Status != PaintingStatus.Submitted.ToString()) return null;

        if (request.IsPassed)
            painting.Status = PaintingStatus.Accepted.ToString();
        else
            painting.Status = PaintingStatus.Rejected.ToString();
        painting.ReviewedTimestamp = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PaintingViewModel>(painting);
    }

    #endregion

    #region Final Decision of Painting

    public async Task<PaintingViewModel?> FinalDecisionOfPainting(PaintingUpdateStatusRequest request)
    {
        var validationResult = await ValidatePaintingUpdateStatusRequest(request);
        if (!validationResult.IsValid)
        {
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        }
        var painting = await _unitOfWork.PaintingRepo.GetByIdAsync(request.Id);
        if (painting == null) return null;

        if (painting.Status != PaintingStatus.Accepted.ToString()) return null;

        if (request.IsPassed)
            painting.Status = PaintingStatus.Pass.ToString();
        else
            painting.Status = PaintingStatus.NotPass.ToString();

        painting.FinalDecisionTimestamp = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PaintingViewModel>(painting);
    }

    #endregion

    #region Get Painting By Code

    public async Task<PaintingViewModel> GetPaintingByCode(string code)
    {
        var painting = await _unitOfWork.PaintingRepo.GetByCodeAsync(code);
        if (painting == null) throw new Exception("Khong tim thay Painting");
        return _mapper.Map<PaintingViewModel>(painting);
    }

    #endregion

    #region Get Painting By Id

    public async Task<PaintingViewModel> GetPaintingById(Guid id)
    {
        var painting = await _unitOfWork.PaintingRepo.GetByIdAsync(id);
        if (painting == null) throw new Exception("Khong tim thay Painting");
        return _mapper.Map<PaintingViewModel>(painting);
    }

    #endregion

    #region Painting Tracking

    public async Task<PaintingTrackingViewModel> PaintingTracking(Guid id)
    {
        var painting = await _unitOfWork.PaintingRepo.GetByIdAsync(id);
        if (painting == null) throw new Exception("Khong tim thay Painting");
        return _mapper.Map<PaintingTrackingViewModel>(painting);
    }

    #endregion

    #region List 16 Wining Painting

    public async Task<List<PaintingViewModel>> List16WiningPainting()
    {
        var painting = await _unitOfWork.PaintingRepo.List16WiningPaintingAsync();
        if (painting.Count == 0) throw new Exception("Khong tim thay Painting nao");
        return _mapper.Map<List<PaintingViewModel>>(painting);
    }

    #endregion

    #region List Painting By AccountId

    public async Task<(List<PaintingViewModel>, int)> ListPaintingByAccountId(Guid accountId,
        ListModels listPaintingModel)
    {
        var listPainting = await _unitOfWork.PaintingRepo.ListByAccountIdAsync(accountId);
        if (listPainting.Count == 0) throw new Exception("Khong tim thay Painting");
        var result = _mapper.Map<List<PaintingViewModel>>(listPainting);

        #region pagination

        var totalPages = (int)Math.Ceiling((double)result.Count / listPaintingModel.PageSize);
        int? itemsToSkip = (listPaintingModel.PageNumber - 1) * listPaintingModel.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listPaintingModel.PageSize)
            .ToList();

        #endregion

        return (result, totalPages);
    }

    #endregion

    #region Filter Painting

    public async Task<(List<PaintingViewModel>, int)> FilterPainting(FilterPaintingRequest filterPainting,
        ListModels listPaintingModel)
    {
        var validationResult = await ValidateFilterPaintingRequest(filterPainting);
        if (!validationResult.IsValid)
        {
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        }
        var listPainting = await _unitOfWork.PaintingRepo.FilterPaintingAsync(filterPainting);
        if (listPainting.Count == 0) throw new Exception("Không có Painting nào!");
        var result = _mapper.Map<List<PaintingViewModel>>(listPainting);

        #region pagination

        var totalPages = (int)Math.Ceiling((double)result.Count / listPaintingModel.PageSize);
        int? itemsToSkip = (listPaintingModel.PageNumber - 1) * listPaintingModel.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listPaintingModel.PageSize)
            .ToList();

        #endregion

        return (result, totalPages);
    }

    #endregion

    #region Generate Painting Code Async

    private async Task<string> GeneratePaintingCode(Guid paintingId, Guid? roundId)
    {
        var painting = await _unitOfWork.PaintingRepo.GetByIdAsync(paintingId);

        var year = painting.RoundTopic.Round.EducationalLevel.Contest.StartTime.ToString("yy");
        var levelChar = painting.RoundTopic.Round.EducationalLevel.Level.Last().ToString();
        var roundCode = painting.RoundTopic.Round.Name == "Vòng Chung Kết" ? "CK" : "VL";

        int number;
        number = await _unitOfWork.PaintingRepo.CreateNewNumberOfPaintingCode(roundId);

        var code = $"NVX{year}-{levelChar}-{roundCode}-{number:D5}";

        return code;
    }

    #endregion

    #region Generate Account Code

    private async Task<string> GenerateAccountCode(Role role)
    {
        var prefix = role switch
        {
            Role.Guardian => "GH",
            Role.Competitor => "TS",
            Role.Staff => "NV",
            Role.Admin => "AD",
            Role.Examiner => "GK",
            _ => throw new ArgumentException("Invalid role")
        };

        var number = await _unitOfWork.AccountRepo.CreateNumberOfAccountCode(prefix);
        var code = $"{prefix}-{number:D6}";
        return code;
    }

    #endregion

    #region Get Painting By account contest 

    public async Task<PaintingViewModel> GetPaintingByAccountContest(Guid contestId, Guid accountId)
    {
        var painting = await _unitOfWork.PaintingRepo.GetPaintingsByContestAndAccountAsync(contestId, accountId);
        if (painting == null) throw new Exception("Khong tim thay Painting");
        return _mapper.Map<PaintingViewModel>(painting);
    }
    #endregion

    #region Validate
    public async Task<ValidationResult> ValidateCompetitorCreateRequest(CompetitorCreatePaintingRequest painting)
    {
        return await _validatorFactory.CompetitorCreatePaintingRequestValidator.ValidateAsync(painting);
    }
    public async Task<ValidationResult> ValidateFilterPaintingRequest(FilterPaintingRequest filterPainting)
    {
        return await _validatorFactory.FilterPaintingRequestValidator.ValidateAsync(filterPainting);
    }
    public async Task<ValidationResult> ValidatePaintingUpdateStatusRequest(PaintingUpdateStatusRequest painting)
    {
        return await _validatorFactory.PaintingUpdateStatusRequestValidator.ValidateAsync(painting);
    }

    public async Task<ValidationResult> ValidateStaffCreateRequest(StaffCreatePaintingRequest painting)
    {
        return await _validatorFactory.StaffCreatePaintingRequestValidator.ValidateAsync(painting);
    }
    public async Task<ValidationResult> ValidateUpdatePaintingRequest(UpdatePaintingRequest painting)
    {
        return await _validatorFactory.UpdatePaintingRequestValidator.ValidateAsync(painting);
    }
    #endregion
}