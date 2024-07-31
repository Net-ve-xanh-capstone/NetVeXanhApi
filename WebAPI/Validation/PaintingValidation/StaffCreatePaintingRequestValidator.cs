using System.Text.RegularExpressions;
using Application;
using Application.SendModels.Painting;
using FluentValidation;

namespace WebAPI.Validation.PaintingValidation;

public class StaffCreatePaintingRequestValidator : AbstractValidator<StaffCreatePaintingRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public StaffCreatePaintingRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate FullName
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ tên là bắt buộc.");

        // Validate Email
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email là bắt buộc.")
            .EmailAddress().WithMessage("Email phải là một địa chỉ email hợp lệ.")
            .MustAsync(async (email, cancellation) =>
            {
                // Kiểm tra số điện thoại có tồn tại không
                return !await _validationServiceManager.AccountValidationService.IsExistEmail(email);
            })
            .WithMessage("Email đã được sử dụng.");

        // Validate Address
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Địa chỉ là bắt buộc.");

        // Validate Phone
        RuleFor(user => user.Phone)
            .Must(phone => !string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, @"^0\d{9,10}$"))
            .MustAsync(async (phone, cancellation) =>
            {
                return !await _validationServiceManager.AccountValidationService.IsExistPhone(phone);
            })
            .WithMessage("Số điện thoại đã được sử dụng.");

        // Validate Birthday
        RuleFor(user => user.Birthday)
            .NotEmpty().WithMessage("Ngày sinh không được để trống.")
            .Must(BeAValidAge).WithMessage("Ngày sinh không hợp lệ.");

        // Validate Image
        RuleFor(x => x.Image)
            .NotEmpty().WithMessage("Hình ảnh là bắt buộc.")
            .Must(BeAValidUrl).WithMessage("Tranh là một URL hợp lệ.");

        // Validate Name (Painting)
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên tác phẩm là bắt buộc.");

        // Validate Description (Painting)
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Mô tả là bắt buộc.");

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

    }
    private bool BeAValidAge(DateTime birthday)
    {
        var age = DateTime.Today.Year - birthday.Year;
        if (birthday.Date > DateTime.Today.AddYears(-age)) age--;
        return age >= 0 && age <= 120; // Giới hạn tuổi từ 0 đến 120
    }
    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

}