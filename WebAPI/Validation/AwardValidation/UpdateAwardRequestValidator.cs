using Application.IService;
using Application.SendModels.Award;
using FluentValidation;

namespace WebAPI.Validation.AwardValidation;

public class UpdateAwardRequestValidator : AbstractValidator<UpdateAwardRequest>
{
    private readonly IAccountService _accountService;
    public UpdateAwardRequestValidator()
    {
        RuleFor(user => user.Id).NotEmpty().WithMessage("Id không được để trống.");

        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("CurrentUserId không được để trống.")
            .NotEqual(Guid.Empty).WithMessage("CurrentUserId không hợp lệ.")
            .MustAsync(async (userId, cancellation) => await _accountService.IsExistedId(userId))
            .WithMessage("CurrentUserId không tồn tại.");
    }
}