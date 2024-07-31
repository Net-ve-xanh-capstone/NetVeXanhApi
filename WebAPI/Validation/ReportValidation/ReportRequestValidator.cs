using Application;
using Application.SendModels.Report;
using FluentValidation;

namespace WebAPI.Validation.ReportValidation;

public class ReportRequestValidator : AbstractValidator<ReportRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public ReportRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate Title
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title không được trống.");

        // Validate Description
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description không được trống.");

        // Validate CurrentUserId
        RuleFor(x => x.CurrentUserId)
        .NotEmpty().WithMessage("CurrentUserId không được để trống.");

        When(x => !string.IsNullOrEmpty(x.CurrentUserId.ToString()), () =>
        {
            RuleFor(x => x.CurrentUserId)
                .Must(userId => Guid.TryParse(userId.ToString(), out _))
                .WithMessage("CurrentUserId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.CurrentUserId)
                        .MustAsync(async (userId, cancellation) =>
                        {
                            return await _validationServiceManager.AccountValidationService.IsExistedId(userId);
                        })
                        .WithMessage("CurrentUserId không tồn tại.");
                });
        });
    }
}