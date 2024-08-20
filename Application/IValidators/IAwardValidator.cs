using Application.SendModels.Award;
using FluentValidation;

namespace Application.IValidators;

public interface IAwardValidator
{
    public IValidator<CreateAwardRequest> AwardRequestValidator { get; }
    public IValidator<UpdateAwardRequest> UpdateAwardRequestValidator { get; }
}