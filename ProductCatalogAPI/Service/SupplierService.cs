using ProductCatalogAPI.Models;
using ProductCatalogAPI.Repository;

namespace ProductCatalogAPI.Service
{
    public class SupplierService
    {
        private readonly SupplierRepository _supplierRepository;
        private readonly ProductRepository _productRepository;

        public SupplierService(SupplierRepository supplierRepository, ProductRepository productRepository)
        {
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
        }

        public Task<IEnumerable<Supplier>> GetAllSuppliersAsync() => _supplierRepository.GetAllAsync();
        public Task<Supplier> GetSupplierByIdAsync(string id) => _supplierRepository.GetByIdAsync(id);

        public async Task<bool> AddSupplierAsync(Supplier supplier)
        {
            var existingSupplier = (await _supplierRepository.GetAllAsync())
                                   .FirstOrDefault(s => s.Name.ToLower() == supplier.Name.ToLower());

            if (existingSupplier != null)
                return false;

            await _supplierRepository.AddAsync(supplier);
            return true;
        }

        public async Task<bool> UpdateSupplierAsync(string id, Supplier supplier)
        {
            return await _supplierRepository.UpdateAsync(id, supplier);
        }

        public async Task<bool> DeleteSupplierAsync(string id)
        {
            var hasProducts = (await _productRepository.GetAllAsync()).Any(p => p.SupplierId == id);
            if (hasProducts)
                return false;

            return await _supplierRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<object>> GetSuppliersWithProductCountAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();

            return suppliers.Select(supplier => new
            {
                supplier.Id,
                supplier.Name,
                ProductCount = products.Count(p => p.SupplierId == supplier.Id)
            });
        }

        public async Task<IEnumerable<Supplier>> SearchSuppliersAsync(string keyword)
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            return suppliers.Where(s => s.Name.ToLower().Contains(keyword.ToLower()) ||
                                        s.Contact.ToLower().Contains(keyword.ToLower()));
        }
    }
}
