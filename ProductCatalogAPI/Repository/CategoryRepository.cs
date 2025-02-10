using MongoDB.Driver;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Repository
{
    public class CategoryRepository
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryRepository(IMongoDatabase database)
        {
            _categories = database.GetCollection<Category>("categories");
        }

        public async Task<IEnumerable<Category>> GetAllAsync() =>
            await _categories.Find(_ => true).ToListAsync();

        public async Task<Category> GetByIdAsync(string id) =>
            await _categories.Find(c => c.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Category category) =>
            await _categories.InsertOneAsync(category);

        public async Task<bool> UpdateAsync(string id, Category category)
        {
            var result = await _categories.ReplaceOneAsync(c => c.Id == id, category);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _categories.DeleteOneAsync(c => c.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
