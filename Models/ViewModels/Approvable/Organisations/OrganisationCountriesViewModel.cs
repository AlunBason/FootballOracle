using System.Collections.Generic;
using FootballOracle.Models.ViewModels.Approvable.Countries;

namespace FootballOracle.Models.ViewModels.Approvable.Organisations
{
    public class OrganisationCountriesViewModel : BaseOrganisationViewModel
    {
        public IEnumerable<BaseCountryViewModel> ChildCountryViewModels { get; set; }
    }
}
