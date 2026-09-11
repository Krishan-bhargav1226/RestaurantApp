using Application.Applications.RecipeIngredients;
using Application.Dtos.RecipeIngredients;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RecipeIngredientController : ControllerBase
    {
        private readonly IRecipeIngredientApplication _recipeIngredientApplication;

        public RecipeIngredientController(IRecipeIngredientApplication recipeIngredientApplication)
        {
            _recipeIngredientApplication = recipeIngredientApplication;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateRecipeIngredientDto input)
        {
            var result = await _recipeIngredientApplication.CreateAsync(input);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _recipeIngredientApplication.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _recipeIngredientApplication.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CreateUpdateRecipeIngredientDto input)
        {
            var result = await _recipeIngredientApplication.UpdateAsync(id, input);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _recipeIngredientApplication.DeleteAsync(id);
            return NoContent();
        }
    }
}
