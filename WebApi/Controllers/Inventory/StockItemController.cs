using Application.Applications.StockItems;
using Application.Dtos.StockItems;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockItemController : ControllerBase
{
    private readonly IStockItemApplication _stockItemApplication;

    public StockItemController(IStockItemApplication stockItemApplication)
    {
        _stockItemApplication = stockItemApplication;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUpdateStockItemDto input)
    {
        var result = await _stockItemApplication.CreateAsync(input);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _stockItemApplication.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _stockItemApplication.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateStockItemDto input)
    {
        var result = await _stockItemApplication.UpdateAsync(id, input);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _stockItemApplication.DeleteAsync(id);
        return NoContent();
    }
}