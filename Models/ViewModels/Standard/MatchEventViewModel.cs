using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using FootballOracle.Models.ViewModels.Approvable.People;
using FootballOracle.Models.ViewModels.Approvable.Teams;
using System;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class MatchEventViewModel
    {
        public MatchEvent Entity { get; set; }
        public DateTime ViewDate { get; set; }

        private BaseMatchViewModel matchViewModel;
        public BaseMatchViewModel MatchViewModel
        {
            get { return matchViewModel = matchViewModel ?? Entity.MatchV.ToViewModel(ViewDate); }
        }

        private BasePersonViewModel personViewModel;
        public BasePersonViewModel PersonViewModel
        {
            get { return personViewModel = personViewModel ?? Entity.Person.ToViewModel(ViewDate); }
        }

        private BaseTeamViewModel teamViewModel;
        public BaseTeamViewModel TeamViewModel
        {
            get { return teamViewModel = teamViewModel ?? Entity.Team.ToViewModel(ViewDate); }
        }
    }
}
