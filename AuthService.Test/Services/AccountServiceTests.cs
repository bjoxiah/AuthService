
namespace AuthService.Test.Services;
using System;
using System.Threading.Tasks;
using AuthService.Domain;
using AuthService.Repository.Interface;
using AuthService.Service.Implementation;
using AuthService.Service.ResponseModel;
using Moq;
using Xunit;
using FluentAssertions;
 public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _mockRepo;
        private readonly AccountService _service;

        public AccountServiceTests()
        {
            _mockRepo = new Mock<IAccountRepository>();
            _service = new AccountService(_mockRepo.Object);
        }

        [Fact]
        public async Task IsUserNameAvailable_Should_Return_True_When_Not_Found()
        {
            _mockRepo.Setup(r => r.GetAccountByUsernameAsync("newUser"))
                     .ReturnsAsync((Account)null);

            var result = await _service.IsUserNameAvailable("newUser");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsUserNameAvailable_Should_Return_False_When_Found()
        {
            _mockRepo.Setup(r => r.GetAccountByUsernameAsync("existing"))
                     .ReturnsAsync(new Account { AccountId = Guid.NewGuid(), Username = "existing" });

            var result = await _service.IsUserNameAvailable("existing");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsAccountValid_Should_Return_True_If_User_Not_Found()
        {
            _mockRepo.Setup(r => r.GetAccountByUsernameAsync("any"))
                     .ReturnsAsync((Account)null);

            var result = await _service.IsAccountValid("any", Guid.NewGuid());

            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsAccountValid_Should_Return_True_If_AccountId_Matches()
        {
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetAccountByUsernameAsync("user"))
                     .ReturnsAsync(new Account { AccountId = id });

            var result = await _service.IsAccountValid("user", id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsAccountValid_Should_Return_False_If_AccountId_Differs()
        {
            _mockRepo.Setup(r => r.GetAccountByUsernameAsync("user"))
                     .ReturnsAsync(new Account { AccountId = Guid.NewGuid() });

            var result = await _service.IsAccountValid("user", Guid.NewGuid());

            result.Should().BeFalse();
        }

        [Fact]
        public async Task CreateOrUpdate_Should_Create_When_Not_Existing()
        {
            var account = new Account { AccountId = Guid.NewGuid(), Username = "newuser" };
            _mockRepo.Setup(r => r.GetAccountByUsernameAsync(account.Username)).ReturnsAsync((Account)null);
            _mockRepo.Setup(r => r.Add(account)).ReturnsAsync(account);

            var result = await _service.CreateOrUpdate(account);

            result.Operation.Should().Be(Operation.Create);
            result.Data.Should().Be(account);
        }

        [Fact]
        public async Task CreateOrUpdate_Should_Update_When_Existing()
        {
            var account = new Account { AccountId = Guid.NewGuid(), Username = "existing" };
            var existing = new Account { AccountId = account.AccountId, Username = "old" };

            _mockRepo.Setup(r => r.GetAccountByUsernameAsync(account.Username)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAccount(existing)).ReturnsAsync(existing);

            var result = await _service.CreateOrUpdate(account);

            result.Operation.Should().Be(Operation.Update);
            result.Data.Username.Should().Be("existing");
        }
    }
