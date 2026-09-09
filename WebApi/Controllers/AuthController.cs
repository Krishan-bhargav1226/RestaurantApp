using Application.Applications.Auth;
using Application.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthApplication _application;
    public AuthController(IAuthApplication application) => _application = application;

    [HttpPost("register-user")]
    public async Task<IActionResult> RegisterUser(RegisterDto input) => Ok(await _application.RegisterUserAsync(input));

    [HttpPost("register-customer")]
    public async Task<IActionResult> RegisterCustomer(RegisterDto input) => Ok(await _application.RegisterCustomerAsync(input));

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto input) => Ok(await _application.LoginAsync(input));
}
