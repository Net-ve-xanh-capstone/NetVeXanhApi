using Application.SendModels.RoundTopic;
using FluentValidation;

namespace Application.IValidators;

public interface IRoundTopicValidator
{
    IValidator<GetListRoundTopicRequest> GetListRoundTopicRequestValidator { get; }
    IValidator<RoundTopicRequest> RoundTopicRequestValidator { get; }
    IValidator<RoundTopicDeleteRequest> RoundTopicDeleteRequestValidator { get; }
}