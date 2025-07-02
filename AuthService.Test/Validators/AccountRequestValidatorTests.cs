namespace AuthService.Test.Validators;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentValidation.TestHelper;
using AuthService.API.Validators;
using AuthService.API.RequestModels;
using AuthService.Service.Interface;

public class AccountRequestValidatorTests
{
    private readonly Mock<IAccountService> _mockService;
    private readonly AccountRequestValidator _validator;

    public AccountRequestValidatorTests()
    {
        _mockService = new Mock<IAccountService>();
        _validator = new AccountRequestValidator(_mockService.Object);
    }

    [Fact]
    public async Task Should_Have_Error_When_Username_Is_Empty()
    {
        var model = new AccountRequest { Username = "", AccountId = Guid.NewGuid() };
        var result = await _validator.TestValidateAsync(model);
        result.ShouldHaveValidationErrorFor(x => x.Username)
              .WithErrorMessage("Username is required.");
    }

    [Fact]
    public async Task Should_Have_Error_When_Username_Too_Short()
    {
        var model = new AccountRequest { Username = "abc", AccountId = Guid.NewGuid() };
        var result = await _validator.TestValidateAsync(model);
        result.ShouldHaveValidationErrorFor(x => x.Username)
              .WithErrorMessage("Username must be between 6 and 30 characters.");
    }

    [Fact]
    public async Task Should_Have_Error_When_Username_Has_Special_Characters()
    {
        var model = new AccountRequest { Username = "abc!@#", AccountId = Guid.NewGuid() };
        var result = await _validator.TestValidateAsync(model);
        result.ShouldHaveValidationErrorFor(x => x.Username)
              .WithErrorMessage("Username must be alphanumeric only.");
    }

    [Fact]
    public async Task Should_Have_Error_When_Username_Is_Taken()
    {
        var accountId = Guid.NewGuid();

        _mockService.Setup(s => s.IsAccountValid("takenUser", accountId))
                    .ReturnsAsync(false);

        var model = new AccountRequest { Username = "takenUser", AccountId = accountId };
        var result = await _validator.TestValidateAsync(model);
        result.ShouldHaveValidationErrorFor(x => x.Username)
              .WithErrorMessage("Username is already taken.");
    }

    [Fact]
    public async Task Should_Have_Error_When_AccountId_Is_Empty()
    {
        var model = new AccountRequest { Username = "ValidUser", AccountId = Guid.Empty };
        _mockService.Setup(s => s.IsAccountValid(It.IsAny<string>(), It.IsAny<Guid>()))
                    .ReturnsAsync(true);

        var result = await _validator.TestValidateAsync(model);
        result.ShouldHaveValidationErrorFor(x => x.AccountId)
              .WithErrorMessage("Account Id is required.");
    }

    [Fact]
    public async Task Should_Not_Have_Errors_When_All_Fields_Valid()
    {
        var accountId = Guid.NewGuid();
        var model = new AccountRequest { Username = "ValidUser123", AccountId = accountId };

        _mockService.Setup(s => s.IsAccountValid("ValidUser123", accountId))
                    .ReturnsAsync(true);

        var result = await _validator.TestValidateAsync(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
