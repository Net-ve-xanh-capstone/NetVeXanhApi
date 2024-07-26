using Application;
using Application.IService;
using Application.IService.IValidationService;
using Application.IValidators;
using Application.SendModels.Contest;
using FluentValidation;

namespace WebAPI.Validation.ContestValidation;

public class UpdateContestValidator : AbstractValidator<UpdateContest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public UpdateContestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate Id
        RuleFor(x => x.Id)
        .NotEmpty().WithMessage("Id không được để trống.");

        When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
        {
            RuleFor(x => x.Id)
                .Must(contestId => Guid.TryParse(contestId.ToString(), out _))
                .WithMessage("Id phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
                        .MustAsync(async (contestId, cancellation) =>
                        {
                            try
                            {
                                return await _validationServiceManager.ContestValidationService.IsExistedId(contestId);
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

        //Current Id
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

        /*RuleFor(e => e.Logo)
            .Must(BeAValidUrl).WithMessage("Logo phải là một URL hợp lệ.");*/
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}