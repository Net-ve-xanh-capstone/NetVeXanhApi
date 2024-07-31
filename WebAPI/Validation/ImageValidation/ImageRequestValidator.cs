using Application.SendModels.Image;
using FluentValidation;

namespace WebAPI.Validation.ImageValidation;

public class ImageRequestValidator : AbstractValidator<ImageRequest>
{
    public ImageRequestValidator()
    {
        // Validate Url
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url không được để trống.")
            .Must(BeAValidUrl).WithMessage("URL phải là một URL hợp lệ và sử dụng HTTP hoặc HTTPS.");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}