using Application.Applications.Payments;
using Application.Dtos.Payments;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentApplication _paymentApplication;

    public PaymentController(IPaymentApplication paymentApplication)
    {
        _paymentApplication = paymentApplication;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUpdatePaymentDto input)
    {
        var result = await _paymentApplication.CreateAsync(input);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _paymentApplication.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _paymentApplication.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUpdatePaymentDto input)
    {
        var result = await _paymentApplication.UpdateAsync(id, input);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _paymentApplication.DeleteAsync(id);
        return NoContent();
    }
}