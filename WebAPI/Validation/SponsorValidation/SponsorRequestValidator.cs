using Application;
using Application.IService;
using Application.IService.IValidationService;
using FluentValidation;
using Infracstructures.SendModels.Sponsor;

namespace WebAPI.Validation.SponsorValidation;

public class SponsorRequestValidator : AbstractValidator<SponsorRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public SponsorRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;


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
            .NotEmpty().WithMessage("Logo không được để trống.")
            .Must(BeAValidUrl).WithMessage("Logo không là một URL hợp lệ.");

        RuleFor(org => org.PhoneNumber)
            .NotEmpty().WithMessage("Số điện thoại không được để trống")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Số điện thoại không hợp lệ");

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
                        .WithMessage("CurrentUserId không tồn tại.");
                });
        });
    }
    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}