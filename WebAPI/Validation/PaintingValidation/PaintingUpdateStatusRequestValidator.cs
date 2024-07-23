using Application.IService.IValidationService;
using FluentValidation;
using Infracstructures.SendModels.Painting;

namespace WebAPI.Validation.PaintingValidation;

public class PaintingUpdateStatusRequestValidator : AbstractValidator<PaintingUpdateStatusRequest>
{
    private readonly IPaintingValidationService _paintingValidationService;

    public PaintingUpdateStatusRequestValidator(IPaintingValidationService paintingValidationService)
    {
        _paintingValidationService = paintingValidationService;
        // Validate Id
        RuleFor(x => x.Id)
        .NotEmpty().WithMessage("Id không được để trống.");

        When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
        {
            RuleFor(x => x.Id)
                .Must(topicId => Guid.TryParse(topicId.ToString(), out _))
                .WithMessage("Id phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
                        .MustAsync(async (topicId, cancellation) =>
                        {
                            try
                            {
                                return await _paintingValidationService.IsExistedId(topicId);
                            }
                            catch (Exception)
                            {
                                // Xử lý lỗi kiểm tra ID
                                return false; // Giả sử ID không tồn tại khi có lỗi
                            }
                        })
                        .WithMessage("Id không tồn tại.");
                });
        });

        // Validate IsPassed
        RuleFor(x => x.IsPassed)
            .NotNull().WithMessage("IsPassed không được trống.");
    }
}