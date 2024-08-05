using System.Text.RegularExpressions;
using Application;
using Application.SendModels.AccountSendModels;
using FluentValidation;

namespace WebAPI.Validation.AccountValidation;

public class AccountUpdateRequestValidator : AbstractValidator<AccountUpdateRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public AccountUpdateRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("CurrentUserId không được để trống.");

        When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
        {
            RuleFor(x => x.Id)
                .Must(userId => Guid.TryParse(userId.ToString(), out _))
                .WithMessage("CurrentUserId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
                        .MustAsync(async (userId, cancellation) =>
                        {
                            return await _validationServiceManager.AccountValidationService.IsExistedId(userId);
                        })
                        .WithMessage("CurrentUserId không tồn tại.");
                });
        });

        RuleFor(user => user.Birthday)
            .NotEmpty().WithMessage("Ngày sinh không được để trống.")
            .Must(BeAValidAge).WithMessage("Ngày sinh không hợp lệ.");

        RuleFor(user => user.FullName)
            .NotEmpty().WithMessage("Tên đầy đủ không được để trống.")
            .Length(2, 100).WithMessage("Tên đầy đủ phải có độ dài từ 2 đến 100 ký tự.");

        /*RuleFor(user => user.Address)
            .NotEmpty().WithMessage("Địa chỉ không được để trống.")
            .Length(10, 200).WithMessage("Địa chỉ phải có độ dài từ 10 đến 200 ký tự.");*/
        RuleFor(c => c.Avatar)
            .NotEmpty().WithMessage("Tranh không được để trống.")
            .Must(BeAValidUrl).WithMessage("Tranh là một URL hợp lệ.");

        RuleFor(user => user.Phone)
            .Must(phone => !string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, @"^0\d{9,10}$"))
            .WithMessage("Số điện thoại không hợp lệ.");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    private bool BeAValidAge(DateTime birthday)
    {
        var age = DateTime.Today.Year - birthday.Year;
        if (birthday.Date > DateTime.Today.AddYears(-age)) age--;
        return age >= 0 && age <= 120; // Giới hạn tuổi từ 0 đến 120
    }
}