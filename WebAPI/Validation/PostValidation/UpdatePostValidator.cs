using Application;
using Application.SendModels.Post;
using FluentValidation;
using WebAPI.Validation.ImageValidation;

namespace WebAPI.Validation.PostValidation;

public class UpdatePostValidator : AbstractValidator<PostUpdateRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public UpdatePostValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate Id
        RuleFor(x => x.Id)
        .NotEmpty().WithMessage("Id không được để trống.");

        When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
        {
            RuleFor(x => x.Id)
                .Must(postId => Guid.TryParse(postId.ToString(), out _))
                .WithMessage("Id phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
                        .MustAsync(async (postId, cancellation) =>
                        {
                            return await _validationServiceManager.PostValidationService.IsExistedId(postId);
                        })
                        .WithMessage("Id không tồn tại.");
                });
        });
        // Validate Url
        RuleFor(x => x.Url)
            .Must(BeAValidUrl).WithMessage("Url phải là một URL hợp lệ và sử dụng HTTP hoặc HTTPS.");

        // Validate CategoryId
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId phải là một GUID hợp lệ.");
        When(x => !string.IsNullOrEmpty(x.CategoryId.ToString()), () =>
        {
            RuleFor(x => x.CategoryId)
                .Must(categoryId => Guid.TryParse(categoryId.ToString(), out _))
                .WithMessage("CategoryId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.CategoryId)
                        .MustAsync(async (categoryId, cancellation) =>
                        {
                            return await _validationServiceManager.CategoryValidationService.IsExistedId(categoryId);
                        })
                        .WithMessage("CategoryId không tồn tại.");
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

        // Validate DeleteImages
        RuleFor(x => x.DeleteImages)
            .NotNull().WithMessage("Danh sách DeleteImages không được để null.")
            .Must(images => images == null || images.All(image => image != Guid.Empty)).WithMessage("Mỗi GUID trong DeleteImages phải là một GUID hợp lệ.");

        // Validate NewImages
        RuleFor(x => x.NewImages)
            .NotNull().WithMessage("Danh sách NewImages không được để null.")
            .Must(images => images == null || images.Any()).WithMessage("Danh sách NewImages phải chứa ít nhất một mục.")
            .ForEach(image => image
                .SetValidator(new ImageRequestValidator())); // Giả định rằng bạn có một ImageRequestValidator để kiểm tra từng hình ảnh
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}