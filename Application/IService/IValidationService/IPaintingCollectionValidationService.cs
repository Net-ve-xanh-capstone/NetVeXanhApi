namespace Application.IService.IValidationService;

public interface IPaintingCollectionValidationService
{
    Task<bool> IsPaintingInCollection(Guid paintingId, Guid collectionId);
    Task<bool> IsExistedId(Guid id);
}