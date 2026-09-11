using Application.Applications.StockTransactions;
using Application.Dtos.StockTransactions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockTransactionController : ControllerBase
{
    private readonly IStockTransactionApplication _stockTransactionApplication;

    public StockTransactionController(IStockTransactionApplication stockTransactionApplication)
    {
        _stockTransactionApplication = stockTransactionApplication;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUpdateStockTransactionDto input)
    {
        var result = await _stockTransactionApplication.CreateAsync(input);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _stockTransactionApplication.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _stockTransactionApplication.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateStockTransactionDto input)
    {
        var result = await _stockTransactionApplication.UpdateAsync(id, input);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _stockTransactionApplication.DeleteAsync(id);
        return NoContent();
    }
}