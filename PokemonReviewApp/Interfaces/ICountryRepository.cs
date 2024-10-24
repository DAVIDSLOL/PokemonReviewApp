using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetCountriesAsync();
        Task<Country> GetCountryAsync(int id);
        Task<Country> GetCountryByOwnerAsync(int ownerid);
        Task<List<Owner>> GetOwnersFromACountryAsync(int countyid);
        bool CountryExist(int id);
        bool CreateCountry (Country country);
        bool UpdateCountry (Country country);
        bool DeleteCountry (Country country);
    }
}
