using Application.BaseModels;
using Application.IService;
using Application.SendModels.Resources;
using Application.SendModels.Topic;
using Application.ViewModels.ResourcesViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Infracstructures;

namespace Application.Services;

public class ResourcesService : IResourcesService
{
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public ResourcesService(IUnitOfWork unitOfWork, IMapper mapper, IValidatorFactory validatorFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validatorFactory = validatorFactory;
    }

    #region Create

    public async Task<bool> CreateResources(ResourcesRequest resources)
    {
        var validationResult = await ValidateResourceRequest(resources);
        if (!validationResult.IsValid)
        {
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        }
        var newResources = _mapper.Map<Resources>(resources);
        newResources.Status = ResourcesStatus.Active.ToString();
        await _unitOfWork.ResourcesRepo.AddAsync(newResources);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get All

    public async Task<List<ResourcesViewModel>> GetListResources()
    {
        var list = await _unitOfWork.ResourcesRepo.GetAllAsync();
        if (list.Count == 0) throw new Exception("Khong tim thay Resource nao");
        
        return _mapper.Map<List<ResourcesViewModel>>(list);
    }

    #endregion

    #region Get By Id

    public async Task<ResourcesViewModel?> GetResourcesById(Guid id)
    {
        var resources = await _unitOfWork.ResourcesRepo.GetByIdAsync(id);
        if (resources == null) throw new Exception("Khong tim thay Resource");
        return _mapper.Map<ResourcesViewModel>(resources);
    }

    #endregion

    #region Update

    public async Task<bool> UpdateResources(ResourcesUpdateRequest updateResources)
    {
        var validationResult = await ValidateResourceUpdateRequest(updateResources);
        if (!validationResult.IsValid)
        {
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        }
        var Resources = await _unitOfWork.ResourcesRepo.GetByIdAsync(updateResources.Id);
        if (Resources == null) throw new Exception("Khong tim thay Resource");
        _mapper.Map(updateResources, Resources);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Delete

    public async Task<bool> DeleteResources(Guid id)
    {
        var Resources = await _unitOfWork.ResourcesRepo.GetByIdAsync(id);
        if (Resources == null) throw new Exception("Khong tim thay Resource");
        Resources.Status = ResourcesStatus.Inactive.ToString();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion
    
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.ResourcesRepo.IsExistIdAsync(id);
    }

    #region Validate
    public async Task<ValidationResult> ValidateResourceRequest(ResourcesRequest resource)
    {
        return await _validatorFactory.ResourcesRequestValidator.ValidateAsync(resource);
    }

    public async Task<ValidationResult> ValidateResourceUpdateRequest(ResourcesUpdateRequest resourceUpdate)
    {
        return await _validatorFactory.ResourcesUpdateRequestValidator.ValidateAsync(resourceUpdate);
    }
    #endregion
}