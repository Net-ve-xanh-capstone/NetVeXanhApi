namespace Application.IService.IValidationService;

public interface IReportValidationService
{
    Task<bool> IsExistedId(Guid id);
}