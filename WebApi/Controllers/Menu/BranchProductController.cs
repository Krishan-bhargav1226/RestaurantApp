using Application.Applications.BranchProducts;
using Application.Dtos.BranchProducts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,BranchAdmin,Manager")]
    public class BranchProductController : ControllerBase
    {
        private readonly IBranchProductApplication _branchProductApplication;

        public BranchProductController(IBranchProductApplication branchProductApplication)
        {
            _branchProductApplication = branchProductApplication;
        }

        // POST: api/BranchProduct
        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateBranchProductDto input)
        {
            var result = await _branchProductApplication.CreateAsync(input);

            return Ok(result);
        }

        // GET: api/BranchProduct
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _branchProductApplication.GetAllAsync();

            return Ok(result);
        }

        // GET: api/BranchProduct/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _branchProductApplication.GetByIdAsync(id);

            return Ok(result);
        }

        // PUT: api/BranchProduct/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] CreateUpdateBranchProductDto input)
        {
            var result = await _branchProductApplication.UpdateAsync(id, input);

            return Ok(result);
        }

        // DELETE: api/BranchProduct/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _branchProductApplication.DeleteAsync(id);

            return NoContent();
        }
    }
}
