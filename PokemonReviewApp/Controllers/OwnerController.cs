using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositoryes;

namespace PokemonReviewApp.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countyRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository,
                               ICountryRepository countyRepository,
                               IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countyRepository = countyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetOwners() 
        { 
            var getOwners = await (_ownerRepository.GetOwnersAsync());

            var owners = _mapper.Map<List<OwnerDto>>(getOwners);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        public async Task<IActionResult> GetOwner(int ownerId)
        {
            var existedOwner = await _ownerRepository.OwnerExistAsync(ownerId);

            if (!existedOwner)
                return NotFound();

            var getOwner = await (_ownerRepository.GetOwnerAsync(ownerId));

            var owner = _mapper.Map<OwnerDto>(getOwner);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpGet("{ownerId}/pokemon")]
        public async Task<IActionResult> GetPokemonByOwner(int ownerId)
        {
            var existedOwner = await _ownerRepository.OwnerExistAsync(ownerId);

            if (!existedOwner)
                return NotFound();

            var getPokemonByOwner = await (_ownerRepository.GetPokemonByOwnerAsync(ownerId));

            var owner = _mapper.Map<List<PokemonDto>>(getPokemonByOwner);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateOwner([FromQuery] int countryId, 
                                                     [FromBody] OwnerDto ownerCreate)

        {
            if (ownerCreate == null)
                return BadRequest(ModelState);

            var getOwners = await _ownerRepository.GetOwnersAsync();

            var owners = getOwners.Where(o => o.LastName.Trim().ToUpper() == ownerCreate.LastName.TrimEnd()
                                  .ToUpper()).FirstOrDefault();

            if (owners != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(ownerCreate);

            ownerMap.Country = await _countyRepository.GetCountryAsync(countryId);

            var createdOwner = await _ownerRepository.CreateOwnerAsync(ownerMap);

            if (!createdOwner)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Owner succesfully created");
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateOwner(int ownerId, [FromBody] OwnerDto updatedOwner)
        {
            if (updatedOwner == null)
                return BadRequest(ModelState);

            if (ownerId != updatedOwner.Id)
                return BadRequest(ModelState);

            var existedOwner = await _ownerRepository.OwnerExistAsync(ownerId);

            if (!existedOwner)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var ownerEntity = _mapper.Map<Owner>(updatedOwner);

            var ownerUpdate = await _ownerRepository.UpdateOwnerAsync(ownerEntity);

            if (!ownerUpdate)
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCountry(int ownerId)
        {
            var existedOwner = await _ownerRepository.OwnerExistAsync(ownerId);

            if (!existedOwner)
                return NotFound();

            var ownerToDelete = await _ownerRepository.GetOwnerAsync(ownerId);

            if (!ModelState.IsValid)
                return BadRequest();

            var deleteOwner = await _ownerRepository.DeleteOwnerAsync(ownerToDelete);

            if (!deleteOwner)
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }
    }
}
