using Application.IService;
using Application.SendModels.RoundTopic;
using FluentValidation;

namespace WebAPI.Validation.RoundTopicValidation;

public class RoundTopicDeleteRequestValidator : AbstractValidator<RoundTopicDeleteRequest>
{
    private readonly IAccountService _accountService;
    public RoundTopicDeleteRequestValidator()
    {
        // Validate RoundId
        RuleFor(x => x.RoundId)
            .NotEmpty().WithMessage("RoundId không được trống.")
            .NotEqual(Guid.Empty).WithMessage("RoundId phải là kiểu GUID.");

        // Validate TopicId
        RuleFor(x => x.TopicId)
            .NotEmpty().WithMessage("TopicId không được trống.")
            .NotEqual(Guid.Empty).WithMessage("TopicId phải là kiểu GUID.")
            .MustAsync(async (userId, cancellation) => await _accountService.IsExistedId(userId))
            .WithMessage("CurrentUserId không tồn tại.");
    }
}