using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Helper;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositoryes
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _dataContext;

        public ReviewRepository(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }

        public async Task<List<Review>> GetReviewsAsync()
        {
            var result = await _dataContext.Reviews.ToListAsync();

            return result;
        }

        public async Task<Review> GetReviewAsync(int reviewid) 
        {
            var result = await _dataContext.Reviews.Where(r => r.Id == reviewid).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<Review>> GetReviewsOfAPokemonAsync (int pokeId)
        {
            var result = await _dataContext.Reviews.Where(r => r.Pokemon.Id == pokeId).ToListAsync();

            return result;
        }

        public bool ReviewExists(int reviewid)
        {
            return _dataContext.Reviews.Any(r => r.Id == reviewid);
        }

        public bool CreateReview(Review review)
        {
            _dataContext.Add(review);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }

        public bool UpdateReview(Review review)
        {
            _dataContext.Update(review);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }

        public bool DeleteReview(Review review)
        {
            _dataContext.Remove(review);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _dataContext.RemoveRange(reviews);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }
    }
}
