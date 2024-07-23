using FluentValidation;
using Infracstructures.SendModels.Painting;

namespace WebAPI.Validation.PaintingValidation;

public class PaintingUpdateStatusRequestValidator : AbstractValidator<PaintingUpdateStatusRequest>
{

    public PaintingUpdateStatusRequestValidator()
    {
        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được trống.")
            .NotEqual(Guid.Empty).WithMessage("Id phải là kiểu GUID.");

        // Validate IsPassed
        RuleFor(x => x.IsPassed)
            .NotNull().WithMessage("IsPassed không được trống.");
    }
}