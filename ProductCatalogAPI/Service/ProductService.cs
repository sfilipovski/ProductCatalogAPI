using ProductCatalogAPI.Models;
using ProductCatalogAPI.Repository;

namespace ProductCatalogAPI.Service
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly SupplierRepository _supplierRepository;

        public ProductService(ProductRepository productRepository, CategoryRepository categoryRepository, SupplierRepository supplierRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync() => _productRepository.GetAllAsync();
        public Task<Product> GetProductByIdAsync(string id) => _productRepository.GetByIdAsync(id);

        public async Task<bool> AddProductAsync(Product product)
        {
            var existingProduct = (await _productRepository.GetAllAsync())
                                  .FirstOrDefault(p => p.Name.ToLower() == product.Name.ToLower() &&
                                                       p.CategoryId == product.CategoryId);

            if (existingProduct != null)
                return false;

            await _productRepository.AddAsync(product);
            return true;
        }

        public async Task<bool> UpdateProductAsync(string id, Product product)
        {
            return await _productRepository.UpdateAsync(id, product);
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        public async Task<bool> ReduceStockAsync(string id, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.Stock < quantity)
                return false;

            product.Stock -= quantity;
            return await _productRepository.UpdateAsync(id, product);
        }

        public async Task<IEnumerable<object>> GetProductsWithDetailsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();
            var suppliers = await _supplierRepository.GetAllAsync();

            return products.Select(product => new
            {
                product.Id,
                product.Name,
                Category = categories.FirstOrDefault(c => c.Id == product.CategoryId)?.Name ?? "Unknown",
                Supplier = suppliers.FirstOrDefault(s => s.Id == product.SupplierId)?.Name ?? "Unknown",
                product.Price,
                product.Stock
            });
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string keyword)
        {
            var products = await _productRepository.GetAllAsync();
            return products.Where(p => p.Name.ToLower().Contains(keyword.ToLower()) ||
                                       p.Description.ToLower().Contains(keyword.ToLower()));
        }
    }
}
