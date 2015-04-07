using FootballOracle.Models.Entities;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FootballOracle.Foundation;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<LookupVenue> lookupVenueRepository;
        private DbSet<LookupVenue> LookupVenueRepository
        {
            get { return lookupVenueRepository = lookupVenueRepository ?? context.Set<LookupVenue>(); }
        }

        private IQueryable<LookupVenue> LookupVenues
        {
            get { return LookupVenueRepository as IQueryable<LookupVenue>; }
        }

        public void Add(LookupVenue lookupVenue)
        {
            LookupVenueRepository.Add(lookupVenue);
        }

        public void Remove(LookupVenue lookupVenue)
        {
            LookupVenueRepository.Remove(lookupVenue);
        }

        public async Task<LookupVenue> GetLookupVenue(ImportSite importSite, string lookupId)
        {
            return await LookupVenues.FirstOrDefaultAsync(f => f.ImportSite == importSite && f.LookupId == lookupId);
        }
    }
}
