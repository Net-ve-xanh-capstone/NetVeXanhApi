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

    public override async Task<EducationalLevel?> GetByIdAsync(Guid id)
    {
        return await DbSet.Include(x => x.Round)
            .ThenInclude(x => x.Schedule)
            .Include(x => x.Award)
            .FirstOrDefaultAsync(x => x.Id == id && x.Status != EducationalLevelStatus.Delete.ToString());
    }

    public async Task<List<EducationalLevel>> GetEducationalLevelByContestId(Guid contestId)
    {
        return await DbSet.Include(x => x.Round).ThenInclude(x => x.Schedule).ThenInclude(x => x.Account).Where(x =>
                x.ContestId == contestId && x.Status != EducationalLevelStatus.Delete.ToString())
            .ToListAsync();
    }

    /*public async Task<List<EducationalLevel>> GetListEducationalLevel(Guid contestId)
    {
        return await DbSet.Where(x => x.ContestId == contestId).ToListAsync();
    }*/
    public async Task<List<Guid>> GetLevelIdByListContestId(List<Guid> contestIdList)
    {
        return await DbSet
            .Where(x => contestIdList.Contains((Guid)x.ContestId))
            .Select(x => x.Id)
            .ToListAsync();
    }
}