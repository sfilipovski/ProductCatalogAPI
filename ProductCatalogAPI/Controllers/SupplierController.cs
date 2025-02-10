using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogAPI.Models;
using ProductCatalogAPI.Service;

namespace ProductCatalogAPI.Controllers
{
    [Route("api/suppliers")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly SupplierService _supplierService;

        public SupplierController(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetAllSuppliers()
        {
            return Ok(await _supplierService.GetAllSuppliersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplierById(string id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }

        [HttpPost]
        public async Task<ActionResult> AddSupplier(Supplier supplier)
        {
            var success = await _supplierService.AddSupplierAsync(supplier);
            if (!success) return BadRequest("Supplier already exists.");
            return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.Id }, supplier);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSupplier(string id, Supplier supplier)
        {
            var success = await _supplierService.UpdateSupplierAsync(id, supplier);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSupplier(string id)
        {
            var success = await _supplierService.DeleteSupplierAsync(id);
            if (!success) return BadRequest("Cannot delete supplier with associated products.");
            return NoContent();
        }

        [HttpGet("with-product-count")]
        public async Task<ActionResult> GetSuppliersWithProductCount()
        {
            return Ok(await _supplierService.GetSuppliersWithProductCountAsync());
        }

        [HttpGet("search/{keyword}")]
        public async Task<ActionResult<IEnumerable<Supplier>>> SearchSuppliers(string keyword)
        {
            return Ok(await _supplierService.SearchSuppliersAsync(keyword));
        }
    }
}
