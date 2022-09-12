using Microsoft.AspNetCore.Mvc;
using ResteuranAPI.Models;
using ResteuranAPI.Services;

namespace ResteuranAPI.Controllers;


[Route("api/account")]
[ApiController]

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    [HttpPost("register")]
    public IActionResult RegisterUser([FromBody] RegisterUserDTO registerUserDto)
    {
        _accountService.RegisterUser(registerUserDto);
        return Ok();
    }

}