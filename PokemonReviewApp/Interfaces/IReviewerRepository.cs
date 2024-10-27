using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        Task<List<Reviewer>> GetReviewersAsync();
        Task<Reviewer> GetReviewerAsync (int reviewerId);
        Task<List<Review>> GetReviewsByReviewerAsync (int reviewerId);
        Task<bool> ReviewerExistsAsync(int reviewerId);
        Task<bool> CreateReviewerAsync(Reviewer reviewer);
        Task<bool> UpdateReviewerAsync(Reviewer reviewer);
        Task<bool> DeleteReviewerAsync(Reviewer reviewer);
    }
}
