﻿using Application.SendModels.Schedule;
using FluentValidation;

namespace WebAPI.Validation.ScheduleValidation;

public class ScheduleRequestValidator : AbstractValidator<ScheduleRequest>
{
    public ScheduleRequestValidator()
    {
        RuleFor(review => review.Description)
            .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự");

        RuleFor(review => review.RoundId)
            .NotEmpty().WithMessage("RoundId không được để trống")
            .NotEqual(Guid.Empty).WithMessage("RoundId không hợp lệ");

        RuleFor(review => review.EndDate)
            .GreaterThan(DateTime.Now).WithMessage("Ngày kết thúc phải lớn hơn ngày hiện tại");

        RuleFor(review => review.ListExaminer)
            .NotEmpty().WithMessage("Danh sách giám khảo không được để trống")
            .Must(list => list != null && list.Count > 0).WithMessage("Danh sách giám khảo phải có ít nhất một giám khảo")
            .Must(list => list.All(id => id != Guid.Empty)).WithMessage("Danh sách giám khảo không được chứa ID trống");

        RuleFor(review => review.CurrentUserId)
            .NotEmpty().WithMessage("ID người dùng hiện tại không được để trống")
            .NotEqual(Guid.Empty).WithMessage("ID người dùng hiện tại không hợp lệ");
    }
}