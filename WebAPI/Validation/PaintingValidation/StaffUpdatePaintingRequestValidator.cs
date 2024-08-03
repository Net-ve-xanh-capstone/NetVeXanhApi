using Application;
using Application.SendModels.Painting;
using FluentValidation;

namespace WebAPI.Validation.PaintingValidation
{
    public class StaffUpdatePaintingRequestValidator : AbstractValidator<StaffUpdatePaintingRequest>
    {
        private readonly IValidationServiceManager _validationServiceManager;
        public StaffUpdatePaintingRequestValidator(IValidationServiceManager validationServiceManager)
        {
            // Validate Id
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id không được để trống.");

            When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
            {
                RuleFor(x => x.Id)
                    .Must(paintingId => Guid.TryParse(paintingId.ToString(), out _))
                    .WithMessage("Id phải là một GUID hợp lệ.")
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.Id)
                            .MustAsync(async (paintingId, cancellation) =>
                            {
                                return await _validationServiceManager.PaintingValidationService.IsExistedId(paintingId);
                            })
                            .WithMessage("Id không tồn tại.");
                    });
            });
            // Validate FullName
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Họ tên là bắt buộc.");

            // Validate Email
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email là bắt buộc.")
                .EmailAddress().WithMessage("Email phải là một địa chỉ email hợp lệ.")
                .MustAsync(async (email, cancellation) =>
                {
                    return !await _validationServiceManager.AccountValidationService.IsExistEmail(email);
                })
                .WithMessage("Email đã được sử dụng!");

        }
    }
}
