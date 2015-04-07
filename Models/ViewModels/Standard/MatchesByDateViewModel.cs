using FootballOracle.Models.ViewModels.Approvable.Matches;
using System;
using System.Collections.Generic;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class MatchesByDateViewModel
    {
        public DateTime ViewDate { get; set; }
        public IEnumerable<BaseMatchViewModel> ViewDateMatchViewModels { get; set; }
        public IEnumerable<BaseMatchViewModel> PreviousMatchViewModels { get; set; }
        public IEnumerable<BaseMatchViewModel> NextMatchViewModels { get; set; }
    }
}
