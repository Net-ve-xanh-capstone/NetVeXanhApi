using Application;
using Application.SendModels.Round;
using FluentValidation;

namespace WebAPI.Validation.RoundValidation;

public class RoundUpdateRequestValidator : AbstractValidator<RoundUpdateRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public RoundUpdateRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;

        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống.");

        When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
        {
            RuleFor(x => x.Id)
                .Must(roundId => Guid.TryParse(roundId.ToString(), out _))
                .WithMessage("Id phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
                        .MustAsync(async (roundId, cancellation) =>
                        {
                            return await _validationServiceManager.RoundValidationService.IsExistedId(roundId);
                        })
                        .WithMessage("Id không tồn tại.");
                });
        });

        RuleFor(contest => contest.Name)
            .NotEmpty().WithMessage("Tên cuộc thi không được để trống");

        RuleFor(contest => contest.StartTime)
            .NotEmpty().WithMessage("Thời gian bắt đầu không được để trống")
            .LessThan(contest => contest.EndTime).WithMessage("Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc");

        RuleFor(contest => contest.EndTime)
            .NotEmpty().WithMessage("Thời gian kết thúc không được để trống");

        RuleFor(contest => contest.Location)
            .NotEmpty().WithMessage("Địa điểm không được để trống");

        RuleFor(contest => contest.Description)
            .NotEmpty().WithMessage("Mô tả không được để trống");

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
}