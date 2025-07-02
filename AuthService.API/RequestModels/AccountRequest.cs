namespace AuthService.API.RequestModels;

public class AccountRequest
{
    public Guid AccountId { get; set; }
    public string Username { get; set; }
}