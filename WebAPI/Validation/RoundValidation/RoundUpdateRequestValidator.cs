using Application.IService;
using Application.SendModels.Round;
using FluentValidation;

namespace WebAPI.Validation.RoundValidation;

public class RoundUpdateRequestValidator : AbstractValidator<RoundUpdateRequest>
{
    private readonly IAccountService _accountService;
    public RoundUpdateRequestValidator()
    {
        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được trống.")
            .NotEqual(Guid.Empty).WithMessage("Id phải là kiểu GUID.");

        // Validate CurrentUserId
        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("CurrentUserId không được trống.")
            .NotEqual(Guid.Empty).WithMessage("CurrentUserId phải là kiểu GUID.")
            .MustAsync(async (userId, cancellation) => await _accountService.IsExistedId(userId))
            .WithMessage("CurrentUserId không tồn tại.");
    }
}