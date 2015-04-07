using FootballOracle.Foundation.Interfaces;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Organisations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<OrganisationV> organisationVRepository;
        private DbSet<OrganisationV> OrganisationVRepository
        {
            get { return organisationVRepository = organisationVRepository ?? context.Set<OrganisationV>(); }
        }

        private IQueryable<OrganisationV> OrganisationVs
        {
            get { return OrganisationVRepository as IQueryable<OrganisationV>; }
        }

        public void Add(OrganisationV organisationV)
        {
            OrganisationVRepository.Add(organisationV);
        }

        public void Remove(OrganisationV organisationV)
        {
            OrganisationVRepository.Remove(organisationV);
        }

        public async Task<OrganisationV> GetOrganisation(Guid organisationKey, DateTime viewDate)
        {
            return (await Organisations.SingleOrDefaultAsync(f => f.PrimaryKey == organisationKey)).GetApprovedVersion(viewDate);
        }

        public async Task<OrganisationV> GetOrganisation(Guid primaryKey, Guid organisationKey)
        {
            return await OrganisationVs.SingleOrDefaultAsync(s => s.PrimaryKey == primaryKey && s.HeaderKey == organisationKey);
        }

        public async Task<IEnumerable<string>> GetOrganisationAutoCompleteList(Guid userId, bool isAdmin, string searchText)
        {
            return await OrganisationVs
                .Where(w => w.OrganisationName.Contains(searchText.Trim()) && w.IsActive)
                .Select(s => s.OrganisationName)
                .Distinct()
                .OrderBy(o => o)
                .ToListAsync();
        }

        public async Task<IEnumerable<BaseOrganisationViewModel>> GetOrganisationsByCountry(Guid countryHeaderKey, DateTime viewDate)
        {
            var organisations = await OrganisationVs.Where(w => w.CountryGuid == countryHeaderKey).ToListAsync();

            return organisations.ToViewModels(viewDate);
        }

        public async Task<IEnumerable<BaseOrganisationViewModel>> GetChildOrganisations(Guid organisationHeaderKey, DateTime viewDate)
        {
            var organisations = await OrganisationVs.Where(w => w.ParentOrganisationGuid == organisationHeaderKey).ToListAsync();

            return organisations.ToViewModels(viewDate);
        }

        public async Task<IEnumerable<ICodePickerData>> GetOrganisationCodePickerData(DateTime viewDate)
        {
            var query = OrganisationVs
                .Where(e => e.IsActive && !e.IsMarkedForDeletion && e.EffectiveFrom <= viewDate && e.EffectiveTo >= viewDate)
                .OrderBy(t => t.OrganisationDescription);
            
            var organisationVs = await query.ToListAsync();

            return organisationVs.Select(s => new CodePickerViewModel() { Code = s.HeaderKey, Description = s.OrganisationName });
        }

        public async Task<IEnumerable<ISearchResult>> SearchOrganisations(string searchText, DateTime viewDate)
        {
            var normalizedText = searchText.RemoveDiacritics();

            var groups = await OrganisationVs
                .Where(t => (t.OrganisationName.Contains(normalizedText.Trim()) || t.OrganisationName.Contains(searchText.Trim())) && t.IsActive)
                .GroupBy(t => t.HeaderKey).ToListAsync();

            var versions = new List<OrganisationV>();

            foreach (var group in groups)
                versions.Add(group.OrderByDescending(t => t.EffectiveFrom).First());

            return versions.ToViewModels(viewDate).Cast<ISearchResult>();
        }
    }
}
