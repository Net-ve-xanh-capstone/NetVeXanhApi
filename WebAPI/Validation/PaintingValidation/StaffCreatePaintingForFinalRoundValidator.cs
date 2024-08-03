using Application;
using Application.SendModels.Painting;
using FluentValidation;

namespace WebAPI.Validation.PaintingValidation
{
    public class StaffCreatePaintingForFinalRoundValidator : AbstractValidator<StaffCreatePaintingFinalRoundRequest>
    {
        private readonly IValidationServiceManager _validationServiceManager;
        public StaffCreatePaintingForFinalRoundValidator(IValidationServiceManager validationServiceManager)
        {
            _validationServiceManager = validationServiceManager;

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

            // Validate CompetitorId
            RuleFor(x => x.CompetitorId)
                .NotEmpty().WithMessage("CompetitorId không được để trống.");

            When(x => !string.IsNullOrEmpty(x.CompetitorId.ToString()), () =>
            {
                RuleFor(x => x.CompetitorId)
                    .Must(competitorId => Guid.TryParse(competitorId.ToString(), out _))
                    .WithMessage("CompetitorId phải là một GUID hợp lệ.")
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.CompetitorId)
                            .MustAsync(async (competitorId, cancellation) =>
                            {
                                return await _validationServiceManager.AccountValidationService.IsExistedId(competitorId);
                            })
                            .WithMessage("CompetitorId không tồn tại.");
                    });
            });

            // Validate Image
            RuleFor(x => x.Image)
                .NotEmpty().WithMessage("Hình ảnh là bắt buộc.")
                .Must(BeAValidUrl).WithMessage("Tranh là một URL hợp lệ.");

            // Validate Name (Painting)
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên tác phẩm là bắt buộc.");

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
                                return await _validationServiceManager.RoundTopicValidationService.IsExistedId(roundtopicId);
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
}
