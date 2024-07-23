﻿using Application.IService;
using Application.SendModels.Collection;
using FluentValidation;

namespace WebAPI.Validation.CollectionValidation;

public class UpdateCollectionRequestValidator : AbstractValidator<UpdateCollectionRequest>
{
    private readonly IAccountService _accountService;
    public UpdateCollectionRequestValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id không được để trống.")
            .NotEqual(Guid.Empty).WithMessage("Id không được là Guid.Empty.");

        RuleFor(c => c.CurrentUserId)
            .NotEmpty().WithMessage("CurrentUserId không được để trống.")
            .NotEqual(Guid.Empty).WithMessage("CurrentUserId không được là Guid.Empty.")
            .MustAsync(async (userId, cancellation) =>
            {
                try
                {
                    return await _accountService.IsExistedId(userId);
                }
                catch (Exception)
                {
                    // Xử lý lỗi kiểm tra ID
                    return false; // Giả sử ID không tồn tại khi có lỗi
                }
            })
            .WithMessage("CurrentUserId không tồn tại.");
    }
}