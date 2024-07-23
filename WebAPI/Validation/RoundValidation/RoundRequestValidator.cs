﻿using Application.SendModels.Round;
using FluentValidation;

namespace WebAPI.Validation.RoundValidation;

public class RoundRequestValidator : AbstractValidator<RoundRequest>
{
    public RoundRequestValidator()
    {
        RuleFor(contest => contest.Name)
            .NotEmpty().WithMessage("Tên cuộc thi không được để trống")
            .Length(3, 100).WithMessage("Tên cuộc thi phải có độ dài từ 3 đến 100 ký tự");

        RuleFor(contest => contest.StartTime)
            .LessThan(contest => contest.EndTime).WithMessage("Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc");

        RuleFor(contest => contest.EndTime)
            .GreaterThan(contest => contest.StartTime).WithMessage("Thời gian kết thúc phải lớn hơn thời gian bắt đầu");

        RuleFor(contest => contest.Location)
            .NotEmpty().WithMessage("Địa điểm không được để trống")
            .Length(3, 100).WithMessage("Địa điểm phải có độ dài từ 3 đến 100 ký tự");

        RuleFor(contest => contest.Description)
            .NotEmpty().WithMessage("Mô tả không được để trống");

        RuleFor(contest => contest.listLevel)
            .NotEmpty().WithMessage("Danh sách cấp độ không được để trống");

        RuleFor(contest => contest.CurrentUserId)
            .NotEmpty().WithMessage("ID người dùng hiện tại không được để trống")
            .NotEqual(Guid.Empty).WithMessage("ID người dùng hiện tại không hợp lệ");
    }
}