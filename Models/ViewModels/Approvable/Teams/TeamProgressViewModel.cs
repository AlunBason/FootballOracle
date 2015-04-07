using FootballOracle.Models.ViewModels.Standard.Charts;
using System.Collections.Generic;

namespace FootballOracle.Models.ViewModels.Approvable.Teams
{
    public class TeamProgressViewModel : BaseTeamViewModel
    {
        public IEnumerable<PositionDateData> TeamPositionChartData { get; set; }
        public int TeamCount { get; set; }
    }
}
