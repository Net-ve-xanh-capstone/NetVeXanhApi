using Application;
using Application.IService;
using Application.IService.IValidationService;
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
                            try
                            {
                                return await _validationServiceManager.AccountValidationService.IsExistedId(userId);
                            }
                            catch (Exception)
                            {
                                // Xử lý lỗi kiểm tra ID
                                return false; // Giả sử ID không tồn tại khi có lỗi
                            }
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
                            try
                            {
                                return await _validationServiceManager.RoundTopicValidationService.IsExistedId(roundtopicId);
                            }
                            catch (Exception)
                            {
                                // Xử lý lỗi kiểm tra ID
                                return false; // Giả sử ID không tồn tại khi có lỗi
                            }
                        })
                        .WithMessage("RoundTopicId không tồn tại.");
                });
        });
    }
    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}