using Application.IService;
using Application.SendModels.EducationalLevel;
using FluentValidation;

namespace WebAPI.Validation.EducationalLevelValidation;

public class EducationalLevelUpdateRequestValidator : AbstractValidator<EducationalLevelUpdateRequest>
{
    private readonly IAccountService _accountService;
    public EducationalLevelUpdateRequestValidator()
    {
        // Validate ContestId
        RuleFor(x => x.ContestId)
            .NotEmpty().WithMessage("ContestId không được trống.")
            .NotEqual(Guid.Empty).WithMessage("ContestId phải là kiểu GUID.");

        // Validate CurrentUserId
        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("CurrentUserId không được trống.")
            .NotEqual(Guid.Empty).WithMessage("CurrentUserId phải là kiểu GUID.")
            .MustAsync(async (userId, cancellation) => await _accountService.IsExistedId(userId))
            .WithMessage("CurrentUserId không tồn tại.");
    }
}