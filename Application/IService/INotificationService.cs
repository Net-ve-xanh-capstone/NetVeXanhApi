using Application.BaseModels;
using Application.SendModels.Notification;
using Infracstructures.ViewModels.NotificationViewModels;

namespace Application.IService;

public interface INotificationService
{
    public Task<bool> CreateNotification(NotificationRequest Notification);
    public Task<List<NotificationResponse>> Get5Notification(Guid id);
    public Task<NotificationDetailResponse?> GetNotificationById(Guid id);
    public Task<(List<NotificationResponse>?, int)> GetNotificationByAccountId(ListModels listModels, Guid id);
    public Task<bool> ReadNotification(Guid id);
    Task<bool> IsExistedId(Guid id);
}