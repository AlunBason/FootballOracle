using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Venues;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballOracle.Models.Entities
{
    public class Venue : BaseHeaderEntity<VenueV>
    {
        public Venue()
        {
            Versions = new HashSet<VenueV>();
            LookupVenues = new HashSet<LookupVenue>();
            MatchVs = new HashSet<MatchV>();
            TeamVs = new HashSet<TeamV>();
        }

        public virtual ICollection<LookupVenue> LookupVenues { get; set; }
        public virtual ICollection<MatchV> MatchVs { get; set; }
        public virtual ICollection<TeamV> TeamVs { get; set; }
        
    }

    public static class VenueExtensions
    {
        public static IEnumerable<BaseVenueViewModel> ToViewModels(this IEnumerable<Venue> headers, DateTime viewDate)
        {
            foreach (var header in headers)
                yield return header.ToViewModel(viewDate);
        }

        public static BaseVenueViewModel ToViewModel(this Venue header, DateTime viewDate)
        {
            return header.ToViewModel<BaseVenueViewModel, Venue, VenueV>(viewDate);
        }

        public static IEnumerable<BaseVenueViewModel> GetEditableVersions(this Venue entity, DateTime viewDate)
        {
            return entity.Versions.OrderByDescending(v => v.EffectiveFrom).ThenByDescending(v => v.IsActive).ToViewModels(viewDate);
        }
    }
}
