using Application.Applications.TableSessions;
using Application.Dtos.TableSessions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.TableManagement;

[Route("api/[controller]")]
[ApiController]
public class TableSessionController : ControllerBase
{
    private readonly ITableSessionApplication _tableSessionApplication;

    public TableSessionController(ITableSessionApplication tableSessionApplication)
    {
        _tableSessionApplication = tableSessionApplication;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUpdateTableSessionDto input)
    {
        var result = await _tableSessionApplication.CreateAsync(input);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _tableSessionApplication.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _tableSessionApplication.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CreateUpdateTableSessionDto input)
    {
        var result = await _tableSessionApplication.UpdateAsync(id, input);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _tableSessionApplication.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
