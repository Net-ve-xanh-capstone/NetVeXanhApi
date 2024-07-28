using Application;
using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Award;
using Application.Services;
using Domain.Enums;
using FluentValidation;

namespace WebAPI.Validation.AwardValidation;

public class AwardRequestValidator : AbstractValidator<AwardRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public AwardRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;

        RuleFor(x => x.Rank)
            .IsEnumName(typeof(RankAward), caseSensitive: true)
            .WithMessage("Rank không đúng định dạng.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(1).WithMessage("Số lượng phải lớn hơn hoặc bằng 1.");

        RuleFor(x => x)
            .Must(model => model.Cash == 0 && model.Artifact == "Không có thông tin")
            .WithMessage("Chỉ 1 trong 2 cash hoặc artifact được trống");

        RuleFor(x => x.Cash)
            .GreaterThanOrEqualTo(0).WithMessage("Cash phải lớn hơn hoặc bằng 0.");

        RuleFor(x => x.EducationalLevelId)
            .NotEmpty().WithMessage("EducationalLevelId không được để trống.");
        When(x => !string.IsNullOrEmpty(x.EducationalLevelId.ToString()), () =>
        {
            RuleFor(x => x.EducationalLevelId)
                .Must(userId => Guid.TryParse(userId.ToString(), out _))
                .WithMessage("EducationalLevelId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.EducationalLevelId)
                        .MustAsync(async (levelId, cancellation) =>
                        {
                            try
                            {
                                return await _validationServiceManager.EducationalLevelValidationService.IsExistedId(levelId);
                            }
                            catch (Exception)
                            {
                                // Xử lý lỗi kiểm tra ID
                                return false; // Giả sử ID không tồn tại khi có lỗi
                            }
                        })
                        .WithMessage("EducationalLevelId không tồn tại.");
                });
        });

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