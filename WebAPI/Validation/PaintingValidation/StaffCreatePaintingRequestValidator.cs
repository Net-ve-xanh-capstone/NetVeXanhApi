using System.Text.RegularExpressions;
using Application;
using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Painting;
using FluentValidation;

namespace WebAPI.Validation.PaintingValidation;

public class StaffCreatePaintingRequestValidator : AbstractValidator<StaffCreatePaintingRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public StaffCreatePaintingRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate FullName
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ tên là bắt buộc.")
            .MaximumLength(100).WithMessage("Họ tên phải ít hơn 100 ký tự.");

        // Validate Email
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email là bắt buộc.")
            .EmailAddress().WithMessage("Email phải là một địa chỉ email hợp lệ.");

        // Validate Address
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Địa chỉ là bắt buộc.")
            .MaximumLength(250).WithMessage("Địa chỉ phải ít hơn 250 ký tự.");

        // Validate Phone
        RuleFor(user => user.Phone)
            .Must(phone => !string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, @"^0\d{9,10}$"))
            .WithMessage("Số điện thoại không hợp lệ.");

        // Validate Birthday
        RuleFor(x => x.Birthday)
            .NotEmpty().WithMessage("Ngày sinh là bắt buộc.")
            .LessThan(DateTime.Today).WithMessage("Ngày sinh phải là một ngày trong quá khứ.");

        // Validate Image
        RuleFor(x => x.Image)
            .NotEmpty().WithMessage("Hình ảnh là bắt buộc.");

        // Validate Name (Painting)
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên tác phẩm là bắt buộc.")
            .MaximumLength(100).WithMessage("Tên tác phẩm phải ít hơn 100 ký tự.");

        // Validate Description (Painting)
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Mô tả là bắt buộc.")
            .MaximumLength(250).WithMessage("Mô tả phải ít hơn 250 ký tự.");

        // Validate RoundTopicId
        RuleFor(x => x.RoundTopicId)
            .NotEmpty().WithMessage("RoundTopicId là bắt buộc.");
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