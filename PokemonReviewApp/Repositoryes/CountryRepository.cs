using AutoMapper;
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


        public List<Country> GetCountries()
        {
            return _dataContext.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return _dataContext.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _dataContext.Owners.Where(o => o.Id == ownerId).
                Select(c => c.Country).FirstOrDefault();
        }

        public List<Owner> GetOwnersFromACountry(int countyid)
        {
            return _dataContext.Owners.Where(c => c.Country.Id == countyid).ToList();
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
