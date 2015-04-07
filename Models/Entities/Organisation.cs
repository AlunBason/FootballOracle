using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Organisations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballOracle.Models.Entities
{
    public class Organisation : BaseHeaderEntity<OrganisationV>
    {
        public Organisation()
        {
            CompetitionVs = new HashSet<CompetitionV>();
            CountryVs = new HashSet<CountryV>();
            Versions = new HashSet<OrganisationV>();
            ParentOrganisationVs = new HashSet<OrganisationV>();
        }

        #region Navigation properties
        public virtual ICollection<CompetitionV> CompetitionVs { get; set; }
        public virtual ICollection<CountryV> CountryVs { get; set; }
        public virtual ICollection<OrganisationV> ParentOrganisationVs { get; set; }
        #endregion
    }

    public static class OrganisationExtensions
    {
        public static IEnumerable<BaseOrganisationViewModel> ToViewModels(this IEnumerable<Organisation> headers, DateTime viewDate)
        {
            foreach (var header in headers)
                yield return header.ToViewModel(viewDate);
        }

        public static BaseOrganisationViewModel ToViewModel(this Organisation header, DateTime viewDate)
        {
            return header.ToViewModel<BaseOrganisationViewModel, Organisation, OrganisationV>(viewDate);
        }

        public static IEnumerable<BaseOrganisationViewModel> GetEditableVersions(this Organisation entity, DateTime viewDate)
        {
            return entity.Versions.OrderByDescending(v => v.EffectiveFrom).ThenByDescending(v => v.IsActive).ToViewModels(viewDate);
        }

        public static CountryV GetParentCountry(this Organisation organisation, DateTime viewDate)
        {
            var organisationV = organisation.GetApprovedVersion<OrganisationV>(viewDate);

            if (organisationV == null)
                return null;

            if (organisationV.CountryGuid != null)
                return organisationV.Country.GetApprovedVersion<CountryV>(viewDate);

            if (organisationV.ParentOrganisationGuid != null)
                return organisationV.ParentOrganisation.GetParentCountry(viewDate);

            return null;
        }
    }
}
