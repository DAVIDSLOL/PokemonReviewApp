using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Helper;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositoryes
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _dataContext;

        public CountryRepository(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }


        public async Task<List<Country>> GetCountriesAsync()
        {
            var result = await _dataContext.Countries.ToListAsync();

            return result;
        }

        public async Task<Country> GetCountryAsync(int id)
        {
            var result = await _dataContext.Countries.Where(c => c.Id == id).FirstOrDefaultAsync();

            return result;
        }

        public async Task<Country> GetCountryByOwnerAsync(int ownerId)
        {
            var result = await _dataContext.Owners.Where(o => o.Id == ownerId).
                Select(c => c.Country).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<Owner>> GetOwnersFromACountryAsync(int countyid)
        {
            var result = await _dataContext.Owners.Where(c => c.Country.Id == countyid).ToListAsync();

            return result;
        }

        public bool CountryExist(int id)
        {
            return _dataContext.Countries.Any(c => c.Id == id);
        }

        public bool CreateCountry(Country country)
        {
            _dataContext.Add(country);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }

        public bool UpdateCountry(Country country)
        {
            _dataContext.Update(country);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }

        public bool DeleteCountry(Country country)
        {
            _dataContext.Remove(country);

            var result = DbHelper.DbSaver (_dataContext);

            return result;
        }
    }
}
