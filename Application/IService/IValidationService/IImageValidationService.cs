namespace Application.IService.IValidationService;

public interface IImageValidationService
{
    Task<bool> IsExistedId(Guid id);
}