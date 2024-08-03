using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Award;
using Application.SendModels.Topic;
using Application.ViewModels.AwardViewModels;
using AutoMapper;
using DocumentFormat.OpenXml.Office2016.Excel;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Infracstructures;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class AwardService : IAwardService
{
    private readonly IClaimsService _claimsService;
    private readonly IConfiguration _configuration;
    private readonly ICurrentTime _currentTime;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public AwardService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, IConfiguration configuration,
        IClaimsService claimsService, IValidatorFactory validatorFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentTime = currentTime;
        _configuration = configuration;
        _claimsService = claimsService;
        _validatorFactory = validatorFactory;
    }

    #region Add Award

    public async Task<bool> AddAward(AwardRequest addAwardViewModel)
    {
        var validationResult = await ValidateAwardRequest(addAwardViewModel);
        if (!validationResult.IsValid)
        {
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        }
        var award = _mapper.Map<Award>(addAwardViewModel);
        award.Status = AwardStatus.Active.ToString();
        await _unitOfWork.AwardRepo.AddAsync(award);
        award.CreatedTime = _currentTime.GetCurrentTime();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get List Award

    public async Task<(List<AwardViewModel>, int)> GetListAward(ListModels listAwardModel)
    {
        var awardList = await _unitOfWork.AwardRepo.GetAllAsync();
        if (awardList.Count == 0) throw new Exception("Khong co Award");
        var result = _mapper.Map<List<AwardViewModel>>(awardList);

        var totalPages = (int)Math.Ceiling((double)result.Count / listAwardModel.PageSize);
        int? itemsToSkip = (listAwardModel.PageNumber - 1) * listAwardModel.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listAwardModel.PageSize)
            .ToList();
        return (result, totalPages);
    }

    #endregion

    #region Delete Award

    public async Task<bool> DeleteAward(Guid awardId)
    {
        var award = await _unitOfWork.AwardRepo.GetByIdAsync(awardId);
        if (award == null) throw new Exception("Khong tim thay Award");

        award.Status = AwardStatus.Inactive.ToString();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Update Award

    public async Task<bool> UpdateAward(UpdateAwardRequest updateAward)
    {
        var award = await _unitOfWork.AwardRepo.GetByIdAsync(updateAward.Id);
        if (award == null) throw new Exception("Khong tim thay Award");

        _mapper.Map(updateAward, award);

        award.UpdatedTime = _currentTime.GetCurrentTime();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion


    #region Get Award By Id

    public async Task<AwardViewModel> GetAwardById(Guid awardId)
    {
        var award = await _unitOfWork.AwardRepo.GetByIdAsync(awardId);
        if (award == null) throw new Exception("Khong tim thay Award");
        return _mapper.Map<AwardViewModel>(award);
    }

    #endregion

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.AwardRepo.IsExistIdAsync(id);
    }
    #region Validate
    public async Task<ValidationResult> ValidateAwardRequest(AwardRequest award)
    {
        return await _validatorFactory.AwardRequestValidator.ValidateAsync(award);
    }

    public async Task<ValidationResult> ValidateTopicUpdateRequest(UpdateAwardRequest awardUpdate)
    {
        return await _validatorFactory.UpdateAwardRequestValidator.ValidateAsync(awardUpdate);
    }
    #endregion
}