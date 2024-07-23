using Application.BaseModels;
using Application.SendModels.Topic;
using Application.ViewModels.TopicViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface ITopicService
{
    public Task<bool> CreateTopic(TopicRequest Topic);
    public Task<(List<TopicViewModel>, int)> GetListTopic(ListModels listModels);
    public Task<TopicViewModel?> GetTopicById(Guid id);
    public Task<bool> UpdateTopic(TopicUpdateRequest updateTopic);
    public Task<bool> DeleteTopic(Guid id);

    Task<ValidationResult> ValidateTopicRequest(TopicRequest topic);
    Task<ValidationResult> ValidateTopicUpdateRequest(TopicUpdateRequest topicUpdate);
    Task<List<TopicViewModel>> GetAllTopic();
}