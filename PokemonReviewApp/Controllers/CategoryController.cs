using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories() 
        {
            var getCategories = await (_categoryRepository.GetCategoriesAsync());

            var categories = _mapper.Map<List<CategoryDto>>(getCategories); 

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("categoryId")]
        public async Task<IActionResult> GetCategory([FromQuery] int categoryId) 
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var getCategory = await (_categoryRepository.GetCategoryAsync(categoryId));

            var category = _mapper.Map<CategoryDto>(getCategory);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        [HttpGet("pokemon/{categoryId}")]
        public async Task<IActionResult> GetPokemonByCategoryId([FromQuery] int categoryId) 
        {
            var pokemon = await (_categoryRepository.GetPokemonByCategoryAsync(categoryId));

            var pokemonByCategory = _mapper.Map<List<PokemonDto>>(pokemon);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemonByCategory);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
                return BadRequest(ModelState);

            var categories = await _categoryRepository.GetCategoriesAsync();

            var existedCategory = categories.Where(c => c.Name.Trim()
                                            .ToUpper() == categoryCreate.Name.TrimEnd()
                                            .ToUpper()).FirstOrDefault();

            if (existedCategory != null)
            {
                ModelState.AddModelError("", "Category already exists");
                    return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<CategoryEntity>(categoryCreate);

            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            
            return Ok("Category successully created");
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody]CategoryDto updatedCategory)
        {
            if (updatedCategory == null) 
                return BadRequest(ModelState);

            if (categoryId != updatedCategory.Id) 
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var categoryEntity = _mapper.Map<CategoryEntity>(updatedCategory);

            if (!_categoryRepository.UpdateCategory(categoryEntity))
            {
                ModelState.AddModelError("", "Something went wrong utdating category");
                    return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var catergoryToDelete = await _categoryRepository.GetCategoryAsync(categoryId);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_categoryRepository.DeleteCategory(catergoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return NoContent();
        }
    }
}
