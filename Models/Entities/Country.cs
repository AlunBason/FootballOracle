using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballOracle.Models.Entities
{
    public class Country : BaseHeaderEntity<CountryV>
    {
        public Country()
        {
            Versions = new HashSet<CountryV>();
            OrganisationVs = new HashSet<OrganisationV>();
            PersonVs = new HashSet<PersonV>();
            TeamVs = new HashSet<TeamV>();
            VenueVs = new HashSet<VenueV>();
        }

        #region Navigation properties
        public virtual ICollection<OrganisationV> OrganisationVs { get; set; }
        public virtual ICollection<PersonV> PersonVs { get; set; }
        public virtual ICollection<TeamV> TeamVs { get; set; }
        public virtual ICollection<VenueV> VenueVs { get; set; }
        #endregion
    }

    public static class CountryExtensions
    {
        public static IEnumerable<BaseCountryViewModel> ToViewModels(this IEnumerable<Country> headers, DateTime viewDate)
        {
            foreach (var header in headers)
                yield return header.ToViewModel(viewDate);
        }

        public static BaseCountryViewModel ToViewModel(this Country header, DateTime viewDate)
        {
            return header.ToViewModel<BaseCountryViewModel, Country, CountryV>(viewDate);
        }

        public static IEnumerable<BaseCountryViewModel> GetEditableVersions(this Country entity, DateTime viewDate)
        {
            return entity.Versions.OrderByDescending(v => v.EffectiveFrom).ThenByDescending(v => v.IsActive).ToViewModels(viewDate);
        }
    }
}
