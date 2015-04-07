using System;
using System.Collections.Generic;
using System.Linq;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using FootballOracle.Models.ViewModels.Approvable.Teams;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class LeagueTableItemViewModel : IComparable<LeagueTableItemViewModel>, IEquatable<LeagueTableItemViewModel>
    {
        public BaseTeamViewModel TeamViewModel { get; set; }
        public int HomeWins { get; set; }
        public int HomeDraws { get; set; }
        public int HomeLosses { get; set; }
        public int AwayWins { get; set; }
        public int AwayDraws { get; set; }
        public int AwayLosses { get; set; }
        public int HomeScored { get; set; }
        public int HomeConceded { get; set; }
        public int AwayScored { get; set; }
        public int AwayConceded { get; set; }
        public int Position { get; set; }
        public BaseMatchViewModel MatchViewModel { get; set; }
        public DateTime ViewDate { get; set; }

        public int Wins
        {
            get { return HomeWins + AwayWins; }
        }

        public int Draws
        {
            get { return HomeDraws + AwayDraws; }
        }

        public int Losses
        {
            get { return HomeLosses + AwayLosses; }
        }

        public int HomePlayed
        {
            get { return HomeWins + HomeDraws + HomeLosses; }
        }

        public int AwayPlayed
        {
            get { return AwayWins + AwayDraws + AwayLosses; }
        }

        public int Played
        {
            get { return HomePlayed + AwayPlayed; }
        }

        public int Scored
        {
            get { return HomeScored + AwayScored; }
        }

        public int Conceded
        {
            get { return HomeConceded + AwayConceded; }
        }

        public int GoalDifference
        {
            get { return Scored - Conceded; }
        }

        public int Points
        {
            get { return (Wins * 3) + Draws; }
        }

        public double HomePoints
        {
            get { return (HomeWins * 3) + HomeDraws; }
        }

        public double AwayPoints
        {
            get { return (AwayWins * 3) + AwayDraws; }
        }

        public double PointsPerGame
        {
            get { return Points / Played; }
        }

        public double HomePointsPerGame
        {
            get { return HomePoints / HomePlayed; }
        }

        public double AwayPointsPerGame
        {
            get { return AwayPoints / AwayPlayed; }
        }

        public int CompareTo(LeagueTableItemViewModel other)
        {
            if (Points != other.Points)
                return -Points.CompareTo(other.Points);

            if (GoalDifference != other.GoalDifference)
                return -GoalDifference.CompareTo(other.GoalDifference);

            if (Scored != other.Scored)
                return -Scored.CompareTo(other.Scored);

            return TeamViewModel.ToString().CompareTo(other.TeamViewModel.ToString());
        }

        public bool Equals(LeagueTableItemViewModel other)
        {
            return Points == other.Points && GoalDifference == other.GoalDifference && Scored == other.Scored;
        }
    }

    public static class _LeagueTableItemViewModelExtensions
    {
        public static void ConstructLeagueTableItems(this List<LeagueTableItemViewModel> leagueTableViewModels, IEnumerable<BaseTeamViewModel> teamViewModels)
        {
            foreach(var teamViewModel in teamViewModels)
                leagueTableViewModels.Add(new LeagueTableItemViewModel() { TeamViewModel = teamViewModel });
        }
    }
}
