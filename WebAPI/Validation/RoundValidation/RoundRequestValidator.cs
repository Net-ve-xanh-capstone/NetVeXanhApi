using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Round;
using FluentValidation;

namespace WebAPI.Validation.RoundValidation;

public class RoundRequestValidator : AbstractValidator<RoundRequest>
{
    private readonly IAccountValidationService _accountValidationService;

    public RoundRequestValidator(IAccountValidationService accountValidationService)
    {
        _accountValidationService = accountValidationService;


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
                                return await _accountValidationService.IsExistedId(userId);
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