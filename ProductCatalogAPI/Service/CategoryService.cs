using ProductCatalogAPI.Models;
using ProductCatalogAPI.Repository;

namespace ProductCatalogAPI.Service
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ProductRepository _productRepository;

        public CategoryService(CategoryRepository categoryRepository, ProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public Task<IEnumerable<Category>> GetAllCategoriesAsync() => _categoryRepository.GetAllAsync();
        public Task<Category> GetCategoryByIdAsync(string id) => _categoryRepository.GetByIdAsync(id);

        public async Task<bool> AddCategoryAsync(Category category)
        {
            var existingCategory = (await _categoryRepository.GetAllAsync())
                                   .FirstOrDefault(c => c.Name.ToLower() == category.Name.ToLower());

            if (existingCategory != null)
                return false;

            await _categoryRepository.AddAsync(category);
            return true;
        }

        public async Task<bool> UpdateCategoryAsync(string id, Category category)
        {
            return await _categoryRepository.UpdateAsync(id, category);
        }

        public async Task<bool> DeleteCategoryAsync(string id)
        {
            var productsInCategory = (await _productRepository.GetAllAsync()).Any(p => p.CategoryId == id);

            if (productsInCategory)
                return false;

            return await _categoryRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<object>> GetCategoriesWithProductCountAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();

            return categories.Select(category => new
            {
                category.Id,
                category.Name,
                ProductCount = products.Count(p => p.CategoryId == category.Id)
            });
        }
    }
}
