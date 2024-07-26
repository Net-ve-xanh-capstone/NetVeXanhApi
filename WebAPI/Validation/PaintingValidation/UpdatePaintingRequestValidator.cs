using Application;
using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Painting;
using Domain.Models;
using FluentValidation;

namespace WebAPI.Validation.PaintingValidation;

public class UpdatePaintingRequestValidator : AbstractValidator<UpdatePaintingRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public UpdatePaintingRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate Id
        RuleFor(x => x.Id)
        .NotEmpty().WithMessage("Id không được để trống.");

        When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
        {
            RuleFor(x => x.Id)
                .Must(paintingId => Guid.TryParse(paintingId.ToString(), out _))
                .WithMessage("Id phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
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
                        .WithMessage("Id không tồn tại.");
                });
        });

        // Validate AwardId
        RuleFor(x => x.AwardId)
            .NotEqual(Guid.Empty).When(x => x.AwardId.HasValue).WithMessage("AwardId không đúng.");
        When(x => !string.IsNullOrEmpty(x.AwardId.ToString()), () =>
        {
            RuleFor(x => x.AwardId)
                .Must(awardId => Guid.TryParse(awardId.ToString(), out _))
                .WithMessage("AwardId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.AwardId)
                        .MustAsync(async (awardId, cancellation) =>
                        {
                            try
                            {
                                return await _validationServiceManager.AwardValidationService.IsExistedId(awardId.Value);
                            }
                            catch (Exception)
                            {
                                // Xử lý lỗi kiểm tra ID
                                return false; // Giả sử ID không tồn tại khi có lỗi
                            }
                        })
                        .WithMessage("AwardId không tồn tại.");
                });
        });

        // Validate RoundTopicId
        RuleFor(x => x.RoundTopicId)
            .NotEmpty().WithMessage("RoundTopicId không được để trống.");
        When(x => !string.IsNullOrEmpty(x.RoundTopicId.ToString()), () =>
        {
            RuleFor(x => x.RoundTopicId)
                .Must(roundtopicId => Guid.TryParse(roundtopicId.ToString(), out _))
                .WithMessage("RoundTopicId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.RoundTopicId)
                        .MustAsync(async (roundtopicId, cancellation) =>
                        {
                            try
                            {
                                return await _validationServiceManager.RoundTopicValidationService.IsExistedId(roundtopicId);
                            }
                            catch (Exception)
                            {
                                // Xử lý lỗi kiểm tra ID
                                return false; // Giả sử ID không tồn tại khi có lỗi
                            }
                        })
                        .WithMessage("RoundTopicId không tồn tại.");
                });
        });

        // Validate AccountId
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("AccountId không được để trống.");
        When(x => !string.IsNullOrEmpty(x.AccountId.ToString()), () =>
        {
            RuleFor(x => x.AccountId)
                .Must(userId => Guid.TryParse(userId.ToString(), out _))
                .WithMessage("AccountId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.AccountId)
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
                        .WithMessage("AccountId không tồn tại.");
                });
        });

        // Validate ScheduleId
        RuleFor(x => x.ScheduleId)
            .NotEqual(Guid.Empty).When(x => x.ScheduleId.HasValue).WithMessage("ScheduleId không đúng.");
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
                                return await _validationServiceManager.ScheduleValidationService.IsExistedId(scheduleId.Value);
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

        // Validate Code
        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Code phải ít hơn 50 ký tự.");

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