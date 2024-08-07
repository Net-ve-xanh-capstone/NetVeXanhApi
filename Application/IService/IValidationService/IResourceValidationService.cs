namespace Application.IService.IValidationService;

public interface IResourceValidationService
{
    Task<bool> IsExistedId(Guid id);
}