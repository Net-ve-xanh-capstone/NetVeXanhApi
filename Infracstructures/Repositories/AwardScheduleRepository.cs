using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class AwardScheduleRepository : GenericRepository<AwardSchedule>, IAwardScheduleRepository
{
    public AwardScheduleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<AwardSchedule>?> GetListByscheduleId(Guid id)
    {
        return await DbSet.Include(a => a.Award).Where(a => a.ScheduleId == id).ToListAsync();
    }

    public override Task<AwardSchedule?> GetByIdAsync(Guid? id)
    {
        return DbSet.Include(a => a.Award)
            .Include(a => a.Schedule)
            .ThenInclude(s => s.Painting.Where(p =>
                p.Status == PaintingStatus.Accepted.ToString() || p.Status == PaintingStatus.FinalRound.ToString()))
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}