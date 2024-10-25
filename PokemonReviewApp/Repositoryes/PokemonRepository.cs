using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Helper;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositoryes
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _dataContext;

        public PokemonRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _dataContext.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
            var category = _dataContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon,
            };

            _dataContext.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon
            };

            _dataContext.Add(pokemonCategory);

            _dataContext.Add(pokemon);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _dataContext.Remove(pokemon);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }

        public async Task<List<Pokemon>> GetListAsync()
        {
            var result = await _dataContext.Pokemons.OrderBy(p => p.Id).ToListAsync();

            return result;
        }

        public async Task<Pokemon> GetPokemonAsync(int id)
        {
            var result = await _dataContext.Pokemons.Where(p => p.Id == id).FirstOrDefaultAsync();

            return result;
        }

        public async Task<Pokemon> GetPokemonByNameAsync(string name)
        {
            var result = await _dataContext.Pokemons.Where(p => p.Name == name).FirstOrDefaultAsync();

            return result;
        } 

        public decimal GetPokemonRating(int pokeid)
        {
            var review = _dataContext.Reviews.Where(p => p.Pokemon.Id == pokeid);

            if (review.Count() <= 0) 
                return 0;

            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public bool PokemonExists(int pokeid)
        {
          return _dataContext.Pokemons.Any(p => p.Id == pokeid);
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _dataContext.Update(pokemon);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }

        ////public bool Save()
        ////{
        ////    var saved = _dataContext.SaveChanges();
        ////    return saved > 0 ? true : false;
        ////} 
    }
}
