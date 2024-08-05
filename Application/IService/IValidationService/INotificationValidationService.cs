namespace Application.IService.IValidationService;

public interface INotificationValidationService
{
    Task<bool> IsExistedId(Guid id);
}