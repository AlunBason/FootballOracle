using FootballOracle.Models.ViewModels.Approvable.Matches;
using System.Collections.Generic;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class TableWithResultsViewModel
    {
        public IEnumerable<LeagueTableItemViewModel> LeagueTable { get; set; }
        public IEnumerable<BaseMatchViewModel> MatchViewModels { get; set; }
    }
}
