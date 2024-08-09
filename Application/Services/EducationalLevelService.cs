using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.EducationalLevel;
using Application.ViewModels.EducationalLevelViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
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

    public async Task<bool> CreateEducationalLevel(CreateEducationalLevelSendModel model)
    {
        var educationalLevel = _mapper.Map<EducationalLevel>(model);
            foreach (var round in educationalLevel.Round)
            {
                round.CreatedBy = educationalLevel.CreatedBy;
                foreach (var award in round.Award)
                {
                    award.CreatedBy = educationalLevel.CreatedBy;
                }
            }
        await _unitOfWork.EducationalLevelRepo.AddAsync(educationalLevel);
        return await _unitOfWork.SaveChangesAsync() > 0;
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
        var validationResult = await ValidateLevelUpdateRequest(updateEducationalLevel);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
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
            foreach (var award in round.Award) award.Status = AwardStatus.Inactive.ToString();
        }

        //award

        level.Status = EducationalLevelStatus.Delete.ToString();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Check Id is Exist

    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.EducationalLevelRepo.IsExistIdAsync(id);
    }
#endregion


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