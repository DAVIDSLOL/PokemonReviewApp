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
        public bool CategoryExists(int categoryId)
        {
            return _dataContext.Categories.Any(c => c.Id == categoryId);
        }

        public bool CreateCategory(CategoryEntity category)
        {
            _dataContext.Add(category);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }

        public bool DeleteCategory(CategoryEntity categoryid)
        {
            _dataContext.Remove(categoryid);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }

        public List<CategoryEntity> GetCategories()
        {
            return _dataContext.Categories.ToList();
        }

        public CategoryEntity GetCategory(int id)
        {
            return _dataContext.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public List<Pokemon> GetPokemonByCategory(int categoryid)
        {
            return _dataContext.PokemonCategories.Where(c => c.CategoryId == categoryid).
                Select(c => c.Pokemon).ToList();
        }

        public bool UpdateCategory(CategoryEntity category)
        {
            _dataContext.Update(category);

            var result = DbHelper.DbSaver(_dataContext);
            
            return result;
        }
    }
}
