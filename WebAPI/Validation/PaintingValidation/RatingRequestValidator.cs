﻿using Application;
using Application.IService.IValidationService;
using Application.SendModels.Painting;
using Application.Services.ValidationService;
using FluentValidation;

namespace WebAPI.Validation.PaintingValidation;

public class RatingRequestValidator : AbstractValidator<RatingRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public RatingRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;


        // Validate ScheduleId
        RuleFor(x => x.ScheduleId)
            .NotEmpty().WithMessage("ScheduleId không được trống.");
        When(x => !string.IsNullOrEmpty(x.ScheduleId.ToString()), () =>
        {
            RuleFor(x => x.ScheduleId)
                .Must(scheduleId => Guid.TryParse(scheduleId.ToString(), out _))
                .WithMessage("ScheduleId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.ScheduleId)
                        .MustAsync(async (scheduleId, cancellation) =>
                        {
                            try
                            {
                                return await _validationServiceManager.ScheduleValidationService.IsExistedId(scheduleId);
                            }
                            catch (Exception)
                            {
                                // Xử lý lỗi kiểm tra ID
                                return false; // Giả sử ID không tồn tại khi có lỗi
                            }
                        })
                        .WithMessage("ScheduleId không tồn tại.");
                });
        });


        //Validate Paintings
        RuleFor(x => x.Paintings)
            .NotNull().WithMessage("Danh sách tranh không được để trống.")
            .Must(paintings => paintings != null && paintings.Any()).WithMessage("Danh sách tranh phải chứa ít nhất một mục.");

        When(x => x.Paintings != null && x.Paintings.Any(), () =>
        {
            RuleForEach(x => x.Paintings).ChildRules(painting =>
            {
                painting.RuleFor(p => p)
                    .NotEmpty().WithMessage("Mỗi GUID của tranh phải là một GUID hợp lệ.")
                    .NotEqual(Guid.Empty).WithMessage("Mỗi GUID của tranh phải là một GUID hợp lệ.")
                    .Must(paintingId => Guid.TryParse(paintingId.ToString(), out _)).WithMessage("Mỗi GUID của tranh phải là một GUID hợp lệ.")
                    .DependentRules(() =>
                    {
                        painting.RuleFor(p => p)
                            .MustAsync(async (paintingId, cancellation) =>
                            {
                                try
                                {
                                    return await _validationServiceManager.PaintingValidationService.IsExistedId(paintingId);
                                }
                                catch (Exception)
                                {
                                    // Xử lý lỗi kiểm tra ID
                                    return false; // Giả sử ID không tồn tại khi có lỗi
                                }
                            })
                            .WithMessage("Tranh với GUID không tồn tại.");
                    });
            });
        });
    }
}