using Application.Applications.PasswordResetOTPs;
using Application.Dtos.PasswordResetOTPs;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PasswordResetOTPController : ControllerBase
{
    private readonly IPasswordResetOTPApplication _passwordResetOTPApplication;

    public PasswordResetOTPController(IPasswordResetOTPApplication passwordResetOTPApplication)
    {
        _passwordResetOTPApplication = passwordResetOTPApplication;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUpdatePasswordResetOTPDto input)
    {
        var result = await _passwordResetOTPApplication.CreateAsync(input);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _passwordResetOTPApplication.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _passwordResetOTPApplication.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUpdatePasswordResetOTPDto input)
    {
        var result = await _passwordResetOTPApplication.UpdateAsync(id, input);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _passwordResetOTPApplication.DeleteAsync(id);
        return NoContent();
    }
}