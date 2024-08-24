using Application.SendModels.Award;
using FluentValidation;

namespace Application.IValidators;

public interface IAwardValidator
{
    public IValidator<CreateAwardRequest> CreateAwardRequestValidator { get; }
    public IValidator<CreateDependentAwardRequest> CreateDependentAwardRequestValidator { get; }
    public IValidator<UpdateAwardRequest> UpdateAwardRequestValidator { get; }
}