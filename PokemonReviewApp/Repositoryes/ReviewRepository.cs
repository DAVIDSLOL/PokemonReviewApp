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

        public async Task<bool> ReviewExistsAsync(int reviewid)
        {
            var result = await _dataContext.Reviews.AnyAsync(r => r.Id == reviewid);

            return result;
        }

        public async Task<bool> CreateReviewAsync(Review review)
        {
            await _dataContext.AddAsync(review);

            var result = await DbHelper.DbSaver(_dataContext);

            return result;
        }

        public async Task<bool> UpdateReviewAsync(Review review)
        {
            _dataContext.Update(review);

            var result = await DbHelper.DbSaver(_dataContext);

            return result;
        }

        public async Task<bool> DeleteReviewAsync(Review review)
        {
            _dataContext.Remove(review);

            var result = await DbHelper.DbSaver(_dataContext);

            return result;
        }

        public async Task<bool> DeleteReviewsAsync(List<Review> reviews)
        {
            _dataContext.RemoveRange(reviews);

            var result = await DbHelper.DbSaver(_dataContext);

            return result;
        }
    }
}
