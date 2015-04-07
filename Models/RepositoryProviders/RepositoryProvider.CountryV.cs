using FootballOracle.Foundation.Interfaces;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<CountryV> countryVRepository;
        private DbSet<CountryV> CountryVRepository
        {
            get { return countryVRepository = countryVRepository ?? context.Set<CountryV>(); }
        }

        private IQueryable<CountryV> CountryVs
        {
            get { return CountryVRepository as IQueryable<CountryV>; }
        }

        public void Add(CountryV countryV)
        {
            CountryVRepository.Add(countryV);
        }

        public void Remove(CountryV countryV)
        {
            CountryVRepository.Remove(countryV);
        }

        public async Task<CountryV> GetCountry(Guid countryKey, DateTime viewDate)
        {
            return (await Countries.SingleOrDefaultAsync(f => f.PrimaryKey == countryKey)).GetApprovedVersion(viewDate);
        }

        public async Task<CountryV> GetCountry(Guid primaryKey, Guid countryKey)
        {
            return await CountryVs.SingleOrDefaultAsync(s => s.PrimaryKey == primaryKey && s.HeaderKey == countryKey);
        }

        public async Task<IEnumerable<string>> GetCountryAutoCompleteListAsync(Guid userId, bool isAdmin, string searchText)
        {
            return await CountryVs
                .Where(w => w.CountryName.Contains(searchText.Trim()) && w.IsActive)
                .Select(s => s.CountryName)
                .Distinct()
                .OrderBy(o => o)
                .ToListAsync();
        }

        public async Task<IEnumerable<BaseCountryViewModel>> GetCountriesByOrganisationAsync(Guid organisationHeaderKey, DateTime viewDate)
        {
            var countryVs = await CountryVs.Where(c => c.OrganisationGuid == organisationHeaderKey).ToListAsync();

            return countryVs.ToViewModels(viewDate);
        }

        public async Task<IEnumerable<ICodePickerData>> GetCountryCodePickerData(DateTime viewDate)
        {
            var countryVs = await CountryVs.Where(e => e.IsActive && !e.IsMarkedForDeletion && e.EffectiveFrom <= viewDate && e.EffectiveTo >= viewDate).OrderBy(t => t.CountryName).ToListAsync();

            return countryVs.Select(s => new CodePickerViewModel()
            {
                Code = s.HeaderKey,
                Description = s.CountryName
            });
        }

        public async Task<IEnumerable<ISearchResult>> SearchCountries(string searchText, DateTime viewDate)
        {
            var normalizedText = searchText.RemoveDiacritics();

            var groups = await CountryVs
                .Where(t => (t.CountryName.Contains(normalizedText.Trim()) || t.CountryName.Contains(searchText.Trim())) && t.IsActive)
                .GroupBy(t => t.HeaderKey).ToListAsync();

            var versions = new List<CountryV>();

            foreach (var group in groups)
                versions.Add(group.OrderByDescending(t => t.EffectiveFrom).First());

            return versions.ToViewModels(viewDate).Cast<ISearchResult>();
        }
    }
}
