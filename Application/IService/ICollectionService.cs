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
    Task<CollectionResponse> GetCollectionById(Guid collectionId);
    Task<GetPaintingInCollectionResponse> GetPaintingByCollection(ListModels listPaintingModel, Guid collectionId);
    Task<(List<CollectionResponse>, int)> GetAllCollection(ListModels listCollectionModel);
    Task<(List<CollectionResponse>, int)> GetCollectionByAccountId(ListModels listCollectionModel, Guid accountId);
    Task<List<CollectionPaintingResponse>> Get6StaffCollection();
    Task<bool> IsExistedId(Guid id);
    Task<ValidationResult> ValidateCollectionRequest(CollectionRequest collection);

    Task<ValidationResult> ValidateCollectionUpdateRequest(UpdateCollectionRequest collectionUpdate);
}