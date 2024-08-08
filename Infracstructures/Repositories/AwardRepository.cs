using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class AwardRepository : GenericRepository<Award>, IAwardRepository
{
    public AwardRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<List<Award>> GetAllAsync()
    {
        return await DbSet.Where(x => x.Status == AwardStatus.Active.ToString()).ToListAsync();
    }

    public override async Task<Award?> GetByIdAsync(Guid id)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id && x.Status == AwardStatus.Active.ToString());
    }

    public async Task<List<Guid>> GetAwardIdByListLevelId(List<Guid> listLevelId)
    {
        return await DbSet
            .Where(x => listLevelId.Contains((Guid)x.EducationalLevelId))
            .Select(x => x.Id)
            .ToListAsync();
    }

    public async Task<List<Award>?> GetAwardsByContestId(Guid contestId)
    {
        return await DbSet.Include(src => src.EducationalLevel)
            .Where(src => src.EducationalLevel.ContestId == contestId).ToListAsync();
    }
}