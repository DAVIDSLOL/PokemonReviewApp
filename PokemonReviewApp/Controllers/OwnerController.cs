﻿using AutoMapper;
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
            if (!_ownerRepository.OwnerExist(ownerId))
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
            if (!_ownerRepository.OwnerExist(ownerId))
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

            if (!_ownerRepository.CreateOwner(ownerMap))
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
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto updatedOwner)
        {
            if (updatedOwner == null)
                return BadRequest(ModelState);

            if (ownerId != updatedOwner.Id)
                return BadRequest(ModelState);

            if (!_ownerRepository.OwnerExist(ownerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var ownerEntity = _mapper.Map<Owner>(updatedOwner);

            if (!_ownerRepository.UpdateOwner(ownerEntity))
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
            if (!_ownerRepository.OwnerExist(ownerId))
                return NotFound();

            var ownerToDelete = await _ownerRepository.GetOwnerAsync(ownerId);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_ownerRepository.DeleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }
    }
}
