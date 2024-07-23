﻿using Application.IService;
using Application.SendModels.Collection;
using FluentValidation;

namespace WebAPI.Validation.CollectionValidation;

public class CollectionRequestValidator : AbstractValidator<CollectionRequest>
{
    private readonly IAccountService _accountService;
    public CollectionRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .Length(2, 50).WithMessage("Tên phải có độ dài từ 2 đến 50 ký tự.");

        RuleFor(c => c.Image)
            .NotEmpty().WithMessage("Hình ảnh không được để trống.")
            .Must(BeAValidUrl).WithMessage("Hình ảnh phải là một URL hợp lệ.");

        RuleFor(c => c.CurrentUserId)
            .NotEmpty().WithMessage("CurrentUserId không được để trống.")
            .NotEqual(Guid.Empty).WithMessage("CurrentUserId không được là Guid.Empty.")
            .MustAsync(async (userId, cancellation) => await _accountService.IsExistedId(userId))
            .WithMessage("CurrentUserId không tồn tại.");
    }
    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}