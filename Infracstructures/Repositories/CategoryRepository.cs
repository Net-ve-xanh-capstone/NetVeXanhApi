using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<Category?> GetByIdAsync(Guid? id)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id && x.Status != CategoryStatus.Deleted.ToString());
    }

    public override async Task<List<Category>> GetAllAsync()
    {
        return await DbSet.Where(x => x.Status != CategoryStatus.Deleted.ToString()).ToListAsync();
    }

    public async Task<List<Category>> GetCategoryUnused()
    {
        return await DbSet.Where(x => x.Status == CategoryStatus.Unused.ToString()).ToListAsync();
    }

    public async Task<List<Category>> GetCategoryUsed()
    {
        return await DbSet.Where(x => x.Status == CategoryStatus.Used.ToString()).ToListAsync();
    }
}