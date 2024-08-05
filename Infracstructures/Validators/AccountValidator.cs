using Application.IValidators;
using Application.SendModels.AccountSendModels;
using Application.SendModels.Authentication;
using FluentValidation;

namespace Infracstructures.Validators;

public class AccountValidator : IAccountValidator
{
    public AccountValidator(IValidator<AccountUpdateRequest> accountvalidator,
        IValidator<SubAccountRequest> subaccountvalidator,
        IValidator<CreateAccountRequest> createaccountvalidator
    )
    {
        AccountUpdateRequestValidator = accountvalidator;
        SubAccountRequestValidator = subaccountvalidator;
        CreateAccountValidator = createaccountvalidator;
    }

    public IValidator<CreateAccountRequest> CreateAccountValidator { get; }
    public IValidator<AccountUpdateRequest> AccountUpdateRequestValidator { get; }

    public IValidator<SubAccountRequest> SubAccountRequestValidator { get; }
}