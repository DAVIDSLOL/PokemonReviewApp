using AutoMapper;
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

        public List<Review> GetReviews()
        {
            return _dataContext.Reviews.ToList();
        }

        public Review GetReview(int reviewid) 
        {
            return _dataContext.Reviews.Where(r => r.Id == reviewid)?.FirstOrDefault();
        }

        public List<Review> GetReviewsOfAPokemon (int pokeId)
        {
            return _dataContext.Reviews.Where(r => r.Pokemon.Id == pokeId).ToList();
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
    }
}
