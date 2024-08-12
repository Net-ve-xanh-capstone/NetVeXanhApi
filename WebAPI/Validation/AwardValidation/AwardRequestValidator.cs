using Application;
using Application.SendModels.Award;
using Domain.Enums;
using FluentValidation;

namespace WebAPI.Validation.AwardValidation;

public class AwardRequestValidator : AbstractValidator<CreateAwardSendModel>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public AwardRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(1).WithMessage("Số lượng phải lớn hơn hoặc bằng 1.");

        RuleFor(x => x)
            .Must(model => model.Cash != 0 || model.Artifact != "Không có thông tin")
            .WithMessage("Chỉ 1 trong 2 cash hoặc artifact được trống");

        RuleFor(x => x.Cash)
            .GreaterThanOrEqualTo(0).WithMessage("Cash phải lớn hơn hoặc bằng 0.");
        

        RuleFor(x => x.RoundId)
            .NotEmpty().WithMessage("EducationalLevelId không được để trống.");
        When(x => !string.IsNullOrEmpty(x.RoundId.ToString()), () =>
        {
            RuleFor(x => x.RoundId)
                .Must(levelId => Guid.TryParse(levelId.ToString(), out _))
                .WithMessage("EducationalLevelId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.RoundId)
                        .MustAsync(async (levelId, cancellation) =>
                        {
                            return await _validationServiceManager.RoundValidationService.IsExistedId(
                                levelId);
                        })
                        .WithMessage("EducationalLevelId không tồn tại.");
                });
        });

        RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("CurrentUserId không được để trống.");

        When(x => !string.IsNullOrEmpty(x.CreatedBy.ToString()), () =>
        {
            RuleFor(x => x.CreatedBy)
                .Must(userId => Guid.TryParse(userId.ToString(), out _))
                .WithMessage("CurrentUserId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.CreatedBy)
                        .MustAsync(async (userId, cancellation) =>
                        {
                            return await _validationServiceManager.AccountValidationService.IsExistedId(userId);
                        })
                        .WithMessage("CurrentUserId không tồn tại.");
                });
        });
    }
}