﻿using Application;
using Application.SendModels.Report;
using FluentValidation;

namespace WebAPI.Validation.ReportValidation;

public class UpdateReportRequestValidator : AbstractValidator<UpdateReportRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public UpdateReportRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống.");

        When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
        {
            RuleFor(x => x.Id)
                .Must(reportId => Guid.TryParse(reportId.ToString(), out _))
                .WithMessage("Id phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
                        .MustAsync(async (reportId, cancellation) =>
                        {
                            return await _validationServiceManager.ReportValidationService.IsExistedId(reportId);
                        })
                        .WithMessage("Id không tồn tại.");
                });
        });


        // Validate Title
        RuleFor(x => x.Title);

        // Validate Description
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description phải ít hơn 500 chữ.");

        // Validate CurrentUserId
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
}