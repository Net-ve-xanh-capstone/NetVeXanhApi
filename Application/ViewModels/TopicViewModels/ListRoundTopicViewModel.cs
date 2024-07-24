namespace Application.ViewModels.TopicViewModels;

public class ListRoundTopicViewModel
{
    public Guid Id { get; set; }
    public Guid? RoundId { get; set; }
    public Guid? TopicId { get; set; }
    public string Name { get; set; } = null!;
}