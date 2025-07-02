using AuthService.Domain;
using AuthService.Repository.Interface;
using AuthService.Service.Interface;
using AuthService.Service.ResponseModel;

namespace AuthService.Service.Implementation;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;

    public AccountService(IAccountRepository repository)
    {
        _repository = repository;
    }
    public async Task<bool> IsUserNameAvailable(string username)
    {
        var result = await _repository.GetAccountByUsernameAsync(username);
        return result == null;
    }

    public async Task<bool> IsAccountValid(string username, Guid accountId)
    {
        var result = await _repository.GetAccountByUsernameAsync(username);
        if (result == null) return true;
        return result.AccountId == accountId;
    }
    
    public async Task<CreateUpdate> CreateOrUpdate(Account account)
    {
        var existingAccount = await _repository.GetAccountByUsernameAsync(account.Username);
        var response = new CreateUpdate();
        if (existingAccount is not null)
        {
            existingAccount.Username = account.Username;
            response.Data = await _repository.UpdateAccount(existingAccount);
            response.Operation = Operation.Update;
        }
        else
        {
            response.Data = await _repository.Add(account);
            response.Operation = Operation.Create;
        }

        return response;
    }

}