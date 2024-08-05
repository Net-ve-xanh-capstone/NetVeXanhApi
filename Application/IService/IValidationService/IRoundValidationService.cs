namespace Application.IService.IValidationService;

public interface IRoundValidationService
{
    Task<bool> IsExistedId(Guid id);
}