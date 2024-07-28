using Application;
using Application.IService;
using Application.IService.IValidationService;
using Application.SendModels.Painting;
using Domain.Models;
using FluentValidation;

namespace WebAPI.Validation.PaintingValidation;

public class UpdatePaintingRequestValidator : AbstractValidator<UpdatePaintingRequest>
{
    private readonly IValidationServiceManager _validationServiceManager;
    public UpdatePaintingRequestValidator(IValidationServiceManager validationServiceManager)
    {
        _validationServiceManager = validationServiceManager;
        // Validate Id
        RuleFor(x => x.Id)
        .NotEmpty().WithMessage("Id không được để trống.");

        When(x => !string.IsNullOrEmpty(x.Id.ToString()), () =>
        {
            RuleFor(x => x.Id)
                .Must(paintingId => Guid.TryParse(paintingId.ToString(), out _))
                .WithMessage("Id phải là một GUID hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
                        .MustAsync(async (paintingId, cancellation) =>
                        {
                            try
                            {
                                return await _validationServiceManager.PaintingValidationService.IsExistedId(paintingId);
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


        // Validate RoundTopicId
        RuleFor(x => x.RoundTopicId)
            .NotEmpty().WithMessage("RoundTopicId không được để trống.");
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
                                return await _validationServiceManager.RoundTopicValidationService.IsExistedId(roundtopicId.Value);
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


        
    }

}