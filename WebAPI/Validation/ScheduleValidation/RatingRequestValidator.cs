using Application;
using Application.SendModels.Schedule;
using FluentValidation;

namespace WebAPI.Validation.ScheduleValidation;

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
                            return await _validationServiceManager.ScheduleValidationService.IsExistedId(scheduleId);
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
                    .NotEmpty().WithMessage("Không có tranh nào để chấm.")
                    .Must(paintingId => Guid.TryParse(paintingId.ToString(), out _)).WithMessage("Mỗi GUID của tranh phải là một GUID hợp lệ.")
                    .DependentRules(() =>
                    {
                        painting.RuleFor(p => p)
                            .MustAsync(async (paintingId, cancellation) =>
                            {
                                return await _validationServiceManager.PaintingValidationService.IsExistedId(paintingId);
                            })
                            .WithMessage("Tranh với GUID không tồn tại.");
                    });
            });
        });
    }
}