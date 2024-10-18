using AutoMapper;
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

        public bool CreateOwner(Owner owner)
        {
            _dataContext.Add(owner);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }

        public Owner GetOwner(int ownerId)
        {
            return _dataContext.Owners.Where(o => o.Id == ownerId).FirstOrDefault(); 
        }

        public List<Owner> GetOwners()
        {
            return _dataContext.Owners.ToList();
        }

        public List<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _dataContext.PokemonOwners.Where(o => o.Owner.Id == ownerId)
                .Select(p  => p.Pokemon).ToList(); 
        }
         
        public bool OwnerExist(int ownerId)
        {
            return _dataContext.Owners.Any(o => o.Id == ownerId);
        }

        public bool UpdateOwner(Owner owner)
        {
            _dataContext.Update(owner);

            var result = DbHelper.DbSaver(_dataContext);

            return result;
        }
    }
}
