using Application.Applications.Customers;
using Application.DTOs.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "SuperAdmin,BranchAdmin")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerApplication _customerApplication;

    public CustomerController(ICustomerApplication customerApplication)
    {
        _customerApplication = customerApplication;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUpdateCustomerDto input)
    {
        var result = await _customerApplication.CreateAsync(input);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _customerApplication.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _customerApplication.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateCustomerDto input)
    {
        var result = await _customerApplication.UpdateAsync(id, input);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _customerApplication.DeleteAsync(id);
        return NoContent();
    }
}
