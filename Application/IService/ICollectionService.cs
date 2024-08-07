using Application.BaseModels;
using Application.SendModels.Collection;
using Application.ViewModels.CollectionViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface ICollectionService
{
    Task<bool> AddCollection(CollectionRequest addCollectionViewModel);
    Task<bool> DeleteCollection(Guid collectionId);
    Task<bool> UpdateCollection(UpdateCollectionRequest updateCollection);
    Task<CollectionViewModel> GetCollectionById(Guid collectionId);
    Task<GetPaintingInCollection> GetPaintingByCollection(ListModels listPaintingModel, Guid collectionId);
    Task<(List<CollectionViewModel>, int)> GetAllCollection(ListModels listCollectionModel);
    Task<(List<CollectionViewModel>, int)> GetCollectionByAccountId(ListModels listCollectionModel, Guid accountId);
    Task<List<CollectionPaintingViewModel>> Get6StaffCollection();
    Task<bool> IsExistedId(Guid id);
    Task<ValidationResult> ValidateCollectionRequest(CollectionRequest collection);

    Task<ValidationResult> ValidateCollectionUpdateRequest(UpdateCollectionRequest collectionUpdate);
}