namespace Application.IService.IValidationService;

public interface IRoundTopicValidationService
{
    Task<bool> IsExistedId(Guid id);
}