using Application;
using Application.SendModels.Resources;
using FluentValidation;

namespace WebAPI.Validation.ResourceValidation;

public class ResourcesUpdateRequestValidator : AbstractValidator<ResourcesUpdateRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public ResourcesUpdateRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống.");

        When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
        {
            RuleFor(x => x.Id)
                .Must(resourceId => Guid.TryParse(resourceId.ToString(), out _))
                .WithMessage("Id phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
                        .MustAsync(async (resourceId, cancellation) =>
                        {
                            return await _validationServiceManager.ResourceValidationService
                                .IsExistedId(resourceId);
                        })
                        .WithMessage("Id không tồn tại.");
                });
        });

        // Validate Sponsorship
        RuleFor(x => x.Sponsorship)
            .NotEmpty().WithMessage("Id không được để trống.");

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