using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<CategoryEntity>> GetCategoriesAsync();
        Task<CategoryEntity> GetCategoryAsync(int id);
        Task<List<Pokemon>> GetPokemonByCategoryAsync(int categoryid);
        bool CategoryExists(int categoryId);
        bool CreateCategory (CategoryEntity category);
        bool UpdateCategory (CategoryEntity category);
        bool DeleteCategory (CategoryEntity categoryid);
    }
}
