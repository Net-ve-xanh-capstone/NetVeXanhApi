using System.Text.RegularExpressions;
using Application;
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
            .NotEmpty().WithMessage("Tên tổ chức không được để trống");

        RuleFor(org => org.Address)
            .NotEmpty().WithMessage("Địa chỉ không được để trống");

        RuleFor(org => org.Delegate)
            .NotEmpty().WithMessage("Người đại diện không được để trống");

        RuleFor(org => org.Logo)
            .NotEmpty().WithMessage("Logo không được để trống.");

        RuleFor(user => user.PhoneNumber)
            .Must(phone => !string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, @"^0\d{9,10}$"))
            .WithMessage("Số điện thoại không hợp lệ.");

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

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}