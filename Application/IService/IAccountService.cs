using Application.BaseModels;
using Application.SendModels.AccountSendModels;
using Application.ViewModels.AccountViewModels;
using FluentValidation;
using FluentValidation.Results;

namespace Application.IService;

public interface IAccountService
{
    Task<bool?> CreateSubAccount(SubAccountRequest request);
    Task<(List<AccountViewModel>, int)> GetListExaminer(ListModels listModels);
    Task<(List<AccountViewModel>, int)> GetListCompetitor(ListModels listModels);
    Task<(List<AccountViewModel>, int)> GetListStaff(ListModels listModels);
    Task<List<AccountViewModel>> GetAllStaff();
    Task<List<AccountViewModel>> GetAllCompetitor();
    Task<List<AccountViewModel>> GetAllExaminer();
    Task<(List<AccountViewModel>, int)> GetListInactiveAccount(ListModels listModels);
    Task<AccountViewModel?> GetAccountById(Guid id);
    Task<AccountViewModel?> GetCompetitorById(Guid id);
    Task<bool?> UpdateAccount(AccountUpdateRequest updateAccount);
    Task<bool?> InactiveAccount(Guid id);
    Task<bool?> ActiveAccount(Guid id);

    Task<List<AccountViewModel>> ListAccountHaveAwardIn3NearestContest();

    Task<AccountViewModel?> GetAccountByCode(string code);
    Task<ValidationResult> ValidateAccountUpdateRequest(AccountUpdateRequest account);
    Task<ValidationResult> ValidateSubAccountRequest(SubAccountRequest accountUpdate);
}