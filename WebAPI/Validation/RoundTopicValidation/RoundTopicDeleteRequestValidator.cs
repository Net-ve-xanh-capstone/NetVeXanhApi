using Application;
using Application.IService;
using Application.SendModels.RoundTopic;
using FluentValidation;

namespace WebAPI.Validation.RoundTopicValidation;

public class RoundTopicDeleteRequestValidator : AbstractValidator<RoundTopicDeleteRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public RoundTopicDeleteRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate RoundId
        RuleFor(x => x.RoundId)
            .NotEmpty().WithMessage("RoundId không được trống.");
        When(x => !string.IsNullOrEmpty(x.RoundId.ToString()), () =>
        {
            RuleFor(x => x.RoundId)
                .Must(roundId => Guid.TryParse(roundId.ToString(), out _))
                .WithMessage("RoundId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.RoundId)
                        .MustAsync(async (roundId, cancellation) =>
                        {
                            return await _validationServiceManager.RoundValidationService.IsExistedId(roundId);
                        })
                        .WithMessage("RoundId không tồn tại.");
                });
        });

        // Validate TopicId
        RuleFor(x => x.TopicId)
            .NotEmpty().WithMessage("TopicId không được trống.");
        When(x => !string.IsNullOrEmpty(x.TopicId.ToString()), () =>
        {
            RuleFor(x => x.TopicId)
                .Must(topicId => Guid.TryParse(topicId.ToString(), out _))
                .WithMessage("TopicId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.TopicId)
                        .MustAsync(async (topicId, cancellation) =>
                        {
                            return await _validationServiceManager.TopicValidationService.IsExistedId(topicId);
                        })
                        .WithMessage("TopicId không tồn tại.");
                });
        });
    }
}