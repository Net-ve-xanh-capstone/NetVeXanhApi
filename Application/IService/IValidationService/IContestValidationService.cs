namespace Application.IService.IValidationService;

public interface IContestValidationService
{
    Task<bool> IsExistedId(Guid id);
    Task<bool> IsExistedName(string name);
}