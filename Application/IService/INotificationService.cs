using Application.BaseModels;
using Application.SendModels.Notification;
using Infracstructures.ViewModels.NotificationViewModels;

namespace Application.IService;

public interface INotificationService
{
    public Task<bool> CreateNotification(NotificationRequest Notification);
    public Task<List<NotificationViewModel>> Get5Notification(Guid id);
    public Task<NotificationDetailViewModel?> GetNotificationById(Guid id);
    public Task<(List<NotificationViewModel>?, int)> GetNotificationByAccountId(ListModels listModels, Guid id);
    public Task<bool> ReadNotification(Guid id);
    public Task<bool> SendResultFinalRound(Guid id);
    public Task<bool> SendResultPreliminaryRound(Guid id);
    Task<bool> IsExistedId(Guid id);
}