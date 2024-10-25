using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        Task <List<Country>> GetCountriesAsync();
        Task <Country> GetCountryAsync(int id);
        Task <Country> GetCountryByOwnerAsync(int ownerid);
        Task <List<Owner>> GetOwnersFromACountryAsync(int countyid);
        Task<bool> CountryExistAsync(int id);
        Task<bool> CreateCountryAsync(Country country);
        Task<bool> UpdateCountryAsync(Country country);
        Task<bool> DeleteCountryAsync(Country country);
    }
}
