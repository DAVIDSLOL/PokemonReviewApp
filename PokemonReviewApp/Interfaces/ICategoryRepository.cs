using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        List<CategoryEntity> GetCategories();
        CategoryEntity GetCategory(int id);
        List<Pokemon> GetPokemonByCategory(int categoryid);
        bool CategoryExists(int categoryId);
        bool CreateCategory (CategoryEntity category);
        bool UpdateCategory (CategoryEntity category);
    }
}
