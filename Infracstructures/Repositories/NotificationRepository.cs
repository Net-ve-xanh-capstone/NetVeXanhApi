using Application.IRepositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Notification?>> Get5NotificationOfUser(Guid id)
    {
        return (await DbSet.Where(n => n.AccountId == id).Take(5).ToListAsync())!;
    }

    public async Task<List<Notification?>> GetAllByAccount(Guid id)
    {
        return (await DbSet.Where(n => n.AccountId == id).ToListAsync())!;
    }
}