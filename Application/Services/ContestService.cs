using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Contest;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.ContestViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class ContestService : IContestService
{
    private readonly IClaimsService _claimsService;
    private readonly IConfiguration _configuration;
    private readonly ICurrentTime _currentTime;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public ContestService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime,
        IConfiguration configuration, IClaimsService claimsService, IValidatorFactory validatorFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentTime = currentTime;
        _configuration = configuration;
        _claimsService = claimsService;
        _validatorFactory = validatorFactory;
    }

    #region Create Contest

    public async Task<bool> CreateContest(CreateContestRequest model)
    {
        var contest = _mapper.Map<Contest>(model);
        foreach (var educationalLevel in contest.EducationalLevel)
        {
            educationalLevel.CreatedBy = contest.CreatedBy;
            foreach (var round in educationalLevel.Round)
            {
                round.CreatedBy = contest.CreatedBy;
                foreach (var award in round.Award) award.CreatedBy = contest.CreatedBy;
            }
        }

        contest.StaffId = contest.CreatedBy;
        if (await _unitOfWork.ContestRepo.CheckContestDuplicate(contest.StartTime, contest.EndTime))
            throw new Exception("Thời gian bị trùng lặp");

        await _unitOfWork.ContestRepo.AddAsync(contest);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Delete Contest

    public async Task<bool> DeleteContest(Guid contestId)
    {
        var contest = await _unitOfWork.ContestRepo.GetByIdAsync(contestId);
        if (contest == null) throw new Exception("Khong tim thay Contest");

        //Contest
        contest.Status = ContestStatus.Delete.ToString();

        //Resource
        foreach (var resource in contest.Resources) resource.Status = ResourcesStatus.Inactive.ToString();

        //Level 
        foreach (var level in contest.EducationalLevel)
        {
            //round
            foreach (var round in level.Round)
            {
                round.Status = RoundStatus.Delete.ToString();
                foreach (var schedule in round.Schedule) schedule.Status = ScheduleStatus.Delete.ToString();
                foreach (var award in round.Award) award.Status = AwardStatus.Inactive.ToString();
            }

            level.Status = EducationalLevelStatus.Delete.ToString();
        }


        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Update Contest

    public async Task<bool> UpdateContest(UpdateContestRequest updateContestRequest)
    {
        var validationResult = await ValidateContestUpdateRequest(updateContestRequest);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        var contest = await _unitOfWork.ContestRepo.GetByIdAsync(updateContestRequest.Id);
        if (contest == null) throw new Exception("Khong tim thay Contest");

        _mapper.Map(updateContestRequest, contest);
        contest.UpdatedTime = _currentTime.GetCurrentTime();


        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get Contest By Id

    public async Task<ContestDetailResponse?> GetContestById(Guid contestId)
    {
        var contest = await _unitOfWork.ContestRepo.GetAllContestInformationAsync(contestId);
        if (contest == null) throw new Exception("Không tìm thấy cuộc thi nào!");
        var result = _mapper.Map<ContestDetailResponse>(contest);
        result.PaintingCount = await _unitOfWork.PaintingRepo.PaintingCountByContest(contestId);
        result.CompetitorCount = await _unitOfWork.AccountRepo.CompetitorCountByContest(contestId);

        return result;
    }

    #endregion

    #region Get 5 recent contest year

    public async Task<List<ContestNameYearResponse>> Get5RecentYear()
    {
        var result = await _unitOfWork.ContestRepo.Get5RecentYearAsync();
        if (result == null) throw new Exception("Không có Cuộc thi nào!");
        return result;
    }

    #endregion

    #region Get All Contest

    public async Task<List<ContestResponse?>> GetAllContest()
    {
        var contest = await _unitOfWork.ContestRepo.GetAllAsync();
        if (contest.Count == 0) throw new Exception("Khong co Contest nao");
        var result = _mapper.Map<List<ContestResponse>>(contest);
        foreach (var item in result)
        {
            item.PaintingCount = await _unitOfWork.PaintingRepo.PaintingCountByContest(item.Id);
            item.CompetitorCount = await _unitOfWork.AccountRepo.CompetitorCountByContest(item.Id);
        }

        return result;
    }

    #endregion

    #region Get All Contest

    public async Task<(List<ContestResponse?>, int)> GetAllContest_v2(ListModels listModel)
    {
        var contest = await _unitOfWork.ContestRepo.GetAllAsync();
        if (contest.Count == 0) throw new Exception("Khong co Contest nao");
        var result = _mapper.Map<List<ContestResponse>>(contest);
        foreach (var item in result)
        {
            item.PaintingCount = await _unitOfWork.PaintingRepo.PaintingCountByContest(item.Id);
            item.CompetitorCount = await _unitOfWork.AccountRepo.CompetitorCountByContest(item.Id);
        }

        var totalPages = (int)Math.Ceiling((double)result.Count / listModel.PageSize);
        int? itemsToSkip = (listModel.PageNumber - 1) * listModel.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listModel.PageSize)
            .ToList();
        return (result, totalPages);
    }

    #endregion

    #region get contest for filter painting

    public async Task<List<FilterPaintingContestResponse>> GetContestForFilterPainting()
    {
        var contest = await _unitOfWork.ContestRepo.GetAllAsync();
        if (contest.Count == 0) throw new Exception("Khong co Contest nao");
        var result = _mapper.Map<List<FilterPaintingContestResponse>>(contest);

        return result;
    }

    #endregion

    #region Get Account Award Information

    public async Task<List<AccountAwardResponse>> GetAccountWithAwardPainting()
    {
        var contest = await _unitOfWork.ContestRepo.GetAccountsByMostRecentContestAsync();
        if (contest.Count == 0) throw new Exception("Khong co Contest nao");
        return contest;
    }

    #endregion

    #region Get Nearest Contest

    public async Task<ContestDetailResponse> GetNearestContest()
    {
        var contest = await _unitOfWork.ContestRepo.GetNearestContestInformationAsync();
        if (contest == null) throw new Exception("Không có Contest nào");

        return _mapper.Map<ContestDetailResponse>(contest);
    }

    #endregion

    #region is Existed

    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.ContestRepo.IsExistIdAsync(id);
    }

    #endregion

    #region list dropDown Infor

    public async Task<ListDropDownContestResponse> GetListForDorpDown(Guid contestId)
    {
        var listLevel = await _unitOfWork.ContestRepo.GetListEducationalLevelName(contestId);
        var listRound = await _unitOfWork.ContestRepo.GetListRoundName(contestId);
        var result = new ListDropDownContestResponse();
        result.Rounds = listRound;
        result.EducationalLevels = listLevel;
        return result;
    }

    #endregion

    #region Validate

    public async Task<ValidationResult> ValidateContestRequest(ContestRequest contest)
    {
        return await _validatorFactory.ContestRequestValidator.ValidateAsync(contest);
    }

    public async Task<ValidationResult> ValidateContestUpdateRequest(UpdateContestRequest contestRequestUpdate)
    {
        return await _validatorFactory.UpdateContestRequestValidator.ValidateAsync(contestRequestUpdate);
    }

    #endregion


    #region DashBoard

    public async Task<List<NumberPaintingResponse>> QuantiyPaintingForYear()
    {
        return await _unitOfWork.ContestRepo.GetNumberOfPaintingsByContestAsync();
    }

    public async Task<List<ContestAwardQuantityResponse>> AwardQuantiy()
    {
        return await _unitOfWork.ContestRepo.GetAwardQuantity();
    }

    #endregion
}