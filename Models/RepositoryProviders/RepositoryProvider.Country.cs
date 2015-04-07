using FootballOracle.Models.Entities;
using System.Data.Entity;
using System.Linq;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<Country> countryRepository;
        private DbSet<Country> CountryRepository
        {
            get { return countryRepository = countryRepository ?? context.Set<Country>(); }
        }

        private IQueryable<Country> Countries
        {
            get { return CountryRepository as IQueryable<Country>; }
        }

        public void Add(Country country)
        {
            CountryRepository.Add(country);
        }

        public void Remove(Country country)
        {
            CountryRepository.Remove(country);
        }
    }
}
