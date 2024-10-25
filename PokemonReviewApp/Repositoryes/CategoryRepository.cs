using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Helper;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositoryes
{
    public class CategoryRepository : ICategoryRepository

    {
        private readonly DataContext _dataContext;

        public CategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            var result = await _dataContext.Categories.AnyAsync(c => c.Id == categoryId);

            return result;
        }

        public async Task<bool> CreateCategoryAsync(CategoryEntity category)
        {
            await _dataContext.AddAsync(category);

            var result = await DbHelper.DbSaver(_dataContext);

            return result;
        }

        public async Task<bool> DeleteCategoryAsync(CategoryEntity categoryid)
        {
            _dataContext.Remove(categoryid);

            var result = await DbHelper.DbSaver(_dataContext);

            return result;
        }

        public async Task<List<CategoryEntity>> GetCategoriesAsync()
        {
            var result = await _dataContext.Categories.ToListAsync();

            return result;
        }

        public async Task<CategoryEntity> GetCategoryAsync(int id)
        {
            var result = await _dataContext.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<Pokemon>> GetPokemonByCategoryAsync(int categoryid)
        {
            var result = await _dataContext.PokemonCategories.Where(c => c.CategoryId == categoryid).
                Select(c => c.Pokemon).ToListAsync();

            return result;
        }

        public async Task<bool> UpdateCategoryAsync(CategoryEntity category)
        {
            _dataContext.Update(category);

            var result = await DbHelper.DbSaver(_dataContext);
            
            return result;
        }
    }
}
