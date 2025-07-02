using AuthService.Domain;
using AuthService.Service.ResponseModel;

namespace AuthService.Service.Interface;

public interface IAccountService
{
    Task<bool> IsUserNameAvailable(string username);
    Task<bool> IsAccountValid(string username, Guid accountId);
    Task<CreateUpdate> CreateOrUpdate(Account account);
}