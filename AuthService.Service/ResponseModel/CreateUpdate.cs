using AuthService.Domain;

namespace AuthService.Service.ResponseModel;

public class CreateUpdate
{
    public Account Data { get; set; }
    public Operation Operation { get; set; }
}