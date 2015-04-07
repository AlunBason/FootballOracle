using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.ViewModels.Approvable.Matches
{
    public class MatchPlayersEditorViewModel : BaseMatchViewModel
    {
        public int TeamNumber { get; set; }
        
        public List<MatchPersonViewModel> MatchPersonViewModels { get; set; }
        public List<MatchPersonViewModel> NewMatchPersonViewModels { get; set; }

    }
}
