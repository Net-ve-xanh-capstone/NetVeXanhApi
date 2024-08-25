using Application.IValidators;
using Application.SendModels.RoundTopic;
using FluentValidation;

namespace Infracstructures.Validators;

public class RoundTopicValidator : IRoundTopicValidator
{
    public RoundTopicValidator(IValidator<RoundTopicRequest> roundtopicvalidator,
        IValidator<RoundTopicDeleteRequest> roundtopicdeletevalidator,
        IValidator<GetListRoundTopicRequest> getlistroundtopicrequestvalidator)
    {
        RoundTopicRequestValidator = roundtopicvalidator;
        RoundTopicDeleteRequestValidator = roundtopicdeletevalidator;
        GetListRoundTopicRequestValidator = getlistroundtopicrequestvalidator;
    }

    public IValidator<RoundTopicRequest> RoundTopicRequestValidator { get; }

    public IValidator<RoundTopicDeleteRequest> RoundTopicDeleteRequestValidator { get; }

    public IValidator<GetListRoundTopicRequest> GetListRoundTopicRequestValidator { get; }
}