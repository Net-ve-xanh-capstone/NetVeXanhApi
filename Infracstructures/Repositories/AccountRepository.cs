using System.Text.RegularExpressions;
using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Account?> GetByIdActiveAsync(Guid id)
    {
        return await DbSet.FirstOrDefaultAsync(a => a.Id == id && a.Status == AccountStatus.Active.ToString());
    }
    
    public async Task<Account?> Login(string username)
    {
        return await DbSet.FirstOrDefaultAsync(a =>
            a.Username == username && a.Status == AccountStatus.Active.ToString());
    }

    public async Task<Account?> GetByRefreshToken(string token)
    {
        return await DbSet.FirstOrDefaultAsync(a => a.RefreshToken == token);
    }

    public async Task<bool> CheckDuplicateEmail(string email)
    {
        return await DbSet.AnyAsync(a => a.Email == email);
    }

    public async Task<bool> CheckDuplicatePhone(string phone)
    {
        return await DbSet.AnyAsync(a => a.Phone == phone);
    }

    public async Task<bool> CheckDuplicateUsername(string username)
    {
        return await DbSet.AnyAsync(a => a.Username == username);
    }

    public async Task<List<Account>> GetAccountByListAccountId(List<Guid> listAccountId)
    {
        return await DbSet
            .Where(x => listAccountId.Contains(x.Id))
            .ToListAsync();
    }

    public async Task<bool> AccountNumberExists(int number)
    {
        // Kiểm tra xem số này đã tồn tại trong cơ sở dữ liệu hay chưa
        var formattedAccountNumber = number.ToString("D6");
        return await DbSet.AnyAsync(a => a.Code.EndsWith(formattedAccountNumber));
    }

    public async Task<Account?> GetAccountByCodeAsync(string code)
    {
        return await DbSet.FirstOrDefaultAsync(a => a.Code == code && a.Status == AccountStatus.Active.ToString());
    }
    
    public async Task<int> CreateNumberOfAccountCode(string roleCode)
    {
        var listAccount = await DbSet.ToListAsync();

        if (listAccount == null || !listAccount.Any())
        {
            return 1;
        }

        // Lọc danh sách các tài khoản có code và bắt đầu với roleCode
        var filteredAccounts = listAccount.Where(a => a.Code != null && a.Code.StartsWith(roleCode)).ToList();

        // Kiểm tra xem danh sách filteredAccounts có phần tử nào không
        if (!filteredAccounts.Any())
        {
            return 1;
        }

        int maxNumber = filteredAccounts
            .Select(a =>
            {
                // Dùng biểu thức chính quy để lấy phần số từ mã
                var match = Regex.Match(a.Code, @"\d+$");
                return match.Success ? int.Parse(match.Value) : 0;
            })
            .DefaultIfEmpty(0)
            .Max();

        return maxNumber + 1;
    }

}