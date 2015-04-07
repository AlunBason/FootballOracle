using System.Data.Entity;
using System.Linq;
using FootballOracle.Models.Entities;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<Venue> venueRepository;
        private DbSet<Venue> VenueRepository
        {
            get { return venueRepository = venueRepository ?? context.Set<Venue>(); }
        }

        private IQueryable<Venue> Venues
        {
            get { return VenueRepository as IQueryable<Venue>; }
        }

        public void Add(Venue venue)
        {
            VenueRepository.Add(venue);
        }

        public void Remove(Venue venue)
        {
            VenueRepository.Remove(venue);
        }
    }
}
