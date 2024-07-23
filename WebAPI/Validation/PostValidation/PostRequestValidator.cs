using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Post;
using FluentValidation;
using WebAPI.Validation.ImageValidation;

namespace WebAPI.Validation.PostValidation;

public class PostRequestValidator : AbstractValidator<PostRequest>
{
    private readonly IAccountValidationService _accountValidationService;

    public PostRequestValidator(IAccountValidationService accountValidationService)
    {
        _accountValidationService = accountValidationService;
        // Validate Url
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url là bắt buộc.")
            .Must(BeAValidUrl).WithMessage("Url phải là một URL hợp lệ và sử dụng HTTP hoặc HTTPS.");

        // Validate Title
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Tiêu đề là bắt buộc.")
            .MaximumLength(100).WithMessage("Tiêu đề phải ít hơn 100 ký tự.");

        // Validate Description
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Mô tả là bắt buộc.")
            .MaximumLength(500).WithMessage("Mô tả phải ít hơn 500 ký tự.");

        // Validate CategoryId
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId là bắt buộc.")
            .NotEqual(Guid.Empty).WithMessage("CategoryId phải là một GUID hợp lệ.");

        // Validate Images
        RuleFor(x => x.Images)
            .NotNull().WithMessage("Danh sách hình ảnh không được để null.")
            .Must(images => images != null && images.Any()).WithMessage("Danh sách hình ảnh phải chứa ít nhất một mục.")
            .ForEach(image => image
                .SetValidator(new ImageRequestValidator())); // Giả định rằng bạn có một ImageRequestValidator để kiểm tra từng hình ảnh

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
                            try
                            {
                                return await _accountValidationService.IsExistedId(userId);
                            }
                            catch (Exception)
                            {
                                // Xử lý lỗi kiểm tra ID
                                return false; // Giả sử ID không tồn tại khi có lỗi
                            }
                        })
                        .WithMessage("CurrentUserId không tồn tại.");
                });
        });
    }


    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}