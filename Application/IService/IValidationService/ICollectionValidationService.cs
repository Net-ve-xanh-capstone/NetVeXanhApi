namespace Application.IService.IValidationService;

public interface ICollectionValidationService
{
    Task<bool> IsExistedId(Guid id);
}