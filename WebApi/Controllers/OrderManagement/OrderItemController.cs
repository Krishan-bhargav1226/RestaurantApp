using Application.Applications.OrderItems;
using Application.Dtos.OrderItems;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderItemController : ControllerBase
{
    private readonly IOrderItemApplication _orderItemApplication;

    public OrderItemController(IOrderItemApplication orderItemApplication)
    {
        _orderItemApplication = orderItemApplication;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUpdateOrderItemCrudDto input)
    {
        var result = await _orderItemApplication.CreateAsync(input);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _orderItemApplication.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _orderItemApplication.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateOrderItemCrudDto input)
    {
        var result = await _orderItemApplication.UpdateAsync(id, input);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _orderItemApplication.DeleteAsync(id);
        return NoContent();
    }
}