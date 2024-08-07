using Application;
using Application.SendModels.RoundTopic;
using FluentValidation;

namespace WebAPI.Validation.RoundTopicValidation;

public class RoundTopicRequestValidator : AbstractValidator<RoundTopicRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public RoundTopicRequestValidator(IValidationServiceManager validationServiceManager)
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

        // Validate ListTopicId

        RuleFor(x => x.ListTopicId)
            .NotNull().WithMessage("Danh sách tranh không được để trống.")
            .Must(paintings => paintings != null && paintings.Any())
            .WithMessage("Danh sách tranh phải chứa ít nhất một mục.");

        When(x => x.ListTopicId != null && x.ListTopicId.Any(), () =>
        {
            RuleForEach(x => x.ListTopicId).ChildRules(topic =>
            {
                topic.RuleFor(p => p)
                    .NotEmpty().WithMessage("Chủ đề không được trống.")
                    .Must(topicId => Guid.TryParse(topicId.ToString(), out _))
                    .WithMessage("Mỗi GUID của chủ đề phải là một GUID hợp lệ.")
                    .DependentRules(() =>
                    {
                        topic.RuleFor(p => p)
                            .MustAsync(async (topicId, cancellation) =>
                            {
                                return await _validationServiceManager.TopicValidationService.IsExistedId(topicId);
                            })
                            .WithMessage("Có chủ đề không tồn tại.");
                    });
            });
        });
    }
}