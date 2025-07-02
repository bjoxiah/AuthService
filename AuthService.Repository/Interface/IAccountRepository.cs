using AuthService.Domain;

namespace AuthService.Repository.Interface;

public interface IAccountRepository
{
    Task<Account> Add(Account account);
    Task<Account> UpdateAccount(Account account);
    Task<Account?> GetAccountByAccountIdAsync(Guid accountId);
    Task<Account?> GetAccountByUsernameAsync(string username);
    Task<int> SaveChangesAsync();
}