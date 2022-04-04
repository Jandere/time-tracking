using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class AccountController : BaseApiController
{
    private readonly IIdentityService _identityService;

    public AccountController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<AuthenticateResponse>> Login([FromBody] AuthenticateRequest request)
    {
        var result = await _identityService.LoginAsync(request);
        return Ok(result);
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<AuthenticateResponse>> AdministratorRegister([FromBody] AdministratorRegisterRequest request)
    {
        var result = await _identityService.AdministratorRegisterAsync(request);
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult<AuthenticateResponse>> DeveloperRegister([FromBody] DeveloperRegisterRequest request)
    {
        var result = await _identityService.DeveloperRegisterAsync(request);
        return Ok(result);
    }
}