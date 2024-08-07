using Application;
using Application.SendModels.Painting;
using FluentValidation;

namespace WebAPI.Validation.PaintingValidation;

public class CompetitorCreatePaintingRequestValidator : AbstractValidator<CompetitorCreatePaintingRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public CompetitorCreatePaintingRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate AccountId
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("AccountId không được để trống.");

        When(x => !string.IsNullOrEmpty(x.AccountId.ToString()), () =>
        {
            RuleFor(x => x.AccountId)
                .Must(userId => Guid.TryParse(userId.ToString(), out _))
                .WithMessage("AccountId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.AccountId)
                        .MustAsync(async (userId, cancellation) =>
                        {
                            return await _validationServiceManager.AccountValidationService.IsExistedId(userId);
                        })
                        .WithMessage("AccountId không tồn tại.");
                });
        });


        // Validate Image
        RuleFor(c => c.Image)
            .NotEmpty().WithMessage("Tranh không được để trống.")
            .Must(BeAValidUrl).WithMessage("Tranh là một URL hợp lệ.");

        // Validate Name
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên là bắt buộc.")
            .MaximumLength(100).WithMessage("Tên phải ít hơn 100 ký tự.");

        // Validate Description
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Mô tả là bắt buộc.")
            .MaximumLength(250).WithMessage("Mô tả phải ít hơn 250 ký tự.");

        // Validate RoundTopicId
        RuleFor(x => x.RoundTopicId)
            .NotEmpty().WithMessage("RoundTopicId là bắt buộc.");
        When(x => !string.IsNullOrEmpty(x.RoundTopicId.ToString()), () =>
        {
            RuleFor(x => x.RoundTopicId)
                .Must(roundtopicId => Guid.TryParse(roundtopicId.ToString(), out _))
                .WithMessage("RoundTopicId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.RoundTopicId)
                        .MustAsync(async (roundtopicId, cancellation) =>
                        {
                            return await _validationServiceManager.RoundTopicValidationService.IsExistedId(
                                roundtopicId);
                        })
                        .WithMessage("RoundTopicId không tồn tại.");
                });
        });
        RuleFor(x => new { x.AccountId, x.RoundTopicId })
            .MustAsync(async (x, cancellation) =>
            {
                return !await _validationServiceManager.PaintingValidationService.IsExistedPaintingInContest(
                    x.AccountId, x.RoundTopicId);
            })
            .WithMessage("Không thể nộp(lưu) vì đã có bài dự thi.");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}