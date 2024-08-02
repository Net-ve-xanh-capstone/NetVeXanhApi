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
    public string ContestId { get; set; }
    public string ContestName { get; set; }
    public Guid? ScheduleId { get; set; }
    public string Status { get; set; }
    public string CompetitorCode { get; set; }
    public string Code { get; set; }
    public string OwnerName { get; set; }
    public string OwnerRole { get; set; }
    public string Birthday { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public DateTime CreatedTime { get; set; }
    public Guid RoundId { get; set; }
    public Guid RoundTopicId { get; set; }

}