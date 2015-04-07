using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.People;
using System.Collections.Generic;

namespace FootballOracle.Models.ViewModels.Approvable.Matches
{
    public class MatchPlayersEventsEditorViewModel : BaseMatchViewModel
    {
        public List<MatchEventPersonViewModel> Team1MatchEventPersonViewModels { get; set; }
        public List<MatchEventPersonViewModel> Team2MatchEventPersonViewModels { get; set; }

        public MatchEventPersonViewModel NewTeam1MatchEventPersonViewModel { get; set; }
        public MatchEventPersonViewModel NewTeam2MatchEventPersonViewModel { get; set; }

        public IEnumerable<ICodePickerData> Team1PersonViewModels { get; set; }
        public IEnumerable<ICodePickerData> Team2PersonViewModels { get; set; }
    }
}
