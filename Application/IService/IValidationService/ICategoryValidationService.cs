namespace Application.IService.IValidationService;

public interface ICategoryValidationService
{
    Task<bool> IsExistedId(Guid id);
}