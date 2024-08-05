namespace Application.IService.IValidationService;

public interface IAwardScheduleValidationService
{
    Task<bool> IsExistedId(Guid id);
}