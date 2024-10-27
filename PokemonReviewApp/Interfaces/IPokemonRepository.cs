using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        Task<List<Pokemon>> GetListAsync();
        Task<Pokemon> GetPokemonAsync(int id);
        Task<Pokemon> GetPokemonByNameAsync(string name);
        Task<decimal> GetPokemonRatingAsync(int pokeid);
        Task<bool> PokemonExistsAsync(int pokeid);
        Task<bool> CreatePokemonAsync(int ownerId, int categoryId, Pokemon pokemon);
        Task<bool> UpdatePokemonAsync(int ownerId, int categoryId, Pokemon pokemon);
        Task<bool> DeletePokemonAsync(Pokemon pokemon); 

    }
}
