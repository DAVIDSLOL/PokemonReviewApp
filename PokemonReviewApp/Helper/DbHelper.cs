using PokemonReviewApp.Data;

namespace PokemonReviewApp.Helper
{
    public static class DbHelper
    {
        public static async Task<bool> DbSaver(DataContext dataContext)
        {
            var saved = await dataContext.SaveChangesAsync();

            return saved > 0 ? true : false;
        }
    }
}
