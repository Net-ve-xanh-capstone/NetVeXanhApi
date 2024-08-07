namespace Application.IService.IValidationService;

public interface ITopicValidationService
{
    Task<bool> IsExistedId(Guid id);
}