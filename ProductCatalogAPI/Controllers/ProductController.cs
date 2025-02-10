using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogAPI.Models;
using ProductCatalogAPI.Service;

namespace ProductCatalogAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            return Ok(await _productService.GetAllProductsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            var success = await _productService.AddProductAsync(product);
            if (!success) return BadRequest("Product already exists in this category.");
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(string id, Product product)
        {
            var success = await _productService.UpdateProductAsync(id, product);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/reduce-stock/{quantity}")]
        public async Task<ActionResult> ReduceStock(string id, int quantity)
        {
            var success = await _productService.ReduceStockAsync(id, quantity);
            if (!success) return BadRequest("Insufficient stock.");
            return NoContent();
        }

        [HttpGet("with-details")]
        public async Task<ActionResult> GetProductsWithDetails()
        {
            return Ok(await _productService.GetProductsWithDetailsAsync());
        }

        [HttpGet("search/{keyword}")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts(string keyword)
        {
            return Ok(await _productService.SearchProductsAsync(keyword));
        }
    }
}
