namespace Application.IService.IValidationService;

public interface ISponsorValidationService
{
    Task<bool> IsExistedId(Guid id);
}