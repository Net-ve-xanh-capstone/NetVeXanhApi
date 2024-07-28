using System.Reflection.Emit;
using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.EducationalLevel;
using Application.SendModels.Topic;
using Application.ViewModels.EducationalLevelViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Infracstructures;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class EducationalLevelService : IEducationalLevelService
{
    private readonly IClaimsService _claimsService;
    private readonly IConfiguration _configuration;
    private readonly ICurrentTime _currentTime;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public EducationalLevelService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime,
        IConfiguration configuration, IClaimsService claimsService, IValidatorFactory validatorFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentTime = currentTime;
        _configuration = configuration;
        _claimsService = claimsService;
        _validatorFactory = validatorFactory;
    }

    #region Create

    public async Task<bool> CreateEducationalLevel(EducationalLevelRequest EducationalLevel)
    {
        var a = await _unitOfWork.ContestRepo.GetStartEndTimeByContestId(EducationalLevel.ContestId);

        var newEducationalLevel = _mapper.Map<EducationalLevel>(EducationalLevel);
        newEducationalLevel.Status = EducationalLevelStatus.NotStarted.ToString();
        await _unitOfWork.EducationalLevelRepo.AddAsync(newEducationalLevel);
        var check = await _unitOfWork.SaveChangesAsync() > 0;
        if (check == false) throw new Exception("Tạo EducationalLevl Thất Bại");

        #region Tạo Round

        //List level
        var listRound = new List<Round>();
        // Create Round 1 Level 1
        var round = new Round();
        round.Name = "Vòng Sơ Khảo";
        round.CreatedBy = EducationalLevel.CurrentUserId;
        round.EducationalLevelId = newEducationalLevel.Id;
        round.Status = RoundStatus.NotStarted.ToString();
        round.CreatedTime = _currentTime.GetCurrentTime();
        round.StartTime = a.Value.StartTime;
        round.EndTime = a.Value.EndTime;
        round.Description = "Không có mô tả";
        round.Location = "Chưa có thông tin địa điểm";
        listRound.Add(round);

        // Create Round 2 Level 1
        var round2 = new Round();
        round2.Name = "Vòng Chung Kết";
        round2.CreatedBy = EducationalLevel.CurrentUserId;
        round2.EducationalLevelId = newEducationalLevel.Id;
        round2.Status = RoundStatus.NotStarted.ToString();
        round2.CreatedTime = _currentTime.GetCurrentTime();
        round2.StartTime = a.Value.StartTime;
        round2.EndTime = a.Value.EndTime;
        round2.Description = "Không có mô tả";
        round2.Location = "Chưa có thông tin địa điểm";
        listRound.Add(round2);

        await _unitOfWork.RoundRepo.AddRangeAsync(listRound);
        check = await _unitOfWork.SaveChangesAsync() > 0;

        //check
        if (check == false) throw new Exception("Tạo Round Thất Bại");

        #endregion

        #region Tạo Award

        //List level
        var listAward = new List<Award>();

        //Create 1st prize Level 1
        var award1 = new Award();
        award1.Rank = "FirstPrize";
        award1.CreatedBy = EducationalLevel.CurrentUserId;
        award1.CreatedTime = _currentTime.GetCurrentTime();
        award1.Quantity = 1;
        award1.Status = ContestStatus.NotStarted.ToString();
        award1.EducationalLevelId = newEducationalLevel.Id;
        listAward.Add(award1);

        //Create 2nd prize  Level 1
        var award2 = new Award();
        award2.Rank = "SecondPrize";
        award2.CreatedBy = EducationalLevel.CurrentUserId;
        award2.CreatedTime = _currentTime.GetCurrentTime();
        award2.Quantity = 2;
        award2.Status = ContestStatus.NotStarted.ToString();
        award2.EducationalLevelId = newEducationalLevel.Id;
        listAward.Add(award2);

        //Create 3rd prize Level 1
        var award3 = new Award();
        award3.Rank = "ThirdPrize";
        award3.CreatedBy = EducationalLevel.CurrentUserId;
        award3.CreatedTime = _currentTime.GetCurrentTime();
        award3.Quantity = 3;
        award3.Status = ContestStatus.NotStarted.ToString();
        award3.EducationalLevelId = newEducationalLevel.Id;
        listAward.Add(award3);

        //Create 4th prize Level 1
        var award4 = new Award();
        award4.Rank = "ConsolationPrize";
        award4.CreatedBy = EducationalLevel.CurrentUserId;
        award4.CreatedTime = _currentTime.GetCurrentTime();
        award4.Quantity = 4;
        award4.Status = ContestStatus.NotStarted.ToString();
        award4.EducationalLevelId = newEducationalLevel.Id;
        listAward.Add(award4);

        //Create Passed Level 1
        var award9 = new Award();
        award9.Rank = "Preliminary";
        award9.CreatedBy = EducationalLevel.CurrentUserId;
        award9.CreatedTime = _currentTime.GetCurrentTime();
        award9.Quantity = 5;
        award9.Status = ContestStatus.NotStarted.ToString();
        award9.EducationalLevelId = newEducationalLevel.Id;
        listAward.Add(award9);

        //Create 1st prize Level 2
        var award5 = new Award();
        award5.Rank = "FirstPrize";
        award5.CreatedBy = EducationalLevel.CurrentUserId;
        award5.CreatedTime = _currentTime.GetCurrentTime();
        award5.Quantity = 1;
        award5.Status = ContestStatus.NotStarted.ToString();
        award5.EducationalLevelId = newEducationalLevel.Id;
        listAward.Add(award5);

        //Create 2nd prize  Level 2
        var award6 = new Award();
        award6.Rank = "SecondPrize";
        award6.CreatedBy = EducationalLevel.CurrentUserId;
        award6.CreatedTime = _currentTime.GetCurrentTime();
        award6.Quantity = 2;
        award6.Status = ContestStatus.NotStarted.ToString();
        award6.EducationalLevelId = newEducationalLevel.Id;
        listAward.Add(award6);

        //Create 3rd prize Level 2
        var award7 = new Award();
        award7.Rank = "ThirdPrize";
        award7.CreatedBy = EducationalLevel.CurrentUserId;
        award7.CreatedTime = _currentTime.GetCurrentTime();
        award7.Quantity = 3;
        award7.Status = ContestStatus.NotStarted.ToString();
        award7.EducationalLevelId = newEducationalLevel.Id;
        listAward.Add(award7);

        //Create 4th prize Level 2
        var award8 = new Award();
        award8.Rank = "ConsolationPrize";
        award8.CreatedBy = EducationalLevel.CurrentUserId;
        award8.CreatedTime = _currentTime.GetCurrentTime();
        award8.Quantity = 4;
        award8.Status = ContestStatus.NotStarted.ToString();
        award8.EducationalLevelId = newEducationalLevel.Id;
        listAward.Add(award8);

        //Create Passed Level 2
        var award10 = new Award();
        award10.Rank = "Preliminary";
        award10.CreatedBy = EducationalLevel.CurrentUserId;
        award10.CreatedTime = _currentTime.GetCurrentTime();
        award10.Quantity = 5;
        award10.Status = ContestStatus.NotStarted.ToString();
        award10.EducationalLevelId = newEducationalLevel.Id;
        listAward.Add(award10);

        await _unitOfWork.AwardRepo.AddRangeAsync(listAward);
        check = await _unitOfWork.SaveChangesAsync() > 0;

        //check
        if (check == false) throw new Exception("Tạo Award Thất Bại");

        #endregion

        return check;
    }

    #endregion

    #region Get All Pagination

    public async Task<(List<EducationalLevelViewModel>, int)> GetListEducationalLevel(ListModels listModels)
    {
        var list = await _unitOfWork.EducationalLevelRepo.GetAllAsync();
        if (list.Count == 0) throw new Exception("Khong tim thay EducationalLevel");
        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        var result = list.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (_mapper.Map<List<EducationalLevelViewModel>>(result), totalPages);
    }

    #endregion

    #region Get All

    public async Task<List<EducationalLevelViewModel>> GetAllEducationalLevel()
    {
        var list = await _unitOfWork.EducationalLevelRepo.GetAllAsync();
        if (list.Count == 0) throw new Exception("Khong tim thay EducationalLevel");

        return _mapper.Map<List<EducationalLevelViewModel>>(list);
    }

    #endregion

    #region Get By Id

    public async Task<EducationalLevelViewModel?> GetEducationalLevelById(Guid levelId)
    {
        var educationalLevel = await _unitOfWork.EducationalLevelRepo.GetByIdAsync(levelId);
        if (educationalLevel == null) throw new Exception("Khong tim thay EducationalLevel");

        return _mapper.Map<EducationalLevelViewModel>(educationalLevel);
    }

    #endregion

    #region Get Level By ContestId

    public async Task<(List<EducationalLevelViewModel>, int)> GetEducationalLevelByContestId(ListModels listLevelModel,
        Guid contestId)
    {
        var list = await _unitOfWork.EducationalLevelRepo.GetEducationalLevelByContestId(contestId);
        if (list.Count == 0) throw new Exception("Khong tim thay EducationalLevel nao");
        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listLevelModel.PageSize);
        int? itemsToSkip = (listLevelModel.PageNumber - 1) * listLevelModel.PageSize;
        var result = list.Skip((int)itemsToSkip)
            .Take(listLevelModel.PageSize)
            .ToList();
        return (_mapper.Map<List<EducationalLevelViewModel>>(result), totalPages);
    }

    #endregion

    #region Update

    public async Task<bool> UpdateEducationalLevel(EducationalLevelUpdateRequest updateEducationalLevel)
    {
        var EducationalLevel = await _unitOfWork.EducationalLevelRepo.GetByIdAsync(updateEducationalLevel.Id);
        if (EducationalLevel == null) throw new Exception("Khong tim thay EducationalLevel");
        _mapper.Map(updateEducationalLevel, EducationalLevel);
        EducationalLevel.UpdatedTime = _currentTime.GetCurrentTime();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Delete

    public async Task<bool> DeleteEducationalLevel(Guid id)
    {
        var level = await _unitOfWork.EducationalLevelRepo.GetByIdAsync(id);
        if (level == null) throw new Exception("Khong tim thay EducationalLevel");
        foreach (var round in level.Round)
        {
            round.Status = RoundStatus.Delete.ToString();
            foreach (var schedule in round.Schedule) schedule.Status = ScheduleStatus.Delete.ToString();
        }

        //award
        foreach (var award in level.Award) award.Status = AwardStatus.Inactive.ToString();

        level.Status = EducationalLevelStatus.Delete.ToString();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.EducationalLevelRepo.IsExistIdAsync(id);
    }

    #region Validate
    public async Task<ValidationResult> ValidateLevelRequest(EducationalLevelRequest level)
    {
        return await _validatorFactory.EducationalLevelRequestValidator.ValidateAsync(level);
    }

    public async Task<ValidationResult> ValidateLevelUpdateRequest(EducationalLevelUpdateRequest levelUpdate)
    {
        return await _validatorFactory.EducationalLevelUpdateRequestValidator.ValidateAsync(levelUpdate);
    }
    #endregion
}