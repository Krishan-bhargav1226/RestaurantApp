using System.Security.Claims;
using Application.Applications.TableStatusHistories;
using Application.Dtos.TableStatusHistories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,BranchAdmin,Manager,Staff")]
    public class TableStatusHistoryController : ControllerBase
    {
        private readonly ITableStatusHistoryApplication _tableStatusHistoryApplication;

        public TableStatusHistoryController(ITableStatusHistoryApplication tableStatusHistoryApplication)
        {
            _tableStatusHistoryApplication = tableStatusHistoryApplication;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateTableStatusHistoryDto input)
        {
            input.CreatedByUserId = GetAuthenticatedUserId();
            var result = await _tableStatusHistoryApplication.CreateAsync(input);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _tableStatusHistoryApplication.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _tableStatusHistoryApplication.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateTableStatusHistoryDto input)
        {
            var existing = await _tableStatusHistoryApplication.GetByIdAsync(id);
            input.CreatedByUserId = existing.CreatedByUserId;
            var result = await _tableStatusHistoryApplication.UpdateAsync(id, input);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _tableStatusHistoryApplication.DeleteAsync(id);
            return NoContent();
        }

        private int GetAuthenticatedUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var id) || id <= 0)
                throw new UnauthorizedAccessException("Authenticated user identity is invalid.");

            return id;
        }
    }
}