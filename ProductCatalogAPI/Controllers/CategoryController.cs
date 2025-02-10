using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogAPI.Models;
using ProductCatalogAPI.Service;

namespace ProductCatalogAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            return Ok(await _categoryService.GetAllCategoriesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> AddCategory(Category category)
        {
            var success = await _categoryService.AddCategoryAsync(category);
            if (!success) return BadRequest("Category already exists.");
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(string id, Category category)
        {
            var success = await _categoryService.UpdateCategoryAsync(id, category);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(string id)
        {
            var success = await _categoryService.DeleteCategoryAsync(id);
            if (!success) return BadRequest("Cannot delete category with associated products.");
            return NoContent();
        }

        [HttpGet("with-product-count")]
        public async Task<ActionResult> GetCategoriesWithProductCount()
        {
            return Ok(await _categoryService.GetCategoriesWithProductCountAsync());
        }
    }
}
