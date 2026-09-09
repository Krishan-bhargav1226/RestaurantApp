using Application.Applications.TableStatusHistories;
using Application.Dtos.TableStatusHistories;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TableStatusHistoryController : ControllerBase
{
    private readonly ITableStatusHistoryApplication _application;
    public TableStatusHistoryController(ITableStatusHistoryApplication application) => _application = application;

    [HttpPost] public async Task<IActionResult> Create(CreateUpdateTableStatusHistoryDto input) => Ok(await _application.CreateAsync(input));
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _application.GetAllAsync());
    [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id) => Ok(await _application.GetByIdAsync(id));
    [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, CreateUpdateTableStatusHistoryDto input) => Ok(await _application.UpdateAsync(id, input));
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) { await _application.DeleteAsync(id); return NoContent(); }
}
