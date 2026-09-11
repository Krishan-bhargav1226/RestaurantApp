using Application.Applications.LoyaltyTransactions;
using Application.Dtos.LoyaltyTransactions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoyaltyTransactionController : ControllerBase
{
    private readonly ILoyaltyTransactionApplication _loyaltyTransactionApplication;

    public LoyaltyTransactionController(ILoyaltyTransactionApplication loyaltyTransactionApplication)
    {
        _loyaltyTransactionApplication = loyaltyTransactionApplication;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUpdateLoyaltyTransactionDto input)
    {
        var result = await _loyaltyTransactionApplication.CreateAsync(input);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _loyaltyTransactionApplication.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _loyaltyTransactionApplication.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateLoyaltyTransactionDto input)
    {
        var result = await _loyaltyTransactionApplication.UpdateAsync(id, input);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _loyaltyTransactionApplication.DeleteAsync(id);
        return NoContent();
    }
}