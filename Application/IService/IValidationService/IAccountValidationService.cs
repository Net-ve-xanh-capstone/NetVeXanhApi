using Application.ViewModels.AccountViewModels;

namespace Application.IService.IValidationService;

public interface IAccountValidationService
{
    Task<bool> IsExistedId(Guid id);
    Task<bool> IsExistedCompetitor(Guid id);

    Task<bool> IsExistStaff(Guid id);
    Task<bool> IsExistPhone(string phone);
    Task<bool> IsExistEmail(string email);
    Task<bool> IsExistUsername(string username);

    Task<AccountValidationInfoViewModel> GetAccountByPaintingId(Guid paintingId);
}