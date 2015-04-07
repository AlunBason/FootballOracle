using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Teams;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballOracle.Models.Entities
{
    public class Team : BaseHeaderEntity<TeamV>
    {
        public Team()
        {
            Versions = new HashSet<TeamV>();
            LookupTeams = new HashSet<LookupTeam>();
            MatchEvents = new HashSet<MatchEvent>();
            Team1MatchVs = new HashSet<MatchV>();
            Team2MatchVs = new HashSet<MatchV>();
        }

        #region Navigation properties
        public virtual ICollection<LookupTeam> LookupTeams { get; set; }
        public virtual ICollection<MatchEvent> MatchEvents { get; set; }
        public virtual ICollection<MatchV> Team1MatchVs { get; set; }
        public virtual ICollection<MatchV> Team2MatchVs { get; set; }
        #endregion
    }

    public static class TeamExtensions
    {
        public static IEnumerable<BaseTeamViewModel> GetEditableVersions(this Team entity, DateTime viewDate)
        {
            return entity.Versions.OrderByDescending(v => v.EffectiveFrom).ThenByDescending(v => v.IsActive).ToViewModels(viewDate);
        }

        public static IEnumerable<BaseTeamViewModel> ToViewModels(this IEnumerable<Team> headers, DateTime viewDate)
        {
            foreach (var header in headers)
                yield return header.ToViewModel(viewDate);
        }

        public static BaseTeamViewModel ToViewModel(this Team header, DateTime viewDate)
        {
            return header.ToViewModel<BaseTeamViewModel, Team, TeamV>(viewDate);
        }
    }
}
