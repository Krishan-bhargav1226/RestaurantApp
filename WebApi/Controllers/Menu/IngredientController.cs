using Application.Applications.Ingredients;
using Application.Dtos.Ingredients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,BranchAdmin,Manager")]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientApplication _ingredientApplication;

        public IngredientController(IIngredientApplication ingredientApplication)
        {
            _ingredientApplication = ingredientApplication;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateIngredientDto input)
        {
            var result = await _ingredientApplication.CreateAsync(input);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ingredientApplication.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _ingredientApplication.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateIngredientDto input)
        {
            var result = await _ingredientApplication.UpdateAsync(id, input);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ingredientApplication.DeleteAsync(id);
            return NoContent();
        }
    }
}
