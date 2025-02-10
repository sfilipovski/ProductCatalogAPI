using MongoDB.Driver;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Repository
{
    public class SupplierRepository
    {
        private readonly IMongoCollection<Supplier> _suppliers;

        public SupplierRepository(IMongoDatabase database)
        {
            _suppliers = database.GetCollection<Supplier>("suppliers");
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync() =>
            await _suppliers.Find(_ => true).ToListAsync();

        public async Task<Supplier> GetByIdAsync(string id) =>
            await _suppliers.Find(s => s.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Supplier supplier) =>
            await _suppliers.InsertOneAsync(supplier);

        public async Task<bool> UpdateAsync(string id, Supplier supplier)
        {
            var result = await _suppliers.ReplaceOneAsync(s => s.Id == id, supplier);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _suppliers.DeleteOneAsync(s => s.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
