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
            .Must(BeAValidUrl).WithMessage("Url must be a valid URL and use HTTP or HTTPS.");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}