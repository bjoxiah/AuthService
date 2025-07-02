using AuthService.Domain;
using AuthService.Repository.DBContext;
using AuthService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repository.Implementation;

public class AccountRepository : IAccountRepository
{
    private readonly AccountContext _context;

    public AccountRepository(AccountContext context)
    {
        _context = context;
    }
    public async Task<Account> Add(Account account)
    {
        var result = await _context.Accounts.AddAsync(account);
        await SaveChangesAsync();
        return result.Entity;
    }

    public async Task<Account> UpdateAccount(Account account)
    {
        var result = _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<Account?> GetAccountByAccountIdAsync(Guid accountId)
    {
        return await _context.Accounts.FirstOrDefaultAsync(x => x.AccountId == accountId);
    }

    public async Task<Account?> GetAccountByUsernameAsync(string username)
    {
        return await _context.Accounts.FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await  _context.SaveChangesAsync();
    }
}