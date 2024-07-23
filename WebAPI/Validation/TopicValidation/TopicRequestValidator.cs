using Application.IService;
using Application.SendModels.Topic;
using FluentValidation;

namespace WebAPI.Validation.TopicValidation;

public class TopicRequestValidator : AbstractValidator<TopicRequest>
{
    private readonly IAccountService _accountService;

    public TopicRequestValidator(IAccountService accountService)
    {
        RuleFor(x => x.Name)
            .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");

        RuleFor(x => x.CurrentUserId)
            .NotEmpty()
            .WithMessage("CurrentUserId không được trống.")
            .MustAsync(async (userId, cancellation) => await _accountService.IsExistedId(userId))
            .WithMessage("CurrentUserId không tồn tại.");
    }
}