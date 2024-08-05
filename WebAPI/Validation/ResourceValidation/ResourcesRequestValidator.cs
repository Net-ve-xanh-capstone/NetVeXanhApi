using Application;
using Application.SendModels.Resources;
using FluentValidation;

namespace WebAPI.Validation.ResourceValidation;

public class ResourcesRequestValidator : AbstractValidator<ResourcesRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;

    public ResourcesRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;


        // Validate Sponsorship
        RuleFor(x => x.Sponsorship)
            .NotEmpty().WithMessage("Sponsorship không được trống.")
            .MaximumLength(200).WithMessage("Sponsorship phải ít hơn 200 chữ.");

        // Validate SponsorId
        RuleFor(x => x.SponsorId)
            .NotEmpty().WithMessage("SponsorId không được trống.");
        When(x => !string.IsNullOrEmpty(x.SponsorId.ToString()), () =>
        {
            RuleFor(x => x.SponsorId)
                .Must(sponsorId => Guid.TryParse(sponsorId.ToString(), out _))
                .WithMessage("SponsorId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.SponsorId)
                        .MustAsync(async (sponsorId, cancellation) =>
                        {
                            return await _validationServiceManager.SponsorValidationService.IsExistedId(sponsorId);
                        })
                        .WithMessage("SponsorId không tồn tại.");
                });
        });

        // Validate ContestId
        RuleFor(x => x.ContestId)
            .NotEmpty().WithMessage("ContestId không được trống.");
        When(x => !string.IsNullOrEmpty(x.ContestId.ToString()), () =>
        {
            RuleFor(x => x.ContestId)
                .Must(contestId => Guid.TryParse(contestId.ToString(), out _))
                .WithMessage("ContestId phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.ContestId)
                        .MustAsync(async (contestId, cancellation) =>
                        {
                            return await _validationServiceManager.ContestValidationService.IsExistedId(contestId);
                        })
                        .WithMessage("ContestId không tồn tại.");
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
                            return await _validationServiceManager.AccountValidationService.IsExistedId(userId);
                        })
                        .WithMessage("CurrentUserId không tồn tại.");
                });
        });
    }
}