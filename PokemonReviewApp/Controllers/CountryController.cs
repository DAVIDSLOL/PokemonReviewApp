using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositoryes;
using System.Diagnostics.Metrics;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countyRepository, IMapper mapper)
        {
            _countryRepository = countyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var getCountries = await (_countryRepository.GetCountriesAsync());

            var countries = _mapper.Map<List<CountryDto>>(getCountries);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        public async Task<IActionResult> GetCountry( int countryId)
        {
            if (!_countryRepository.CountryExist(countryId))
                return NotFound();

            var getCountry = await (_countryRepository.GetCountryAsync(countryId));

            var country = _mapper.Map<CountryDto>(getCountry);

            if (!ModelState.IsValid)
                return BadRequest(ModelState); 

            return Ok(country);
        }

        [HttpGet("/owners/{ownerId}")]
        public async Task<IActionResult> GetCountryByOwner(int ownerId)
        {
            var getCountryByOwner = await (_countryRepository.GetCountryByOwnerAsync(ownerId));

            var countryByOwner = _mapper.Map<CountryDto>(getCountryByOwner);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countryByOwner);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCounty([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
                return BadRequest(ModelState);

            var getCountry = await _countryRepository.GetCountriesAsync();

            var country = getCountry.Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.
                                     TrimEnd().ToUpper()).FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                    return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Country succesfully created");
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int countryId, [FromBody] CountryDto updatedCountry) 
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);

            if (countryId != updatedCountry.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExist(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var countryEntity = _mapper.Map<Country>(updatedCountry);

            if (!_countryRepository.UpdateCountry(countryEntity))
            {
                ModelState.AddModelError("", "Something went wrong updating country");
                    return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCountry(int countryId)
        {
            if (!_countryRepository.CountryExist(countryId))
                return NotFound();

            var countryToDelete = await _countryRepository.GetCountryAsync(countryId);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_countryRepository.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting country");
            }

            return NoContent();
        }
    }
}
