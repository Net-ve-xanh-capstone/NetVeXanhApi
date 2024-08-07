namespace Application.IService.IValidationService;

public interface IEducationalLevelValidationService
{
    Task<bool> IsExistedId(Guid id);
}