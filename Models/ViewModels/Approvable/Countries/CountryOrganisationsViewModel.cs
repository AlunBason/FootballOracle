using System.Collections.Generic;
using FootballOracle.Models.ViewModels.Approvable.Organisations;

namespace FootballOracle.Models.ViewModels.Approvable.Countries
{
    public class CountryOrganisationsViewModel : BaseCountryViewModel
    {
        public IEnumerable<BaseOrganisationViewModel> ChildOrganisationViewModels { get; set; }
    }
}
