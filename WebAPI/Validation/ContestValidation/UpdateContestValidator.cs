using Application.IService;
using Application.SendModels.Contest;
using FluentValidation;

namespace WebAPI.Validation.ContestValidation;

public class UpdateContestValidator : AbstractValidator<UpdateContest>
{
    private readonly IAccountService _accountService;
    public UpdateContestValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id không được để trống.")
            .NotEqual(Guid.Empty).WithMessage("Id không được là Guid.Empty.");

        RuleFor(c => c.CurrentUserId)
            .NotEmpty().WithMessage("CurrentUserId không được để trống.")
            .NotEqual(Guid.Empty).WithMessage("CurrentUserId không được là Guid.Empty.")
            .MustAsync(async (userId, cancellation) => await _accountService.IsExistedId(userId))
            .WithMessage("CurrentUserId không tồn tại.");
    }
}