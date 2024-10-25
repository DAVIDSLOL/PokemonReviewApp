using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        Task <List<Pokemon>> GetListAsync();
        Task <Pokemon> GetPokemonAsync(int id);
        Task <Pokemon> GetPokemonByNameAsync(string name);
        decimal GetPokemonRating(int pokeid);
        bool PokemonExists (int pokeid);
        bool CreatePokemon (int ownerId, int categoryId, Pokemon pokemon);
        bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon); 

    }
}
