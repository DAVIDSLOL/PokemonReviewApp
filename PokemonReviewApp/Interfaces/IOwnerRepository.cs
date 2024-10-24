using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        Task <List<Owner>> GetOwnersAsync();
        Task<Owner> GetOwnerAsync(int ownerId);
        Task<List<Pokemon>> GetPokemonByOwnerAsync(int ownerId);
        bool OwnerExist(int ownerId);
        bool CreateOwner (Owner owner);
        bool UpdateOwner (Owner owner);
        bool DeleteOwner (Owner owner);
    }
}
