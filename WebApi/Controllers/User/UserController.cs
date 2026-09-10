using Application.Applications.Users;
using Application.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "SuperAdmin,BranchAdmin")]
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
        if (User.IsInRole("BranchAdmin") && input.Role == Domain.Entities.Enums.UserRole.SuperAdmin)
            return Forbid();

        var result = await _userApplication.CreateAsync(input);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _userApplication.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _userApplication.GetByIdAsync(id));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateUserDto input)
    {
        if (User.IsInRole("BranchAdmin") && input.Role == Domain.Entities.Enums.UserRole.SuperAdmin)
            return Forbid();

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
