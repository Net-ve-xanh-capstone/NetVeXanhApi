namespace Application.SendModels.Notification;

public class NotificationRequest
{
    public NotificationRequest(string title, string message, Guid? accountId)
    {
        Title = title;
        Message = message;
        AccountId = accountId;
    }

    public string Title { get; set; }
    public string Message { get; set; }
    public Guid? AccountId { get; set; }
}