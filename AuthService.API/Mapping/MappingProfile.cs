
using AuthService.API.RequestModels;
using AuthService.API.ResponseModel;
using AuthService.Domain;
using AutoMapper;

namespace AuthService.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Account, AccountResponse>();
        CreateMap<AccountRequest, Account>(); 
    }
}