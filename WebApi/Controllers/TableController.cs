using Application.Applications.Tables;
using Application.Dtos.Tables;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TableController : ControllerBase
{
    private readonly ITableApplication _application;
    public TableController(ITableApplication application) => _application = application;

    [HttpPost] public async Task<IActionResult> Create(CreateUpdateTableDto input) => Ok(await _application.CreateAsync(input));
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _application.GetAllAsync());
    [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id) => Ok(await _application.GetByIdAsync(id));
    [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, CreateUpdateTableDto input) => Ok(await _application.UpdateAsync(id, input));
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) { await _application.DeleteAsync(id); return NoContent(); }
}
