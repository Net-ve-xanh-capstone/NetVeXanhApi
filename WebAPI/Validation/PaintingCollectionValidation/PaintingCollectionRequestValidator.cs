using Application;
using Application.SendModels.PaintingCollection;
using FluentValidation;

namespace WebAPI.Validation.PaintingCollectionValidation;

public class PaintingCollectionRequestValidator : AbstractValidator<PaintingCollectionRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public PaintingCollectionRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;

        //CurrentUserId
        RuleFor(x => x.CollectionId)
            .NotEmpty().WithMessage("CurrentUserId không được để trống.");

        When(x => !string.IsNullOrEmpty(x.CollectionId.ToString()), () =>
        {
            RuleFor(x => x.CollectionId)
                .Must(collectionId => Guid.TryParse(collectionId.ToString(), out _))
                .WithMessage("CollectionId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.CollectionId)
                        .MustAsync(async (collectionId, cancellation) =>
                        {
                            return await _validationServiceManager.CollectionValidationService.IsExistedId(collectionId);
                        })
                        .WithMessage("CollectionId không tồn tại.");
                });
        });


        RuleFor(x => x.PaintingId)
            .NotEmpty().WithMessage("CurrentUserId không được để trống.");

        When(x => !string.IsNullOrEmpty(x.PaintingId.ToString()), () =>
        {
            RuleFor(x => x.PaintingId)
                .Must(paintingId => Guid.TryParse(paintingId.ToString(), out _))
                .WithMessage("PaintingId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.PaintingId)
                        .MustAsync(async (paintingId, cancellation) =>
                        {
                            return await _validationServiceManager.PaintingValidationService.IsExistedId(paintingId);
                        })
                        .WithMessage("PaintingId không tồn tại.");
                });
        });

        RuleFor(x => new { x.PaintingId, x.CollectionId })
            .MustAsync(async (x, cancellation) =>
            {
                return !await _validationServiceManager.PaintingCollectionValidationService.IsPaintingInCollection(x.PaintingId, x.CollectionId);
            })
            .WithMessage("Đã có painting trong collection.");

    }
}