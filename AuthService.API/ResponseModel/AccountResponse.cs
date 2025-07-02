using AuthService.Domain;

namespace AuthService.API.ResponseModel;

public class AccountResponse
{
    public Guid AccountId { get; set; }
    public string Username { get; set; }
}