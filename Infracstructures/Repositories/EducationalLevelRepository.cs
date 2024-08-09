using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class EducationalLevelRepository : GenericRepository<EducationalLevel>, IEducationalLevelRepository
{
    public EducationalLevelRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<List<EducationalLevel>> GetAllAsync()
    {
        return await DbSet.Where(x => x.Status != EducationalLevelStatus.Delete.ToString()).ToListAsync();
    }

    public override async Task<EducationalLevel?> GetByIdAsync(Guid? id)
    {
        return await DbSet.Include(src => src.Contest).Include(src => src.Round).FirstOrDefaultAsync(src => src.Id == id);
    }

    public Task<List<EducationalLevel>> GetEducationalLevelByContestId(Guid contestId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Guid>> GetLevelIdByListContestId(List<Guid> contestIdList)
    {
        return await DbSet
            .Where(x => contestIdList.Contains((Guid)x.ContestId))
            .Select(x => x.Id)
            .ToListAsync();
    }
}