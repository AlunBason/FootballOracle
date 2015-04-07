using FootballOracle.Models.ViewModels.Standard;
using System.Collections.Generic;

namespace FootballOracle.Models.ViewModels.Approvable.Competitions
{
    public class CompetitionManageLookupsViewModel : BaseCompetitionViewModel
    {
        public List<LookupCompetitionViewModel> LookupCompetitionViewModels { get; set; }
    }
}
