using Application.Common;
using Microsoft.EntityFrameworkCore.Query;

namespace Application.IRepositories;

public interface IGenericRepository<TModel> where TModel : class
{
    Task<TModel> CloneAsync(TModel model);
    Task<List<TModel>> GetAllAsync();
    Task<List<TModel>> GetAllAsync(Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>>? include = null);

    Task<TModel?> GetByIdAsync(Guid? id);
    Task AddAsync(TModel model);

    void AddAttach(TModel model);
    void AddEntry(TModel model);
    void Update(TModel model);

    void UpdateRange(List<TModel> models);

    Task AddRangeAsync(List<TModel> models);

    // Add paging method to generic interface 
    Task<Pagination<TModel>> ToPaginationAsync(int pageIndex = 0, int pageSize = 10);

    Task DeleteAsync(TModel model);
    Task DeleteRangeAsync(List<TModel> models);
    Task<bool> IsExistIdAsync(Guid id);

    Task<List<TModel>> GetByIdsAsync(List<Guid> ids);
}