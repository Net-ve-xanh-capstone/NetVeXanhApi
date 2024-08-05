using Domain.Models;

namespace Application.IRepositories;

public interface IAccountRepository : IGenericRepository<Account>
{
    Task<Account?> Login(string email);
    Task<Account?> GetByRefreshToken(string token);
    Task<bool> CheckDuplicateEmail(string email);
    Task<bool> CheckDuplicatePhone(string phone);
    Task<bool> CheckDuplicateUsername(string username);
    Task<bool> AccountNumberExists(int number);
    Task<Account?> GetByIdActiveAsync(Guid id);
    Task<Account?> GetCompetitorByIdAsync(Guid id);

    Task<List<Account>> GetAccountByListAccountId(List<Guid> listAccountId);

    Task<Account?> GetAccountByCodeAsync(string code);

    Task<int> CreateNumberOfAccountCode(string roleCode);

    Task<int> CompetitorCountByContest(Guid contestId);

    #region Validate

    Task<bool> IsExistCompetitor(Guid id);

    Task<bool> IsExistStaff(Guid id);
    Task<bool> IsExistPhone(string phone);
    Task<bool> IsExistEmail(string email);
    Task<bool> IsExistUsername(string username);

    #endregion
}