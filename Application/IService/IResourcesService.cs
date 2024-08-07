using Application.SendModels.Resources;
using Application.ViewModels.ResourcesViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IResourcesService
{
    Task<bool> CreateResources(ResourcesRequest resources);
    Task<List<ResourcesViewModel>> GetListResources();
    Task<ResourcesViewModel?> GetResourcesById(Guid id);
    Task<bool> UpdateResources(ResourcesUpdateRequest updateResources);
    Task<bool> DeleteResources(Guid id);
    Task<bool> IsExistedId(Guid id);

    Task<ValidationResult> ValidateResourceRequest(ResourcesRequest resource);
    Task<ValidationResult> ValidateResourceUpdateRequest(ResourcesUpdateRequest resourceUpdate);
}