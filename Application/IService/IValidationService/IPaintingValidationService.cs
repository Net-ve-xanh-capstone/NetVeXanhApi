namespace Application.IService.IValidationService;

public interface IPaintingValidationService
{
    Task<bool> IsExistedId(Guid id);
    Task<bool> IsExistedPaintingInContest(Guid accountId, Guid roundtopicId);
}