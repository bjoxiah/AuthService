using System.ComponentModel.DataAnnotations;

namespace AuthService.API.RequestModels;

public class UsernameRequest
{
    public string Username { get; set; }
}