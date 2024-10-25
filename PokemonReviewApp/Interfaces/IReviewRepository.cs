using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        Task <List<Review>> GetReviewsAsync();
        Task <Review> GetReviewAsync(int reviewid);
        Task <List<Review>> GetReviewsOfAPokemonAsync(int pokeId);
        bool ReviewExists (int reviewid);
        bool CreateReview (Review review);
        bool UpdateReview (Review review);
        bool DeleteReview (Review review);
        bool DeleteReviews (List<Review> reviews);
    }
}
