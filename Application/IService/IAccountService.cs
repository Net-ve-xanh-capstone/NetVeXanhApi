using Application.BaseModels;
using Application.SendModels.AccountSendModels;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.ContestViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IAccountService
{
    Task<List<AccountResponse>> GetAllAccount();
    Task<(List<AccountResponse>, int)> GetListExaminer(ListModels listModels);
    Task<(List<AccountResponse>, int)> GetListCompetitor(ListModels listModels);
    Task<(List<AccountResponse>, int)> GetListStaff(ListModels listModels);
    Task<List<AccountResponse>> GetAllStaff();
    Task<List<AccountResponse>> GetAllCompetitor();
    Task<List<AccountResponse>> GetAllExaminer();
    Task<(List<AccountResponse>, int)> GetListInactiveAccount(ListModels listModels);
    Task<AccountResponse?> GetAccountById(Guid id);
    Task<AccountResponse?> GetCompetitorById(Guid id);
    Task<bool?> UpdateAccount(AccountUpdateRequest updateAccount);
    Task<bool?> InactiveAccount(Guid id);
    Task<bool?> ActiveAccount(Guid id);

    Task<List<ContestRewardResponse>> ListAccountHaveAwardIn3NearestContest();

    Task<AccountResponse?> GetAccountByCode(string code);
    Task<ValidationResult> ValidateAccountUpdateRequest(AccountUpdateRequest account);
    Task<ValidationResult> ValidateSubAccountRequest(SubAccountRequest accountUpdate);
}