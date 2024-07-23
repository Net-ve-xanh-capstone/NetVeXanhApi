using Application.IService;
using FluentValidation;
using Infracstructures.SendModels.Sponsor;

namespace WebAPI.Validation.SponsorValidation;

public class SponsorUpdateRequestValidator : AbstractValidator<SponsorUpdateRequest>
{
    private readonly IAccountService _accountService;
    public SponsorUpdateRequestValidator()
    {
        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được trống.")
            .NotEqual(Guid.Empty).WithMessage("Id phải là kiểu GUID.");

        // Validate CurrentUserId
        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("CurrentUserId không được trống.")
            .MustAsync(async (userId, cancellation) => await _accountService.IsExistedId(userId))
            .NotEqual(Guid.Empty).WithMessage("CurrentUserId phải là kiểu GUID.");
    }
}