using Application.IService;
using Application.SendModels.Post;
using FluentValidation;
using WebAPI.Validation.ImageValidation;

namespace WebAPI.Validation.PostValidation;

public class UpdatePostValidator : AbstractValidator<PostUpdateRequest>
{
    private readonly IAccountService _accountService;
    public UpdatePostValidator()
    {
        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id là bắt buộc.")
            .NotEqual(Guid.Empty).WithMessage("Id phải là một GUID hợp lệ.");

        // Validate Url
        RuleFor(x => x.Url)
            .Must(BeAValidUrl).WithMessage("Url phải là một URL hợp lệ và sử dụng HTTP hoặc HTTPS.");

        // Validate CategoryId
        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty).When(x => x.CategoryId.HasValue).WithMessage("CategoryId phải là một GUID hợp lệ.");

        // Validate CurrentUserId
        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("CurrentUserId là bắt buộc.")
            .NotEqual(Guid.Empty).WithMessage("CurrentUserId phải là một GUID hợp lệ.")
            .MustAsync(async (userId, cancellation) =>
            {
                try
                {
                    return await _accountService.IsExistedId(userId);
                }
                catch (Exception)
                {
                    // Xử lý lỗi kiểm tra ID
                    return false; // Giả sử ID không tồn tại khi có lỗi
                }
            })
            .WithMessage("CurrentUserId không tồn tại.");

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