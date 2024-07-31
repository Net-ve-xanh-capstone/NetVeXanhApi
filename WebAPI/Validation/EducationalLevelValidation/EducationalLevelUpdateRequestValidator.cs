using Application;
using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.EducationalLevel;
using FluentValidation;

namespace WebAPI.Validation.EducationalLevelValidation;

public class EducationalLevelUpdateRequestValidator : AbstractValidator<EducationalLevelUpdateRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public EducationalLevelUpdateRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống.");

        When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
        {
            RuleFor(x => x.Id)
                .Must(levelId => Guid.TryParse(levelId.ToString(), out _))
                .WithMessage("Id phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
                        .MustAsync(async (levelId, cancellation) =>
                        {
                            return await _validationServiceManager.EducationalLevelValidationService.IsExistedId(levelId);
                        })
                        .WithMessage("Id không tồn tại.");
                });
        });

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

        // Validate Level
        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level không được trống.");
    }
}