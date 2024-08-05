using Application;
using FluentValidation;
using Infracstructures.SendModels.Painting;

namespace WebAPI.Validation.PaintingValidation;

public class PaintingUpdateStatusRequestValidator : AbstractValidator<PaintingUpdateStatusRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public PaintingUpdateStatusRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống.");

        When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
        {
            RuleFor(x => x.Id)
                .Must(paintingId => Guid.TryParse(paintingId.ToString(), out _))
                .WithMessage("Id phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
                        .MustAsync(async (paintingId, cancellation) =>
                        {
                            return await _validationServiceManager.PaintingValidationService
                                .IsExistedId(paintingId);
                        })
                        .WithMessage("Id không tồn tại.");
                });
        });

        // Validate IsPassed
        RuleFor(x => x.IsPassed)
            .NotNull().WithMessage("IsPassed không được trống.");
    }
}