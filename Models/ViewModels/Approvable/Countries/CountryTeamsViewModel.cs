using System.Collections.Generic;
using FootballOracle.Models.ViewModels.Approvable.Teams;

namespace FootballOracle.Models.ViewModels.Approvable.Countries
{
    public class CountryTeamsViewModel : BaseCountryViewModel
    {
        public IEnumerable<BaseTeamViewModel> ChildTeamViewModels { get; set; }
        
    }
}
