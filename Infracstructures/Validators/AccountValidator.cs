using Application.IValidators;
using Application.SendModels.AccountSendModels;
using Application.SendModels.Authentication;
using FluentValidation;

namespace Infracstructures.Validators;

public class AccountValidator : IAccountValidator
{
    public AccountValidator(IValidator<AccountUpdateRequest> accountvalidator,
        IValidator<SubAccountRequest> subaccountvalidator,
        IValidator<CreateAccountRequest> createaccountvalidator,
        IValidator<CreateAccountV2Request> createaccountv2validator
    )
    {
        AccountUpdateRequestValidator = accountvalidator;
        SubAccountRequestValidator = subaccountvalidator;
        CreateAccountValidator = createaccountvalidator;
        CreateAccountV2RequestValidator = createaccountv2validator;
    }

    public IValidator<CreateAccountRequest> CreateAccountValidator { get; }
    public IValidator<AccountUpdateRequest> AccountUpdateRequestValidator { get; }

    public IValidator<SubAccountRequest> SubAccountRequestValidator { get; }

    public IValidator<CreateAccountV2Request> CreateAccountV2RequestValidator { get; }
}