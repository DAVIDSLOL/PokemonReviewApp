using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositoryes;
using System.ComponentModel;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository,
                                 IReviewRepository reviewRepository, 
                                 IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public async Task<IActionResult> GetPokemons()
        {
            var pokemonsEntities = await _pokemonRepository.GetListAsync();

            var pokemons = _mapper.Map<List<PokemonDto>>(pokemonsEntities);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpGet("pokeId")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemon([FromQuery] int pokeid)
        {
            var existedPokemon = await _pokemonRepository.PokemonExistsAsync(pokeid);

            if (!existedPokemon)
                return NotFound();

            var getPokemon = await _pokemonRepository.GetPokemonAsync(pokeid);

            var pokemon = _mapper.Map<PokemonDto>(getPokemon);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/categories")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonRaiting([FromRoute] int pokeId)

        {
            var existedPokemon = await _pokemonRepository.PokemonExistsAsync(pokeId);

            if (!existedPokemon)
                return NotFound();

            var raiting = await _pokemonRepository.GetPokemonRatingAsync(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(raiting);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePokemon([FromQuery] int ownerId,
                                                       [FromQuery] int catId, 
                                                       [FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemon = await _pokemonRepository.GetListAsync();

            var cratedPokemon = pokemon.Where(p => p.Name.Trim().ToUpper() == pokemonCreate.Name.
                                        TrimEnd().ToUpper()).FirstOrDefault();

            if (cratedPokemon != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonEntity = _mapper.Map<Pokemon>(pokemonCreate);

            var createPokemon = await _pokemonRepository.CreatePokemonAsync(ownerId, catId, pokemonEntity);

            if (!createPokemon)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Pokemon succesfully created");
        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePokemon(int pokeId,
                                           [FromQuery] int ownerId,
                                           [FromQuery] int catId,
                                           [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest(ModelState);

            if (pokeId != updatedPokemon.Id)
                return BadRequest(ModelState);

            var existedPokemon = await _pokemonRepository.PokemonExistsAsync(pokeId);

            if (!existedPokemon)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var pokemonEntity = _mapper.Map<Pokemon>(updatedPokemon);

            var updatePokemon = await _pokemonRepository.UpdatePokemonAsync(ownerId, catId, pokemonEntity);

            if (!updatePokemon)
            {
                ModelState.AddModelError("", "Something went wrong updating pokemon");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePokemon(int pokeId)
        {
            var existedPokemon = await _pokemonRepository.PokemonExistsAsync(pokeId);

            if (!existedPokemon)
                return NotFound();

            var reviewsToDelete = await _reviewRepository.GetReviewsOfAPokemonAsync(pokeId);

            var pokemonToDelete = await _pokemonRepository.GetPokemonAsync(pokeId);

            if (!ModelState.IsValid)
                return BadRequest();

            var deleteReciews = await _reviewRepository.DeleteReviewsAsync(reviewsToDelete.ToList());

            if (!deleteReciews)
            {
                ModelState.AddModelError("", "Something went wrong deleting reviews");
            }

            var deletePokemon = await _pokemonRepository.DeletePokemonAsync(pokemonToDelete);

            if (!deletePokemon)
            {
                ModelState.AddModelError("", "Something went wrong deleting pokemon");
            }

            return NoContent();
        }
    }
}
