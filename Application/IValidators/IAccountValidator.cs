using Application.SendModels.AccountSendModels;
using Application.SendModels.Authentication;
using FluentValidation;

namespace Application.IValidators;

public interface IAccountValidator
{
    IValidator<AccountUpdateRequest> AccountUpdateRequestValidator { get; }
    IValidator<SubAccountRequest> SubAccountRequestValidator { get; }
    IValidator<CreateAccountRequest> CreateAccountValidator { get; }
}