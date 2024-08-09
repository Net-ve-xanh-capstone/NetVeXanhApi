using Application;
using Application.SendModels.Award;
using Domain.Enums;
using FluentValidation;

namespace WebAPI.Validation.AwardValidation;

public class UpdateAwardRequestValidator : AbstractValidator<UpdateAwardRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public UpdateAwardRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;


        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống.");

        When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
        {
            RuleFor(x => x.Id)
                .Must(topicId => Guid.TryParse(topicId.ToString(), out _))
                .WithMessage("Id phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
                        .MustAsync(async (topicId, cancellation) =>
                        {
                            return await _validationServiceManager.AwardValidationService.IsExistedId(topicId);
                        })
                        .WithMessage("Id không tồn tại.");
                });
        });
        
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(1).WithMessage("Quantity phải lớn hơn hoặc bằng 1.");

        RuleFor(x => x)
            .Must(model => model.Cash != 0 || model.Artifact != "Không có thông tin")
            .WithMessage("Chỉ 1 trong 2 cash hoặc artifact được trống");

        RuleFor(x => x.Cash)
            .GreaterThanOrEqualTo(0).WithMessage("Cash phải lớn hơn hoặc bằng 0.");

        //CurrentUserId
        RuleFor(x => x.UpdatedBy)
            .NotEmpty().WithMessage("CurrentUserId không được để trống.");

        When(x => !string.IsNullOrEmpty(x.UpdatedBy.ToString()), () =>
        {
            RuleFor(x => x.UpdatedBy)
                .Must(userId => Guid.TryParse(userId.ToString(), out _))
                .WithMessage("CurrentUserId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.UpdatedBy)
                        .MustAsync(async (userId, cancellation) =>
                        {
                            return await _validationServiceManager.AccountValidationService.IsExistedId(userId);
                        })
                        .WithMessage("UpdatedBy không tồn tại.");
                });
        });
    }
}