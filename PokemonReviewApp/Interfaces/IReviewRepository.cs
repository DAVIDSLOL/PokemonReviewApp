﻿using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        List<Review> GetReviews();
        Review GetReview(int reviewid);
        List<Review> GetReviewsOfAPokemon(int pokeId);
        bool ReviewExists (int reviewid);
        bool CreateReview (Review review);
        bool UpdateReview (Review review);
        bool DeleteReview (Review review);
        bool DeleteReviews (List<Review> reviews);
    }
}
