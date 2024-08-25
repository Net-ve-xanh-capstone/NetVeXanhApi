using System.Text.RegularExpressions;
using Application;
using Application.SendModels.Authentication;
using Domain.Enums;
using FluentValidation;

namespace WebAPI.Validation.AccountValidation;

public class CreateAccountV2RequestValidator : AbstractValidator<CreateAccountV2Request>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public CreateAccountV2RequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Tên không được để trống.");

        // Validate Email
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email không được để trống.")
            .EmailAddress().WithMessage("Email phải là một địa chỉ email hợp lệ.")
            .MustAsync(async (email, cancellation) =>
            {
                return !await _validationServiceManager.AccountValidationService.IsExistEmail(email);
            })
            .WithMessage("Email đã được sử dụng!");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Vai trò không được để trống.")
            .Must(role => Enum.IsDefined(typeof(Role), role))
            .WithMessage("Vai trò không hợp lệ.");

        RuleFor(user => user.Phone)
            .Must(phone => !string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, @"^0\d{9}$"))
            .MustAsync(async (phone, cancellation) =>
            {
                return !await _validationServiceManager.AccountValidationService.IsExistPhone(phone);
            })
            .WithMessage("Số điện thoại đã được sử dụng");

        RuleFor(user => user.Birthday)
            .NotEmpty().WithMessage("Ngày sinh không được để trống.")
            .Must(BeAValidAge).WithMessage("Ngày sinh không hợp lệ.");
    }

    private bool BeAValidAge(DateTime birthday)
    {
        var age = DateTime.Today.Year - birthday.Year;
        if (birthday.Date > DateTime.Today.AddYears(-age)) age--;
        return age >= 0 && age <= 120; // Giới hạn tuổi từ 0 đến 120
    }
}