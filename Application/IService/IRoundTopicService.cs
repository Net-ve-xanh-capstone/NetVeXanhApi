using Application.SendModels.RoundTopic;
using Application.ViewModels.TopicViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IRoundTopicService
{
    Task<List<ListRoundTopicResponse>> GetAll();
    Task<List<RoundTopicResponse>> GetListRoundTopicForCompetitor(GetListRoundTopicRequest request);
    Task<bool> AddTopicToRound(RoundTopicRequest roundTopicRequest);
    Task<bool> DeleteTopicInRound(RoundTopicDeleteRequest roundTopicDeleteRequest);
    Task<List<RoundTopicResponse>> GetListRoundTopicForStaff(Guid id);
    Task<ValidationResult> ValidateRoundTopicRequest(RoundTopicRequest roundtopic);

    Task<ValidationResult> ValidateRoundTopicDeleteRequest(RoundTopicDeleteRequest roundtopicDelete);
}