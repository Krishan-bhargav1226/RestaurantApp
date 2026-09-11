using Application.Applications.LoyaltyRewards;
using Application.Dtos.LoyaltyRewards;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoyaltyRewardController : ControllerBase
{
    private readonly ILoyaltyRewardApplication _loyaltyRewardApplication;

    public LoyaltyRewardController(ILoyaltyRewardApplication loyaltyRewardApplication)
    {
        _loyaltyRewardApplication = loyaltyRewardApplication;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUpdateLoyaltyRewardDto input)
    {
        var result = await _loyaltyRewardApplication.CreateAsync(input);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _loyaltyRewardApplication.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _loyaltyRewardApplication.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateLoyaltyRewardDto input)
    {
        var result = await _loyaltyRewardApplication.UpdateAsync(id, input);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _loyaltyRewardApplication.DeleteAsync(id);
        return NoContent();
    }
}