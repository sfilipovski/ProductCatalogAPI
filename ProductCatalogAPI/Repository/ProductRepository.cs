using MongoDB.Driver;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Repository
{
    public class ProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(IMongoDatabase database)
        {
            _products = database.GetCollection<Product>("products");
        }

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _products.Find(_ => true).ToListAsync();

        public async Task<Product> GetByIdAsync(string id) =>
            await _products.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Product product) =>
            await _products.InsertOneAsync(product);

        public async Task<bool> UpdateAsync(string id, Product product)
        {
            var result = await _products.ReplaceOneAsync(p => p.Id == id, product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _products.DeleteOneAsync(p => p.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
