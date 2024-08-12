using Application.IRepositories;
using DocumentFormat.OpenXml.Office2010.Excel;
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

    public async Task<List<EducationalLevel>> GetEducationalLevelByContestId(Guid contestId)
    {
        return await DbSet.Include(src => src.Contest).Include(src => src.Round).Where(src => src.ContestId == contestId).OrderBy(x=>x.Level).ToListAsync();
    }

    public async Task<List<Guid>> GetLevelIdByListContestId(List<Guid> contestIdList)
    {
        return await DbSet
            .Where(x => contestIdList.Contains((Guid)x.ContestId))
            .Select(x => x.Id)
            .ToListAsync();
    }
}