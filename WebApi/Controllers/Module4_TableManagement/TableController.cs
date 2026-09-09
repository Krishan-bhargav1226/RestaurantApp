using Application.Applications.Tables;
using Application.Dtos.Tables;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableApplication _tableApplication;

        public TableController(ITableApplication tableApplication)
        {
            _tableApplication = tableApplication;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateTableDto input)
        {
            var result = await _tableApplication.CreateAsync(input);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _tableApplication.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _tableApplication.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] CreateUpdateTableDto input)
        {
            var result = await _tableApplication.UpdateAsync(id, input);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _tableApplication.DeleteAsync(id);

            return NoContent();
        }
    }
}