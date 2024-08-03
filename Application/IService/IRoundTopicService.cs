using Application.SendModels.RoundTopic;
using Application.ViewModels.TopicViewModels;
using FluentValidation;
using FluentValidation.Results;

namespace Application.IService;

public interface IRoundTopicService
{
    Task<List<ListRoundTopicViewModel>> GetAll();
    Task<List<RoundTopicViewModel>> GetListRoundTopicForCompetitor(GetListRoundTopicRequest request);
    Task<bool> AddTopicToRound(RoundTopicRequest roundTopicRequest);
    Task<bool> DeleteTopicInRound(RoundTopicDeleteRequest roundTopicDeleteRequest);
    Task<List<RoundTopicViewModel>> GetListRoundTopicForStaff(Guid id);
    Task<ValidationResult> ValidateRoundTopicRequest(RoundTopicRequest roundtopic);

    Task<ValidationResult> ValidateRoundTopicDeleteRequest(RoundTopicDeleteRequest roundtopicDelete);
}