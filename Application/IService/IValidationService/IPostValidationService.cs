namespace Application.IService.IValidationService;

public interface IPostValidationService
{
    Task<bool> IsExistedId(Guid id);
}