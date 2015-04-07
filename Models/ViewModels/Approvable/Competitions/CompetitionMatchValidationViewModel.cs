using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using System;

namespace FootballOracle.Models.ViewModels.Approvable.Competitions
{
    public class CompetitionMatchValidationViewModel : BaseCompetitionViewModel
    {
        public IEnumerable<BaseMatchViewModel> MatchesWithoutVenues { get; set; }
        public IEnumerable<DateTime> MatchDatesWithoutEvents { get; set; }

        [Display(Name = "Fix matches without venues")]
        public bool FixMatchesWithoutVenues { get; set; }

        [Display(Name = "Fix matches without events")]
        public int FixMatchesWithoutEvents { get; set; }
    }
}
