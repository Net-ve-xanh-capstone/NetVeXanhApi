using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
{
    public ScheduleRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<List<Schedule>> GetAllAsync()
    {
        return await DbSet.Where(x => x.Status != ScheduleStatus.Delete.ToString()).ToListAsync();
    }

    public override async Task<Schedule?> GetByIdAsync(Guid id)
    {
        return await DbSet.Where(x => x.Status != ScheduleStatus.Delete.ToString())
            .Include(s => s.Painting.Where(x => x.Status != PaintingStatus.Delete.ToString()))
            .Include(s => s.AwardSchedule.Where(x => x.Status != AwardScheduleStatus.Delete.ToString()))
            .ThenInclude(a => a.Award)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Schedule>> GetByExaminerId(Guid id)
    {
        return await DbSet.Include(s => s.Round).Include(s => s.Round).ThenInclude(r => r.EducationalLevel)
            .Where(s => s.ExaminerId == id).OrderByDescending(s => s.CreatedTime).ToListAsync();
    }

    public async Task<List<Schedule>> SchedulerTrigger()
    {
        return await DbSet.Include(src => src.Account).Include(src => src.AwardSchedule).Include(src => src.Painting)
            .Where(src => src.EndDate <= DateTime.Now && src.Status == ScheduleStatus.Rating.ToString()).ToListAsync();
    }

    public async Task<List<Painting>> GetListByRoundId(Guid roundId)
    {
        return await DbSet.Include(src => src.Painting).ThenInclude(src => src.Award).Include(src => src.Painting)
            .ThenInclude(src => src.Account)
            .Where(src => src.RoundId == roundId && src.Status == ScheduleStatus.Done.ToString())
            .SelectMany(src => src.Painting).Where(src => src.Status == PaintingStatus.Pass.ToString()).ToListAsync();
    }
}