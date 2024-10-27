using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositoryes;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviewers()
        {
            var getReviews = await _reviewerRepository.GetReviewersAsync();

            var reviewers = _mapper.Map<List<ReviewerDto>>(getReviews);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        public async Task<IActionResult> GetReviewer(int reviewerId)
        {
            var existedReviewer = await _reviewerRepository.ReviewerExistsAsync(reviewerId);

            if (!existedReviewer)
                return NotFound();

            var getReviewer = await _reviewerRepository.GetReviewerAsync(reviewerId);

            var reviewer = _mapper.Map<ReviewerDto>(getReviewer);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        public async Task<IActionResult> GetReviewsByAReviewer(int reviewerId)
        {
            var existedReviewer = await _reviewerRepository.ReviewerExistsAsync(reviewerId);

            if (!existedReviewer)
                return NotFound();

            var getReviews = await _reviewerRepository.GetReviewsByReviewerAsync(reviewerId);

            var reviews = _mapper.Map<List<ReviewDto>>(getReviews);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReviewer([FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);

            var getReviewers = await _reviewerRepository.GetReviewersAsync();

            var reviewer = getReviewers.Where(r => r.LastName.Trim().ToUpper() == reviewerCreate.LastName.
                                        TrimEnd().ToUpper()).FirstOrDefault();

            if (reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

            var createReviewer = await _reviewerRepository.CreateReviewerAsync(reviewerMap);

            if (!createReviewer)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Reviewer succesfully created");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updatedReviewer)
        {
            if (updatedReviewer == null)
                return BadRequest(ModelState);

            if (reviewerId != updatedReviewer.Id)
                return BadRequest(ModelState);

            var existedReviewer = await _reviewerRepository.ReviewerExistsAsync(reviewerId);

            if (!existedReviewer)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewerEntity = _mapper.Map<Reviewer>(updatedReviewer);

            var updateReviewer = await _reviewerRepository.UpdateReviewerAsync(reviewerEntity);

            if (!updateReviewer)
            {
                ModelState.AddModelError("", "Something went wrong updating reviewer");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReviewer(int reviewerId)
        {
            var existedReviewer = await _reviewerRepository.ReviewerExistsAsync(reviewerId);

            if (!existedReviewer)
                return NotFound();

            var ownerToDelete = await _reviewerRepository.GetReviewerAsync(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest();

            var deleteReviewer = await _reviewerRepository.DeleteReviewerAsync(ownerToDelete);

            if (!deleteReviewer)
            {
                ModelState.AddModelError("", "Something went wrong deleting Reviewer");
            }

            return NoContent();
        }
    }
}
