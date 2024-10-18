using PokemonReviewApp.Data;

namespace PokemonReviewApp.Helper
{
    public static class DbHelper
    {
        public static bool DbSaver(DataContext dataContext)
        {
            var saved = dataContext.SaveChanges();

            return saved > 0 ? true : false;
        }
    }
}
