using Application;
using Application.SendModels.RoundTopic;
using FluentValidation;

namespace WebAPI.Validation.RoundTopicValidation;

public class RoundTopicRequestValidator : AbstractValidator<RoundTopicRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public RoundTopicRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate RoundId
        RuleFor(x => x.RoundId)
            .NotEmpty().WithMessage("RoundId không được trống.")
        When(x => !string.IsNullOrEmpty(x.RoundId.ToString()), () =>
        {
            RuleFor(x => x.RoundId)
                .Must(roundId => Guid.TryParse(roundId.ToString(), out _))
                .WithMessage("RoundId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.RoundId)
                        .MustAsync(async (roundId, cancellation) =>
                        {
                            try
                            {
                                return await _validationServiceManager.RoundValidationService.IsExistedId(roundId);
                            }
                            catch (Exception)
                            {
                                // Xử lý lỗi kiểm tra ID
                                return false; // Giả sử ID không tồn tại khi có lỗi
                            }
                        })
                        .WithMessage("RoundId không tồn tại.");
                });
        });

        // Validate ListTopicId
        RuleFor(x => x.ListTopicId)
            .NotNull().WithMessage("ListTopicId không được trống.")
            .Must(list => list.All(id => id != Guid.Empty)).WithMessage("Mọi topicID trong ListTopicId phải là kiểu GUID.")
            .Must(list => list.Distinct().Count() == list.Count).WithMessage("ListTopicId không được trùng.")
            .WithMessage("ListTopicId phải có ít nhất 1 topic ID.");
    }
}