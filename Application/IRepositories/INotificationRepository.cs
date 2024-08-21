using Domain.Models;

namespace Application.IRepositories;

public interface INotificationRepository : IGenericRepository<Notification>
{
    public Task<List<Notification?>> Get5NotificationOfUser(Guid id);
    public Task<List<Notification?>> GetAllByAccount(Guid id);
}