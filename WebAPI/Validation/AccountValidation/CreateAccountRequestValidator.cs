using System.Text.RegularExpressions;
using Application;
using Application.SendModels.Authentication;
using Domain.Enums;
using FluentValidation;

namespace WebAPI.Validation.AccountValidation
{
    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        private readonly IValidationServiceManager _validationServiceManager;
        public CreateAccountRequestValidator(IValidationServiceManager validationServiceManager)
        {
            _validationServiceManager = validationServiceManager;

            RuleFor(user => user.Username)
            .NotEmpty().WithMessage("tên không được để trống.")
            .MustAsync(async (username, cancellation) =>
            {
                try
                {
                    // Kiểm tra số điện thoại có tồn tại không
                    return !await _validationServiceManager.AccountValidationService.IsExistPhone(username);
                }
                catch (Exception)
                {
                    // Xử lý lỗi kiểm tra số điện thoại
                    return false; // Giả sử số điện thoại đã tồn tại khi có lỗi
                }
            })
            .WithMessage("Số điện thoại đã được sử dụng.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Họ và tên không được để trống.");

            // Validate Email
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email là bắt buộc.")
                .EmailAddress().WithMessage("Email phải là một địa chỉ email hợp lệ.")
                .MustAsync(async (email, cancellation) =>
                {
                    try
                    {
                        // Kiểm tra số điện thoại có tồn tại không
                        return !await _validationServiceManager.AccountValidationService.IsExistEmail(email);
                    }
                    catch (Exception)
                    {
                        // Xử lý lỗi kiểm tra số điện thoại
                        return false; // Giả sử số điện thoại đã tồn tại khi có lỗi
                    }
                })
            .WithMessage("Email đã được sử dụng.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Vai trò không được để trống.")
                .Must(role => Enum.IsDefined(typeof(Role), role))
                .WithMessage("Vai trò không hợp lệ.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống.")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.");

            RuleFor(user => user.Phone)
            .Must(phone => !string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, @"^0\d{9,10}$"))
            .MustAsync(async (phone, cancellation) =>
            {
                try
                {
                    // Kiểm tra số điện thoại có tồn tại không
                    return !await _validationServiceManager.AccountValidationService.IsExistPhone(phone);
                }
                catch (Exception)
                {
                    // Xử lý lỗi kiểm tra số điện thoại
                    return false; // Giả sử số điện thoại đã tồn tại khi có lỗi
                }
            })
            .WithMessage("Số điện thoại đã được sử dụng.");

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

}
