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

        public async Task<bool> CountryExistAsync(int id)
        {
            return await _dataContext.Countries.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateCountryAsync(Country country)
        {
            await _dataContext.AddAsync(country);

            var result = await DbHelper.DbSaver(_dataContext);

            return result;
        }

        public async Task<bool> UpdateCountryAsync(Country country)
        {
            _dataContext.Update(country);

            var result = await DbHelper.DbSaver(_dataContext);

            return result;
        }

        public async Task<bool> DeleteCountryAsync(Country country)
        {
            _dataContext.Remove(country);

            var result = await DbHelper.DbSaver (_dataContext);

            return result;
        }
    }
}
