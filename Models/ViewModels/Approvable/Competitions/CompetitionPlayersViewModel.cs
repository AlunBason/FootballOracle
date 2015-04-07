using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Standard;
using System.Collections.Generic;

namespace FootballOracle.Models.ViewModels.Approvable.Competitions
{
    public class CompetitionPlayersViewModel : BaseCompetitionViewModel
    {
        //public List<PersonTableItemViewModel> PersonTableItemViewModels { get; set; }
        public IEnumerable<PersonMatchEventCountViewModel> GoalsEventTotals { get; set; }
        public IEnumerable<PersonMatchEventCountViewModel> SentOffEventTotals { get; set; }
        public IEnumerable<PersonMatchEventCountViewModel> BookedEventTotals { get; set; }
    }
}
