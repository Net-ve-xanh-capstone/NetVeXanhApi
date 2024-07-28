using System.Text.RegularExpressions;
using Application;
using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.AccountSendModels;
using FluentValidation;

namespace WebAPI.Validation.AccountValidation;

public class AccountUpdateRequestValidator : AbstractValidator<AccountUpdateRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public AccountUpdateRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;

        RuleFor(user => user.Id)
            .NotEmpty().WithMessage("Id không được để trống.")
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
            .WithMessage("Id không tồn tại."); ;

        RuleFor(user => user.Birthday)
            .NotEmpty().WithMessage("Ngày sinh không được để trống.")
            .Must(BeAValidAge).WithMessage("Ngày sinh không hợp lệ.");

        RuleFor(user => user.FullName)
            .NotEmpty().WithMessage("Tên đầy đủ không được để trống.")
            .Length(2, 100).WithMessage("Tên đầy đủ phải có độ dài từ 2 đến 100 ký tự.");

        RuleFor(user => user.Address)
            .NotEmpty().WithMessage("Địa chỉ không được để trống.")
            .Length(10, 200).WithMessage("Địa chỉ phải có độ dài từ 10 đến 200 ký tự.");

        RuleFor(user => user.Phone)
            .Must(phone => !string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, @"^0\d{9,10}$"))
            .WithMessage("Số điện thoại không hợp lệ.");
    }

    private bool BeAValidAge(DateTime birthday)
    {
        var age = DateTime.Today.Year - birthday.Year;
        if (birthday.Date > DateTime.Today.AddYears(-age)) age--;
        return age >= 0 && age <= 120; // Giới hạn tuổi từ 0 đến 120
    }
}