using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        List<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        List<Pokemon> GetPokemonByOwner(int ownerId);
        bool OwnerExist(int ownerId);
        bool CreateOwner (Owner owner);
        bool UpdateOwner (Owner owner);
        bool DeleteOwner (Owner owner);
    }
}
