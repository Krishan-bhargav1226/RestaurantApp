using Application.Applications.Products;
using Application.Dtos.Products;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductApplication _productApplication;

        public ProductController(IProductApplication productApplication)
        {
            _productApplication = productApplication;
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateProductDto input)
        {
            var result = await _productApplication.CreateAsync(input);

            return Ok(result);
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productApplication.GetAllAsync();

            return Ok(result);
        }

        // GET: api/Product/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productApplication.GetByIdAsync(id);

            return Ok(result);
        }

        // PUT: api/Product/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] CreateUpdateProductDto input)
        {
            var result = await _productApplication.UpdateAsync(id, input);

            return Ok(result);
        }

        // DELETE: api/Product/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productApplication.DeleteAsync(id);

            return NoContent();
        }
    }
}
