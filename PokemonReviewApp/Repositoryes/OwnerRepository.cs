using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Helper;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositoryes
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _dataContext;

        public OwnerRepository(DataContext dataContext)
        {
           _dataContext = dataContext;
        }

        public async Task<bool> CreateOwnerAsync(Owner owner)
        {
            await _dataContext.AddAsync(owner);

            var result = await DbHelper.DbSaver(_dataContext);

            return result;
        }

        public async Task<bool> DeleteOwnerAsync(Owner owner)
        {
            _dataContext.Remove(owner);

            var result = await DbHelper.DbSaver(_dataContext);

            return result;
        }

        public async Task<Owner> GetOwnerAsync(int ownerId)
        {
            var result = await _dataContext.Owners.Where(o => o.Id == ownerId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<Owner>> GetOwnersAsync()
        {
            var result = await _dataContext.Owners.ToListAsync();

            return result;
        }

        public async Task<List<Pokemon>> GetPokemonByOwnerAsync(int ownerId)
        {
            var result = await _dataContext.PokemonOwners.Where(o => o.Owner.Id == ownerId)
                                                         .Select(p => p.Pokemon).ToListAsync();

            return result;
        }
         
        public async Task<bool> OwnerExistAsync(int ownerId)
        {
            return await _dataContext.Owners.AnyAsync(o => o.Id == ownerId);
        }

        public async Task<bool> UpdateOwnerAsync(Owner owner)
        {
            _dataContext.Update(owner);

            var result = await DbHelper.DbSaver(_dataContext);

            return result;
        }
    }
}
