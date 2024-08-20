using Application.ViewModels.TopicViewModels;

namespace Application.ViewModels.RoundViewModels;

public class ListTopicResponse
{
    private ICollection<TopicResponse> Topic { get; set; }
}