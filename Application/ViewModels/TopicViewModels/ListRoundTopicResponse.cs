namespace Application.ViewModels.TopicViewModels;

public class ListRoundTopicResponse
{
    public Guid Id { get; set; }
    public Guid? RoundId { get; set; }
    public Guid? TopicId { get; set; }
    public string Name { get; set; } = null!;
}