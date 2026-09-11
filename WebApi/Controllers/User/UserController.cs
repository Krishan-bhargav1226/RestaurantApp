using Application.Applications.Users;
using Application.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class UserController : ControllerBase
{
    private readonly IUserApplication _userApplication;

    public UserController(IUserApplication userApplication)
    {
        _userApplication = userApplication;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUpdateUserDto input)
    {
        var result = await _userApplication.CreateAsync(input);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userApplication.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _userApplication.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateUserDto input)
    {
        var result = await _userApplication.UpdateAsync(id, input);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _userApplication.DeleteAsync(id);
        return NoContent();
    }
}
