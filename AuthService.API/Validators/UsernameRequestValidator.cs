using AuthService.API.RequestModels;
using AuthService.Repository.Interface;
using AuthService.Service.Interface;

namespace AuthService.API.Validators;

using FluentValidation;

public class UsernameRequestValidator : AbstractValidator<UsernameRequest>
{
    public UsernameRequestValidator(IAccountService service)
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(6, 30).WithMessage("Username must be between 6 and 30 characters.")
            .Matches("^[a-zA-Z0-9]*$").WithMessage("Username must be alphanumeric only.")
            .MustAsync(async (username, cancellation) =>
            {
                return await service.IsUserNameAvailable(username);
            }).WithMessage("Username is already taken.");
    }
}