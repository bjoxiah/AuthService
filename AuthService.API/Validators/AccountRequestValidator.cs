using AuthService.API.RequestModels;
using AuthService.Service.Interface;
using FluentValidation;

namespace AuthService.API.Validators;

public class AccountRequestValidator : AbstractValidator<AccountRequest>
{
        public AccountRequestValidator(IAccountService service)
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(6, 30).WithMessage("Username must be between 6 and 30 characters.")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("Username must be alphanumeric only.")
                .MustAsync(async (request, username, cancellation) => await service.IsAccountValid(username, request.AccountId)).WithMessage("Username is already taken.");

            RuleFor(x => x.AccountId)
                    .NotEmpty().WithMessage("Account Id is required.");
        }
    
}