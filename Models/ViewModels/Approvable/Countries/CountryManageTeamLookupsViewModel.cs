using System.Collections.Generic;
using FootballOracle.Models.ViewModels.Standard;

namespace FootballOracle.Models.ViewModels.Approvable.Countries
{
    public class CountryManageTeamLookupsViewModel : BaseCountryViewModel
    {
        public List<TeamLookupsViewModel> TeamLookupsViewModels { get; set; }
    }
}
