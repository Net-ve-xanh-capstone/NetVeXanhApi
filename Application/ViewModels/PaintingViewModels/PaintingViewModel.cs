namespace Application.ViewModels.PaintingViewModels;

public class PaintingViewModel
{
    public Guid Id { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public DateTime SubmitTime { get; set; }
    public string TopicId { get; set; }
    public string TopicName { get; set; }
    public string RoundName { get; set; }
    public string Level { get; set; }
    public string ContestName { get; set; }
    public Guid? ScheduleId { get; set; }
    public string Status { get; set; }
    public string Code { get; set; }
    public string OwnerName { get; set; }
}