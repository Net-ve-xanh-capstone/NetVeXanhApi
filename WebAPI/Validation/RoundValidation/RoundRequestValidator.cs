using Application;
using Application.SendModels.Round;
using FluentValidation;

namespace WebAPI.Validation.RoundValidation;

public class RoundRequestValidator : AbstractValidator<RoundRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public RoundRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;


        RuleFor(contest => contest.Name)
            .NotEmpty().WithMessage("Tên cuộc thi không được để trống");

        RuleFor(contest => contest.StartTime)
            .NotEmpty().WithMessage("Thời gian bắt đầu không được để trống");

        RuleFor(contest => contest.EndTime)
            .NotEmpty().WithMessage("Thời gian kết thúc không được để trống");

        RuleFor(contest => contest.Location)
            .NotEmpty().WithMessage("Địa điểm không được để trống");

        RuleFor(contest => contest.Description)
            .NotEmpty().WithMessage("Mô tả không được để trống");

        RuleFor(contest => contest.listLevel)
            .NotEmpty().WithMessage("Danh sách dối tượng không được để trống");

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