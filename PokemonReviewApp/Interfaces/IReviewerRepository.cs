using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        Task <List<Reviewer>> GetReviewersAsync();
        Task <Reviewer> GetReviewerAsync (int reviewerId);
        Task <List<Review>> GetReviewsByReviewerAsync (int reviewerId);
        bool ReviewerExists (int reviewerId);
        bool CreateReviewer (Reviewer reviewer);
        bool UpdateReviewer (Reviewer reviewer);
        bool DeleteReviewer (Reviewer reviewer);
    }
}
