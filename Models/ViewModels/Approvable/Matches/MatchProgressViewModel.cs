using FootballOracle.Models.ViewModels.Standard.Charts;
using System.Collections.Generic;

namespace FootballOracle.Models.ViewModels.Approvable.Matches
{
    public class MatchProgressViewModel : BaseMatchViewModel
    {
        public IEnumerable<PositionDateData> TeamPositionChartData { get; set; }
        public int TeamCount { get; set; }
    }
}
