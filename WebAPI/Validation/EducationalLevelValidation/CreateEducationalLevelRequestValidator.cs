using Application.SendModels.AccountSendModels;
using Application.SendModels.EducationalLevel;
using FluentValidation;

namespace WebAPI.Validation.EducationalLevelValidation
{
    public class CreateEducationalLevelRequestValidator : AbstractValidator<CreateEducationalLevelRequest>
    {
    }
}
