using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        List<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountryByOwner(int ownerid);
        List<Owner> GetOwnersFromACountry(int countyid);
        bool CountryExist(int id);
        bool CreateCountry (Country country);
        bool UpdateCountry (Country country);
    }
}
