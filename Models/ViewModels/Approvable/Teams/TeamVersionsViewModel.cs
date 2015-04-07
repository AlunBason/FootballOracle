using System.Collections.Generic;

namespace FootballOracle.Models.ViewModels.Approvable.Teams
{
    public class TeamVersionsViewModel : BaseTeamViewModel
    {
        public IEnumerable<BaseTeamViewModel> TeamVersionList { get; set; }
    }
}
