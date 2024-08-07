using Application;
using Application.SendModels.Post;
using FluentValidation;
using WebAPI.Validation.ImageValidation;

namespace WebAPI.Validation.PostValidation;

public class PostRequestValidator : AbstractValidator<PostRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public PostRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;

        // Validate Url
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url là bắt buộc.")
            .Must(BeAValidUrl).WithMessage("Url phải là một URL hợp lệ và sử dụng HTTP hoặc HTTPS.");

        // Validate Title
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Tiêu đề là bắt buộc.");

        // Validate Description
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Mô tả là bắt buộc.");

        // Validate CategoryId
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId là bắt buộc.");
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
                            return await _validationServiceManager.CategoryValidationService
                                .IsExistedId(categoryId);
                        })
                        .WithMessage("CategoryId không tồn tại.");
                });
        });

        // Validate Images
        RuleFor(x => x.Images)
            .NotNull().WithMessage("Danh sách hình ảnh không được để null.")
            .Must(images => images != null && images.Any()).WithMessage("Danh sách hình ảnh phải chứa ít nhất một mục.")
            .ForEach(image => image
                .SetValidator(
                    new ImageRequestValidator())); // Giả định rằng bạn có một ImageRequestValidator để kiểm tra từng hình ảnh

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


    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}