using Application;
using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Collection;
using FluentValidation;

namespace WebAPI.Validation.CollectionValidation;

public class CollectionRequestValidator : AbstractValidator<CollectionRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public CollectionRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .Length(2, 50).WithMessage("Tên phải có độ dài từ 2 đến 50 ký tự.");

        /*RuleFor(c => c.Image)
            .NotEmpty().WithMessage("Hình ảnh không được để trống.")
            .Must(BeAValidUrl).WithMessage("Hình ảnh phải là một URL hợp lệ.");*/

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

        RuleFor(x => x.listPaintingId)
            .NotNull().WithMessage("Danh sách tranh không được để trống.")
            .Must(paintings => paintings != null && paintings.Any()).WithMessage("Danh sách tranh phải chứa ít nhất một mục.");

        When(x => x.listPaintingId != null && x.listPaintingId.Any(), () =>
        {
            RuleForEach(x => x.listPaintingId).ChildRules(topic =>
            {
                topic.RuleFor(p => p)
                    .NotEmpty().WithMessage("Chủ đề không được trống.")
                    .Must(paintingId => Guid.TryParse(paintingId.ToString(), out _)).WithMessage("Mỗi GUID của chủ đề phải là một GUID hợp lệ.")
                    .DependentRules(() =>
                    {
                        topic.RuleFor(p => p)
                            .MustAsync(async (paintingId, cancellation) =>
                            {
                                return await _validationServiceManager.PaintingValidationService.IsExistedId(paintingId);
                            })
                            .WithMessage("Có chủ đề không tồn tại.");
                    });
            });
        });
    }
    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}