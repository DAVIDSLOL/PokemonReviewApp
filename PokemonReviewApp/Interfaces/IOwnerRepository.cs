using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        Task <List<Owner>> GetOwnersAsync();
        Task <Owner> GetOwnerAsync(int ownerId);
        Task <List<Pokemon>> GetPokemonByOwnerAsync(int ownerId);
        Task<bool> OwnerExistAsync(int ownerId);
        Task<bool> CreateOwnerAsync (Owner owner);
        Task<bool> UpdateOwnerAsync (Owner owner);
        Task<bool> DeleteOwnerAsync (Owner owner);
    }
}
