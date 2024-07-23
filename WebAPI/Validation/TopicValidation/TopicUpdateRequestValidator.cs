using Application.IService;
using Application.SendModels.Topic;
using FluentValidation;

namespace WebAPI.Validation.TopicValidation;

public class TopicUpdateRequestValidator : AbstractValidator<TopicUpdateRequest>
{
    private readonly IAccountService _accountService;
    public TopicUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên không được trống.")
            .Length(2, 50).WithMessage("Tên phải trong khoảng 2 tới 50 kí tự.");

        RuleFor(x => x.CurrentUserId)
            .NotEmpty()
            .WithMessage("CurrentUserId không được trống.")
            .MustAsync(async (userId, cancellation) => await _accountService.IsExistedId(userId))
            .WithMessage("CurrentUserId không tồn tại.");
    }
}