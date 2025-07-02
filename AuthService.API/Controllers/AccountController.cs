using AuthService.API.RequestModels;
using AuthService.API.ResponseModel;
using AuthService.Domain;
using AuthService.Service.Interface;
using AuthService.Service.ResponseModel;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _service;
    private readonly IValidator<UsernameRequest> _usernameValidator;
    private readonly IValidator<AccountRequest> _accountValidator;
    private readonly IMapper _mapper;

    public AccountController(
        ILogger<AccountController> logger, 
        IAccountService service, 
        IMapper mapper, IValidator<UsernameRequest> usernameValidator, IValidator<AccountRequest> accountValidator)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
        _usernameValidator = usernameValidator;
        _accountValidator = accountValidator;
    }

    [HttpGet]
    public async Task<Response<bool>> ValidateUsername([FromQuery] string username)
    {
        var request = new UsernameRequest() { Username = username };
        var result = await _usernameValidator.ValidateAsync(request);
        return !result.IsValid ? Response<bool>.Fail(result.Errors.Select(e => e.ErrorMessage).ToList()) : Response<bool>.Ok(true, "Username is valid");
    }
    
    [HttpPost]
    public async Task<Response<AccountResponse>> CreateAccount([FromBody] AccountRequest model)
    {
        var result = await _accountValidator.ValidateAsync(model);
        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            return Response<AccountResponse>.Fail(errors, "Validation failed");
        }

        // Request model mapping
        var domainModel = _mapper.Map<Account>(model);
        var response = await _service.CreateOrUpdate(domainModel);
        
        // Response Model Mapping
        var responseModel = _mapper.Map<AccountResponse>(response.Data);
        return Response<AccountResponse>.Ok(responseModel, response.Operation == Operation.Create ? "Account created successfully" :  "Account updated successfully");
    }

}