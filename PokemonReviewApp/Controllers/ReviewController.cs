﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositoryes;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository,
            IReviewerRepository reviewerRepository,
            IPokemonRepository pokemonRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpGet("reviewId")]
        public IActionResult GetReview([FromQuery] int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

        [HttpGet("pokemon/{pokeId}")]
        public IActionResult GetReviewsForAPokemon (int pokeId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>
                (_reviewRepository.GetReviewsOfAPokemon(pokeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId,
            [FromQuery] int pokemonId,
            [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest(ModelState);

            var reviews = _reviewRepository.GetReviews()
                .Where(r => r.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd()
                .ToUpper()).FirstOrDefault();

            if (reviews != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewCreate);

            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonId);

            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Review succesfully created");
        }


        [HttpPut("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest(ModelState);

            if (reviewId != updatedReview.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewEntity = _mapper.Map<Review>(updatedReview);

            if (!_reviewRepository.UpdateReview(reviewEntity))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var reviewToDelete = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting review");
            }

            return NoContent();
        }

    }
}
