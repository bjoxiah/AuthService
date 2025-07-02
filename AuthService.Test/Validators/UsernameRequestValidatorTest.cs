using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentValidation.TestHelper;
using AuthService.API.Validators;
using AuthService.API.RequestModels;
using AuthService.Service.Interface;

namespace AuthService.Test.Validators
{
    public class UsernameRequestValidatorTests
    {
        private readonly UsernameRequestValidator _validator;
        private readonly Mock<IAccountService> _mockAccountService;

        public UsernameRequestValidatorTests()
        {
            _mockAccountService = new Mock<IAccountService>();

            _mockAccountService
                .Setup(s => s.IsUserNameAvailable(It.IsAny<string>()))
                .ReturnsAsync(true);

            _validator = new UsernameRequestValidator(_mockAccountService.Object);
        }

        [Fact]
        public async Task Should_Have_Error_When_Username_Is_Empty()
        {
            var model = new UsernameRequest { Username = "" };

            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.Username)
                  .WithErrorMessage("Username is required.");
        }

        [Fact]
        public async Task Should_Have_Error_When_Username_Is_Too_Short()
        {
            var model = new UsernameRequest { Username = "abc" };

            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.Username)
                  .WithErrorMessage("Username must be between 6 and 30 characters.");
        }

        [Fact]
        public async Task Should_Have_Error_When_Username_Has_Special_Characters()
        {
            var model = new UsernameRequest { Username = "user@name" };

            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.Username)
                  .WithErrorMessage("Username must be alphanumeric only.");
        }

        [Fact]
        public async Task Should_Have_Error_When_Username_Is_Not_Available()
        {
            var username = "takenUser";
            _mockAccountService.Setup(s => s.IsUserNameAvailable(username)).ReturnsAsync(false);

            var model = new UsernameRequest { Username = username };

            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.Username)
                  .WithErrorMessage("Username is already taken.");
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Username_Is_Valid_And_Available()
        {
            var model = new UsernameRequest { Username = "ValidUser123" };

            var result = await _validator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
