using System;
using FootballOracle.Models.Attributes;
using FootballOracle.Models.ViewModels.Approvable.Teams;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class TeamLookupsViewModel
    {
        public Guid TeamKey { get; set; }
        public BaseTeamViewModel TeamViewModel { get; set; }

        [HideLabel]
        public string EspnLookupId { get; set; }

        [HideLabel]
        public string SoccerbaseLookupId { get; set; }
    }
}
