using Application.Common;
using Application.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infracstructures.Repositories;

public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
{
    protected readonly DbSet<TModel> DbSet;

    public GenericRepository(AppDbContext context)
    {
        DbSet = context.Set<TModel>();
    }

    public virtual async Task AddAsync(TModel model)
    {
        await DbSet.AddAsync(model);
    }

    public virtual void AddAttach(TModel model)
    {
        DbSet.Attach(model).State = EntityState.Added;
    }

    public virtual void AddEntry(TModel model)
    {
        DbSet.Entry(model).State = EntityState.Added;
    }

    public virtual async Task AddRangeAsync(List<TModel> models)
    {
        await DbSet.AddRangeAsync(models);
    }

    public virtual async Task<List<TModel>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    /// <summary>
    ///     The function return list of Tmodel with an include method.
    ///     Example for user we want to include the relation Role:
    ///     + GetAllAsync(user => user.Include(x => x.Role));
    /// </summary>
    /// <param name="include"> The linq expression for include relations we want. </param>
    /// <returns> Return the list of TModel include relations. </returns>
    public virtual async Task<List<TModel>> GetAllAsync(
        Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>>? include = null)
    {
        IQueryable<TModel> query = DbSet;
        if (include != null) query = include(query);
        return await query.ToListAsync();
    }

    public virtual async Task<TModel?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Update(TModel? model)
    {
        DbSet.Update(model);
    }

    public void UpdateRange(List<TModel> models)
    {
        DbSet.UpdateRange(models);
    }

    // Implement to pagination method
    public async Task<Pagination<TModel>> ToPaginationAsync(int pageIndex = 0, int pageSize = 10)
    {
        // get total count of items in the db set
        var itemCount = await DbSet.CountAsync();

        // Create Pagination instance
        // to set data related to paging
        // Calculate and replace pageIndex and pageSize
        // if they are invalid
        var result = new Pagination<TModel>
        {
            PageSize = pageSize,
            TotalItemCount = itemCount,
            PageIndex = pageIndex
        };

        // Take items according to the page size and page index
        // skip items in the previous pages
        // and take next items equal to page size
        var items = await DbSet.Skip(result.PageIndex * result.PageSize)
            .Take(result.PageSize)
            .AsNoTracking()
            .ToListAsync();

        // Assign items to page
        result.Items = items;

        return result;
    }

    public async Task<TModel> CloneAsync(TModel model)
    {
        DbSet.Entry(model).State = EntityState.Detached;
        var values = DbSet.Entry(model).CurrentValues.Clone().ToObject() as TModel;
        return values;
    }

    public virtual async Task DeleteAsync(TModel model)
    {
        DbSet.Remove(model);
    }

    public virtual async Task DeleteRangeAsync(List<TModel> models)
    {
       DbSet.RemoveRange(models);
    }

    public async Task<bool> IsExistIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id) != null;
    }
}