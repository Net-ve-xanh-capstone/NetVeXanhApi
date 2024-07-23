using Application.SendModels.RoundTopic;
using FluentValidation;

namespace WebAPI.Validation.RoundTopicValidation;

public class RoundTopicRequestValidator : AbstractValidator<RoundTopicRequest>
{

    public RoundTopicRequestValidator()
    {
        // Validate RoundId
        RuleFor(x => x.RoundId)
            .NotEmpty().WithMessage("RoundId không được trống.")
            .NotEqual(Guid.Empty).WithMessage("RoundId phải là kiểu GUID.");

        // Validate ListTopicId
        RuleFor(x => x.ListTopicId)
            .NotNull().WithMessage("ListTopicId không được trống.")
            .Must(list => list.All(id => id != Guid.Empty)).WithMessage("Mọi topicID trong ListTopicId phải là kiểu GUID.")
            .Must(list => list.Distinct().Count() == list.Count).WithMessage("ListTopicId không được trùng.")
            .WithMessage("ListTopicId phải có ít nhất 1 topic ID.");
    }
}