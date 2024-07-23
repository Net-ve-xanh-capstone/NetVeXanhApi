using Application.IService;
using FluentValidation;
using Infracstructures.SendModels.Sponsor;

namespace WebAPI.Validation.SponsorValidation;

public class SponsorRequestValidator : AbstractValidator<SponsorRequest>
{
    private readonly IAccountService _accountService;
    public SponsorRequestValidator()
    {
        RuleFor(org => org.Name)
            .NotEmpty().WithMessage("Tên tổ chức không được để trống")
            .Length(3, 100).WithMessage("Tên tổ chức phải có độ dài từ 3 đến 100 ký tự");

        RuleFor(org => org.Address)
            .NotEmpty().WithMessage("Địa chỉ không được để trống")
            .Length(10, 200).WithMessage("Địa chỉ phải có độ dài từ 10 đến 200 ký tự");

        RuleFor(org => org.Delegate)
            .NotEmpty().WithMessage("Người đại diện không được để trống")
            .Length(3, 100).WithMessage("Tên người đại diện phải có độ dài từ 3 đến 100 ký tự");

        RuleFor(org => org.Logo)
            .NotEmpty().WithMessage("Logo không được để trống")
            .Must(logo => Uri.IsWellFormedUriString(logo, UriKind.RelativeOrAbsolute)).WithMessage("Đường dẫn logo không hợp lệ");

        RuleFor(org => org.PhoneNumber)
            .NotEmpty().WithMessage("Số điện thoại không được để trống")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Số điện thoại không hợp lệ");

        RuleFor(org => org.CurrentUserId)
            .NotEmpty().WithMessage("ID người dùng hiện tại không được để trống")
            .NotEqual(Guid.Empty).WithMessage("ID người dùng hiện tại không hợp lệ")
            .MustAsync(async (userId, cancellation) => await _accountService.IsExistedId(userId))
            .WithMessage("CurrentUserId không tồn tại.");
    }
}