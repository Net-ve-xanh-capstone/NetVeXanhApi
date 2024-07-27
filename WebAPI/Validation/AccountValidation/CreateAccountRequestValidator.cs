using Application.SendModels.Authentication;
using FluentValidation;

namespace WebAPI.Validation.AccountValidation
{
    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Tên đăng nhập không được để trống.")
                .MinimumLength(3).WithMessage("Tên đăng nhập phải có ít nhất 3 ký tự.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Họ và tên không được để trống.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Email không hợp lệ.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Vai trò không được để trống.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống.")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.");

            RuleFor(x => x.Phone)
                .Matches(@"^0\d{9,10}$").When(x => !string.IsNullOrEmpty(x.Phone)).WithMessage("Số điện thoại không hợp lệ.");

            RuleFor(x => x.Birthday)
                .LessThan(DateTime.Now).WithMessage("Ngày sinh phải là ngày trong quá khứ.");
        }
    }
}
