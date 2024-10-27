using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewsAsync();
        Task<Review> GetReviewAsync(int reviewid);
        Task<List<Review>> GetReviewsOfAPokemonAsync(int pokeId);
        Task<bool> ReviewExistsAsync(int reviewid);
        Task<bool> CreateReviewAsync(Review review);
        Task<bool> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(Review review);
        Task<bool> DeleteReviewsAsync(List<Review> reviews);
    }
}
