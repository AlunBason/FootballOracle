using System.Collections.Generic;
using FootballOracle.Models.ViewModels.Standard;

namespace FootballOracle.Models.ViewModels.Approvable.Teams
{
    public class TeamManageLookupsViewModel : BaseTeamViewModel
    {
        public List<LookupTeamViewModel> LookupTeamViewModels { get; set; }
    }
}
