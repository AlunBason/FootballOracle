using FootballOracle.Models.ViewModels.Standard;
using System.Collections.Generic;

namespace FootballOracle.Models.ViewModels.Approvable.Competitions
{
    public class CompetitionLeagueSummaryViewModel : BaseCompetitionViewModel
    {
        public int ViewType { get; set; }
        public int ResultsPage { get; set; }
    }
}
