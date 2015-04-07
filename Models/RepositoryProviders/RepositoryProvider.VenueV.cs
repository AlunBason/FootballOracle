using FootballOracle.Foundation.Interfaces;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Venues;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<VenueV> venueVRepository;
        private DbSet<VenueV> VenueVRepository
        {
            get { return venueVRepository = venueVRepository ?? context.Set<VenueV>(); }
        }

        private IQueryable<VenueV> VenueVs
        {
            get { return VenueVRepository as IQueryable<VenueV>; }
        }

        public void Add(VenueV venueV)
        {
            VenueVRepository.Add(venueV);
        }

        public void Remove(VenueV venueV)
        {
            VenueVRepository.Remove(venueV);
        }

        public async Task<VenueV> GetVenue(Guid venueKey, DateTime viewDate)
        {
            return (await Venues.SingleOrDefaultAsync(f => f.PrimaryKey == venueKey)).GetApprovedVersion(viewDate);
        }

        public async Task<VenueV> GetVenue(Guid primaryKey, Guid venueKey)
        {
            return await VenueVs.SingleOrDefaultAsync(s => s.PrimaryKey == primaryKey && s.HeaderKey == venueKey);
        }

        public async Task<IEnumerable<string>> GetVenueAutoCompleteList(Guid userId, bool isAdmin, string searchText)
        {
            var venues = await VenueVs.Where(w => w.VenueName.Contains(searchText.Trim()) && w.IsActive).ToListAsync();

            return venues.Select(s => s.VenueName).Distinct().OrderBy(o => o);
        }

        public async Task<IEnumerable<BaseVenueViewModel>> GetCountryVenueViewModels(Guid countryHeaderKey, DateTime viewDate)
        {
            var venues = await VenueVs.Where(w => w.CountryGuid == countryHeaderKey).OrderBy(v => v.VenueName).ToListAsync();

            return venues.ToViewModels(viewDate);
        }

        public async Task<IEnumerable<ICodePickerData>> GetVenueCodePickerData(DateTime viewDate)
        {
            var venueVs = await VenueVs.Where(e => e.IsActive && !e.IsMarkedForDeletion && e.EffectiveFrom <= viewDate && e.EffectiveTo >= viewDate).OrderBy(t => t.VenueName).ToListAsync();

            return venueVs.Select(s => new CodePickerViewModel()
            {
                Code = s.HeaderKey,
                Description = s.VenueName
            });
        }

        public async Task<IEnumerable<ICodePickerData>> GetCascadeVenueCodePickerData(DateTime viewDate, Guid countryKey)
        {
            var venueVs = await VenueVs.Where(e => e.CountryGuid == countryKey && e.IsActive && !e.IsMarkedForDeletion && e.EffectiveFrom <= viewDate && e.EffectiveTo >= viewDate).OrderBy(t => t.VenueName).ToListAsync();

            return venueVs.Select(s => new CodePickerViewModel()
            {
                Code = s.HeaderKey,
                Description = string.IsNullOrWhiteSpace(s.Address3) ? s.VenueName : string.Format("{0} ({1})", s.VenueName, s.Address3)
            });
        }

        public async Task<IEnumerable<ISearchResult>> SearchVenues(string searchText, DateTime viewDate)
        {
            var normalizedText = searchText.RemoveDiacritics();

            var groups = await VenueVs
                .Where(t => (t.VenueName.Contains(normalizedText.Trim()) || t.VenueName.Contains(searchText.Trim())) && t.IsActive)
                .GroupBy(t => t.HeaderKey).ToListAsync();

            var versions = new List<VenueV>();

            foreach (var group in groups)
                versions.Add(group.OrderByDescending(t => t.EffectiveFrom).First());

            return versions.ToViewModels(viewDate).Cast<ISearchResult>();
        }
    }
}
