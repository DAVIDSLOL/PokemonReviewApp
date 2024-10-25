using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Helper;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositoryes
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ReviewerRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _dataContext.Add(reviewer);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _dataContext.Remove(reviewer);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }

        public async Task<Reviewer> GetReviewerAsync(int reviewerId)
        {
            var result = await _dataContext.Reviewers.Where(r => r.Id == reviewerId)
                                                     .Include(e => e.Reviews).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<Reviewer>> GetReviewersAsync()
        {
            var result = await _dataContext.Reviewers.ToListAsync();

            return result;
        }

        public async Task<List<Review>> GetReviewsByReviewerAsync(int reviewerId)
        {
            var result = await _dataContext.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToListAsync();

            return result;
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _dataContext.Reviewers.Any(r => r.Id == reviewerId);
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _dataContext.Update(reviewer);

            var result = DbHelper.DbSaver(_dataContext);

            return result; 
        }
    }
}
