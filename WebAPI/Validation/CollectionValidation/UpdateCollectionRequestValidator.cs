using Application;
using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Collection;
using FluentValidation;

namespace WebAPI.Validation.CollectionValidation;

public class UpdateCollectionRequestValidator : AbstractValidator<UpdateCollectionRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public UpdateCollectionRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống.");

        When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
        {
            RuleFor(x => x.Id)
                .Must(collectionId => Guid.TryParse(collectionId.ToString(), out _))
                .WithMessage("Id phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
                        .MustAsync(async (collectionId, cancellation) =>
                        {
                            return await _validationServiceManager.CollectionValidationService.IsExistedId(collectionId);
                        })
                        .WithMessage("Id không tồn tại.");
                });
        });

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

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Tên không được để trống.");

        RuleFor(c => c.Image)
            .NotEmpty().WithMessage("Hình ảnh không được để trống.")
            .Must(BeAValidUrl).WithMessage("Hình ảnh phải là một URL hợp lệ.");
    }
    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}