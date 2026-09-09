using Application.Applications.Branches;
using Application.DTOs.Branches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,BranchAdmin")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchApplication _branchApplication;

        public BranchController(IBranchApplication branchApplication)
        {
            _branchApplication = branchApplication;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateBranchDto input)
        {
            var result = await _branchApplication.CreateAsync(input);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _branchApplication.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _branchApplication.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateBranchDto input)
        {
            var result = await _branchApplication.UpdateAsync(id, input);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _branchApplication.DeleteAsync(id);
            return NoContent();
        }
    }
}
