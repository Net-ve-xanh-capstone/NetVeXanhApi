namespace Application.IService.IValidationService;

public interface IAwardValidationService
{
    Task<bool> IsExistedId(Guid id);
}