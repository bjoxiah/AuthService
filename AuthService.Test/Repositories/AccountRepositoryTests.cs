namespace AuthService.Test.Repositories;

using System;
using System.Threading.Tasks;
using AuthService.Domain;
using AuthService.Repository.DBContext;
using AuthService.Repository.Implementation;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class AccountRepositoryTests
{
        private AccountContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AccountContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AccountContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        private Account GetTestAccount()
        {
            return new Account
            {
                AccountId = Guid.NewGuid(),
                Username = "TestUser"
            };
        }

        [Fact]
        public async Task Add_Should_Save_Account()
        {
            var context = GetDbContext();
            var repo = new AccountRepository(context);
            var account = GetTestAccount();

            var result = await repo.Add(account);

            Assert.NotNull(result);
            Assert.Equal(account.Username, result.Username);
        }
        
        [Fact]
        public async Task Update_Should_Modify_Account()
        {
            var context = GetDbContext();
            var repo = new AccountRepository(context);
            var account = GetTestAccount();
            await repo.Add(account);

            account.Username = "UpdatedUser";
            var updated = await repo.UpdateAccount(account);

            Assert.Equal("UpdatedUser", updated.Username);
        }

        [Fact]
        public async Task GetAccountByAccountIdAsync_Should_Return_Account()
        {
            var context = GetDbContext();
            var repo = new AccountRepository(context);
            var account = GetTestAccount();
            await repo.Add(account);

            var result = await repo.GetAccountByAccountIdAsync(account.AccountId);

            Assert.NotNull(result);
            Assert.Equal(account.AccountId, result.AccountId);
        }

        [Fact]
        public async Task GetAccountByUsernameAsync_Should_Return_Account()
        {
            var context = GetDbContext();
            var repo = new AccountRepository(context);
            var account = GetTestAccount();
            await repo.Add(account);

            var result = await repo.GetAccountByUsernameAsync(account.Username);

            Assert.NotNull(result);
            Assert.Equal(account.Username, result.Username);
        }
}
