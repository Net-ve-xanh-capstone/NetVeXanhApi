using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Contest;
using FluentValidation;

namespace WebAPI.Validation.ContestValidation;

public class ContestRequestValidator : AbstractValidator<ContestRequest>
{
    private readonly IAccountValidationService _accountValidationService;

    public ContestRequestValidator(IAccountValidationService accountValidationService)
    {
        _accountValidationService = accountValidationService;
        RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .Length(2, 100).WithMessage("Tên phải có độ dài từ 2 đến 100 ký tự.");

        RuleFor(e => e.StartTime)
            .NotEmpty().WithMessage("Thời gian bắt đầu không được để trống.")
            .LessThan(e => e.EndTime).WithMessage("Thời gian bắt đầu phải trước thời gian kết thúc.");

        RuleFor(e => e.EndTime)
            .NotEmpty().WithMessage("Thời gian kết thúc không được để trống.");

        RuleFor(e => e.Description)
            .MaximumLength(500).WithMessage("Mô tả không được quá 500 ký tự.");

        RuleFor(e => e.Content)
            .NotEmpty().WithMessage("Nội dung không được để trống.");

        RuleFor(e => e.Logo)
            .Must(BeAValidUrl).WithMessage("Logo phải là một URL hợp lệ.");

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

        RuleFor(e => e.Round1StartTime)
            .NotEmpty().WithMessage("Thời gian bắt đầu vòng 1 không được để trống.")
            .LessThan(e => e.Round1EndTime).WithMessage("Thời gian bắt đầu vòng 1 phải trước thời gian kết thúc vòng 1.");

        RuleFor(e => e.Round1EndTime)
            .NotEmpty().WithMessage("Thời gian kết thúc vòng 1 không được để trống.")
            .GreaterThan(e => e.Round1StartTime).WithMessage("Thời gian kết thúc vòng 1 phải sau thời gian bắt đầu vòng 1.");

        RuleFor(e => e.Round2StartTime)
            .NotEmpty().WithMessage("Thời gian bắt đầu vòng 2 không được để trống.")
            .LessThan(e => e.Round2EndTime).WithMessage("Thời gian bắt đầu vòng 2 phải trước thời gian kết thúc vòng 2.");

        RuleFor(e => e.Round2EndTime)
            .NotEmpty().WithMessage("Thời gian kết thúc vòng 2 không được để trống.")
            .GreaterThan(e => e.Round2StartTime).WithMessage("Thời gian kết thúc vòng 2 phải sau thời gian bắt đầu vòng 2.");

        RuleFor(e => e.Rank1)
            .GreaterThanOrEqualTo(1).WithMessage("Số lượng giải nhất phải lớn hơn hoặc bằng 1.");

        RuleFor(e => e.Rank2)
            .GreaterThanOrEqualTo(1).WithMessage("Số lượng giải nhì phải lớn hơn hoặc bằng 1");

        RuleFor(e => e.Rank3)
            .GreaterThanOrEqualTo(1).WithMessage("Số lượng giải ba phải lớn hơn hoặc bằng 1.");

        RuleFor(e => e.Rank4)
            .GreaterThanOrEqualTo(1).WithMessage("Số lượng giải tư phải lớn hơn hoặc bằng 1.");

        RuleFor(e => e.PassRound1)
            .GreaterThanOrEqualTo(1).WithMessage("Số lượng qua vòng 1 phải lớn hơn hoặc bằng 1.");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}