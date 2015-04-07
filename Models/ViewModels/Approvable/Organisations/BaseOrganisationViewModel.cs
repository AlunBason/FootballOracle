using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using FootballOracle.Models.ViewModels.Base;

namespace FootballOracle.Models.ViewModels.Approvable.Organisations
{
    public class BaseOrganisationViewModel : BaseApprovableViewModel<Organisation, OrganisationV>, ISearchResult, IApprovableLinkData
    {
        public override string ToString()
        {
            return VersionEntity.OrganisationName;
        }

        public bool HasChildCompetitions { get; set; }
        public bool HasChildCountries { get; set; }
        public bool HasChildOrganisations { get; set; }

        public BaseOrganisationViewModel ParentOrganisationViewModel
        {
            get { return VersionEntity.ParentOrganisation.ToViewModel(ViewDate); }
        }

        public BaseCountryViewModel CountryViewModel
        {
            get { return VersionEntity.Country.ToViewModel(ViewDate); }
        }

        public AreaType AreaType
        {
            get { return AreaType.Org; }
        }

        public override string Description
        {
            get { return VersionEntity.OrganisationDescription; }
        }

        public IApprovableLinkData ParentLinkData
        {
            get 
            {
                if (VersionEntity.ParentOrganisation != null && IsParentDisplayed)
                    return ParentOrganisationViewModel;
                else if (VersionEntity.Country != null && IsParentDisplayed)
                    return CountryViewModel;

                return null;
            }
        }

        public override Organisation HeaderEntity
        {
            get { return VersionEntity.Organisation; }
        }
    }
}
