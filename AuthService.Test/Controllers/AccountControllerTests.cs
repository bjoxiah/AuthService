namespace AuthService.Test.Controllers;

using Xunit;
using Moq;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using AuthService.API.Controllers;
using AuthService.API.RequestModels;
using AuthService.API.ResponseModel;
using AuthService.Domain;
using AuthService.Service.Interface;
using AuthService.Service.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentAssertions;

public class AccountControllerTests
{
    private readonly Mock<ILogger<AccountController>> _mockLogger = new();
    private readonly Mock<IAccountService> _mockService = new();
    private readonly Mock<IValidator<UsernameRequest>> _mockUsernameValidator = new();
    private readonly Mock<IValidator<AccountRequest>> _mockAccountValidator = new();
    private readonly Mock<IMapper> _mockMapper = new();

    private AccountController CreateController() =>
        new(_mockLogger.Object, _mockService.Object, _mockMapper.Object,
            _mockUsernameValidator.Object, _mockAccountValidator.Object);

    [Fact]
    public async Task ValidateUsername_Should_Return_Success_When_Valid()
    {
        // Arrange
        var username = "validUser";
        var request = new UsernameRequest { Username = username };

        _mockUsernameValidator
            .Setup(v => v.ValidateAsync(It.Is<UsernameRequest>(r => r.Username == username), default))
            .ReturnsAsync(new ValidationResult());

        var controller = CreateController();

        // Act
        var result = await controller.ValidateUsername(username);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().BeTrue();
        result.Message.Should().Be("Username is valid");
    }

    [Fact]
    public async Task ValidateUsername_Should_Return_Error_When_Invalid()
    {
        // Arrange
        var username = "";
        var failures = new List<ValidationFailure> { new("Username", "Username is required") };

        _mockUsernameValidator
            .Setup(v => v.ValidateAsync(It.IsAny<UsernameRequest>(), default))
            .ReturnsAsync(new ValidationResult(failures));

        var controller = CreateController();

        // Act
        var result = await controller.ValidateUsername(username);

        // Assert
        result.Success.Should().BeFalse();
        result.Data.Should().BeFalse();
        result.Errors.Should().Contain("Username is required");
    }

    [Fact]
    public async Task CreateAccount_Should_Return_Error_When_Model_Invalid()
    {
        // Arrange
        var model = new AccountRequest { Username = "", AccountId = Guid.Empty };
        var failures = new List<ValidationFailure> { new("Username", "Invalid") };

        _mockAccountValidator
            .Setup(v => v.ValidateAsync(model, default))
            .ReturnsAsync(new ValidationResult(failures));

        var controller = CreateController();

        // Act
        var result = await controller.CreateAccount(model);

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain("Invalid");
        result.Message.Should().Be("Validation failed");
    }

    [Fact]
    public async Task CreateAccount_Should_Create_And_Return_Success()
    {
        // Arrange
        var model = new AccountRequest { AccountId = Guid.NewGuid(), Username = "NewUser" };
        var domainModel = new Account { AccountId = model.AccountId, Username = model.Username };
        var response = new CreateUpdate
        {
            Operation = Operation.Create,
            Data = domainModel
        };
        var responseModel = new AccountResponse { AccountId = model.AccountId, Username = model.Username };

        _mockAccountValidator
            .Setup(v => v.ValidateAsync(It.IsAny<AccountRequest>(), default))
            .ReturnsAsync(new ValidationResult());

        _mockMapper.Setup(m => m.Map<Account>(model)).Returns(domainModel);
        _mockService.Setup(s => s.CreateOrUpdate(domainModel)).ReturnsAsync(response);
        _mockMapper.Setup(m => m.Map<AccountResponse>(domainModel)).Returns(responseModel);

        var controller = CreateController();

        // Act
        var result = await controller.CreateAccount(model);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Account created successfully");
        result.Data.Username.Should().Be("NewUser");
    }

    [Fact]
    public async Task CreateAccount_Should_Update_And_Return_Success()
    {
        // Arrange
        var model = new AccountRequest { AccountId = Guid.NewGuid(), Username = "ExistingUser" };
        var domainModel = new Account { AccountId = model.AccountId, Username = model.Username };
        var response = new CreateUpdate
        {
            Operation = Operation.Update,
            Data = domainModel
        };
        var responseModel = new AccountResponse { AccountId = model.AccountId, Username = model.Username };

        _mockAccountValidator
            .Setup(v => v.ValidateAsync(It.IsAny<AccountRequest>(), default))
            .ReturnsAsync(new ValidationResult());

        _mockMapper.Setup(m => m.Map<Account>(model)).Returns(domainModel);
        _mockService.Setup(s => s.CreateOrUpdate(domainModel)).ReturnsAsync(response);
        _mockMapper.Setup(m => m.Map<AccountResponse>(domainModel)).Returns(responseModel);

        var controller = CreateController();

        // Act
        var result = await controller.CreateAccount(model);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Account updated successfully");
        result.Data.Username.Should().Be("ExistingUser");
    }
}
