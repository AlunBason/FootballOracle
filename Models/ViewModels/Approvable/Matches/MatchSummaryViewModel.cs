using System.Collections.Generic;

namespace FootballOracle.Models.ViewModels.Approvable.Matches
{
    public class MatchSummaryViewModel : BaseMatchViewModel
    {
        public IEnumerable<BaseMatchViewModel> OtherMatchesByCompetitionAndDate { get; set; }
    }
}
