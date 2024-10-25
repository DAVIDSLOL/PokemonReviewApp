using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        Task <List<CategoryEntity>> GetCategoriesAsync();
        Task <CategoryEntity> GetCategoryAsync(int id);
        Task <List<Pokemon>> GetPokemonByCategoryAsync(int categoryid);
        Task <bool> CategoryExistsAsync(int categoryId);
        Task <bool> CreateCategoryAsync(CategoryEntity category);
        Task <bool> UpdateCategoryAsync(CategoryEntity category);
        Task <bool> DeleteCategoryAsync(CategoryEntity categoryid);
    }
}
