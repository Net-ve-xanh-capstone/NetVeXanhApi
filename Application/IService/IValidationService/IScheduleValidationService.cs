namespace Application.IService.IValidationService;

public interface IScheduleValidationService
{
    Task<bool> IsExistedId(Guid id);
}