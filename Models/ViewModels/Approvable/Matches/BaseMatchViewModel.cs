using FootballOracle.Foundation;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Competitions;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using FootballOracle.Models.ViewModels.Approvable.Teams;
using FootballOracle.Models.ViewModels.Approvable.Venues;
using FootballOracle.Models.ViewModels.Base;
using FootballOracle.Models.ViewModels.Standard;
using FootballOracle.Models.ViewModels.Standard.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballOracle.Models.ViewModels.Approvable.Matches
{
    public class BaseMatchViewModel : BaseApprovableViewModel<Match, MatchV>
    {
        public override string ToString()
        {
            return string.Format("{0} {1} - {2} {3}", Team1ViewModel.ToString(), Team1Ft, Team2Ft, Team2ViewModel.ToString());
        }

        #region EditorProperties
        public virtual DateTime MatchDate 
        {
            get { return VersionEntity.MatchDate.AddTicks(VersionEntity.MatchTimeTicks ?? 0); }
            set { }
        }

        public virtual Guid? CompetitionGuid
        {
            get { return VersionEntity.CampaignStage.Campaign.CompetitionKey; }
            set { }
        }
        
        public virtual Guid? VenueGuid
        {
            get { return VersionEntity.VenueGuid; }
            set { }
        }
        
        public virtual Guid Team1Guid
        {
            get { return VersionEntity.Team1Guid; }
            set { }
        }

        public virtual short? Team1Ht
        {
            get { return VersionEntity.Team1HT; }
            set { }
        }

        public virtual short? Team1Ft
        {
            get { return VersionEntity.Team1FT; }
            set { }
        }

        public virtual Guid Team2Guid
        {
            get { return VersionEntity.Team2Guid; }
            set { }
        }

        public virtual short? Team2Ht
        {
            get { return VersionEntity.Team2HT; }
            set { }
        }

        public virtual short? Team2Ft
        {
            get { return VersionEntity.Team2FT; }
            set { }
        }
        #endregion

        private BaseCampaignStageViewModel campaignStageViewModel;
        public BaseCampaignStageViewModel CampaignStageViewModel
        {
            get { return campaignStageViewModel = campaignStageViewModel ?? VersionEntity.CampaignStage.ToViewModel(ViewDate); }
        }

        private BaseCampaignViewModel campaignViewModel;
        public BaseCampaignViewModel CampaignViewModel
        {
            get { return campaignViewModel = campaignViewModel ?? CampaignStageViewModel.Entity.Campaign.ToViewModel(ViewDate); }
        }

        private BaseCompetitionViewModel competitionViewModel;
        public BaseCompetitionViewModel CompetitionViewModel
        {
            get { return competitionViewModel = competitionViewModel ?? CampaignViewModel.Entity.Competition.ToViewModel(ViewDate); }
        }

        private BaseCountryViewModel countryViewModel;
        public BaseCountryViewModel CountryViewModel
        {
            get { return countryViewModel = countryViewModel ?? CompetitionViewModel.HeaderEntity.GetParentCountry(ViewDate).ToViewModel(ViewDate); }
        }

        private BaseVenueViewModel venueViewModel;
        public BaseVenueViewModel VenueViewModel
        {
            get 
            {
                if (venueViewModel == null)
                {
                    if (VersionEntity.Venue != null)
                        venueViewModel = VersionEntity.Venue.ToViewModel(ViewDate);
                }

                return venueViewModel;
            }
        }

        private BaseTeamViewModel team1ViewModel;
        public BaseTeamViewModel Team1ViewModel
        {
            get { return team1ViewModel = team1ViewModel ?? VersionEntity.Team1.ToViewModel(ViewDate); }
        }

        private BaseTeamViewModel team2ViewModel;
        public BaseTeamViewModel Team2ViewModel
        {
            get { return team2ViewModel = team2ViewModel ?? VersionEntity.Team2.ToViewModel(ViewDate); }
        }

        public bool IsAggregateScoreAvailable
        {
            get
            {
                return CampaignStageViewModel.Entity.LegCount == 2
                    && LinkedMatchViewModel != null
                    && Team1Ft != null && Team2Ft != null
                    && LinkedMatchViewModel.Team1Ft != null && LinkedMatchViewModel.Team2Ft != null
                    && LinkedMatchViewModel.MatchDate < MatchDate;
            }
        }

        private BaseMatchViewModel linkedMatchViewModel;
        public BaseMatchViewModel LinkedMatchViewModel
        {
            get
            {
                if (linkedMatchViewModel == null)
                {
                    var query = from m in CampaignStageViewModel.Entity.MatchVs
                                where m.Team1Guid == VersionEntity.Team2Guid
                                && m.Team2Guid == VersionEntity.Team1Guid
                                select m.Match;

                    var match = query.FirstOrDefault();

                    linkedMatchViewModel = match != null ? match.ToViewModel(ViewDate) : null;
                }

                return linkedMatchViewModel;
            }
        }

        private IEnumerable<MatchEventViewModel> team1Starters;
        public IEnumerable<MatchEventViewModel> Team1Starters
        {
            get { return team1Starters = team1Starters ?? VersionEntity.MatchEvents.Where(e => e.MatchEventType == MatchEventType.Started && e.TeamPrimaryKey == Team1Guid).OrderBy(e => e.PositionType).ToViewModels(MatchDate); }
        }

        private IEnumerable<MatchEventViewModel> team1Substitutes;
        public IEnumerable<MatchEventViewModel> Team1Substitutes
        {
            get { return team1Substitutes = team1Substitutes ?? VersionEntity.MatchEvents.Where(e => e.MatchEventType == MatchEventType.Substitute && e.TeamPrimaryKey == Team1Guid).OrderBy(e => e.PositionType).ToViewModels(MatchDate); }
        }

        private IEnumerable<MatchEventViewModel> team2Starters;
        public IEnumerable<MatchEventViewModel> Team2Starters
        {
            get { return team2Starters = team2Starters ?? VersionEntity.MatchEvents.Where(e => e.MatchEventType == MatchEventType.Started && e.TeamPrimaryKey == Team2Guid).OrderBy(e => e.PositionType).ToViewModels(MatchDate); }
        }

        private IEnumerable<MatchEventViewModel> team2Substitutes;
        public IEnumerable<MatchEventViewModel> Team2Substitutes
        {
            get { return team2Substitutes = team2Substitutes ?? VersionEntity.MatchEvents.Where(e => e.MatchEventType == MatchEventType.Substitute && e.TeamPrimaryKey == Team2Guid).OrderBy(e => e.PositionType).ToViewModels(MatchDate); }
        }

        private IEnumerable<MatchEventViewModel> team1Goals;
        public IEnumerable<MatchEventViewModel> Team1Goals
        {
            get { return team1Goals = team1Goals ?? VersionEntity.MatchEvents.Where(e => (e.TeamPrimaryKey == Team1Guid && e.MatchEventType == MatchEventType.Scored) || (e.TeamPrimaryKey == Team2Guid && e.MatchEventType == MatchEventType.OwnGoal)).OrderBy(e => e.Minute).ThenBy(e => e.Extra).ToViewModels(MatchDate); }
        }

        private IEnumerable<MatchEventViewModel> team2Goals;
        public IEnumerable<MatchEventViewModel> Team2Goals
        {
            get { return team2Goals = team2Goals ?? VersionEntity.MatchEvents.Where(e => (e.TeamPrimaryKey == Team2Guid && e.MatchEventType == MatchEventType.Scored) || (e.TeamPrimaryKey == Team1Guid && e.MatchEventType == MatchEventType.OwnGoal)).OrderBy(e => e.Minute).ThenBy(e => e.Extra).ToViewModels(MatchDate); }
        }

        public double OverallTeam1GoalsPrediction
        {
            get { return (Team1ViewModel.GoalsScoredForm ?? 0 + Team2ViewModel.GoalsConcededForm ?? 1) / 2; }
        }

        public double OverallTeam2GoalsPrediction
        {
            get { return (Team2ViewModel.GoalsScoredForm ?? 0 + Team1ViewModel.GoalsConcededForm ?? 1) / 2; }
        }

        public double HomeVAwayTeam1GoalsPrediction
        {
            get { return (Team1ViewModel.HomeGoalsScoredForm ?? 0 + Team2ViewModel.AwayGoalsConcededForm ?? 1) / 2; }
        }

        public double HomeVAwayTeam2GoalsPrediction
        {
            get { return (Team2ViewModel.AwayGoalsScoredForm ?? 0 + Team1ViewModel.HomeGoalsConcededForm ?? 1) / 2; }
        }

        public override Match HeaderEntity
        {
            get { return VersionEntity.Match; }
        }

        public int GetPointsGained(bool forTeam1)
        {
            if (Team1Ft == null || Team2Ft == null)
                return 0;

            if ((forTeam1 && Team1Ft > Team2Ft) || (!forTeam1 && Team1Ft < Team2Ft))
                return 3;

            if (Team1Ft == Team2Ft)
                return 1;

            return 0;
        }

        public MatchResult GetMatchResult(Guid teamKey)
        {
            if (Team1Ft != null && Team2Ft != null && Team1Ft == Team2Ft)
                return MatchResult.Draw;

            if ((Team1Guid == teamKey && Team1Ft > Team2Ft) || (Team2Guid == teamKey && Team1Ft < Team2Ft))
                return MatchResult.Win;

            if ((Team1Guid == teamKey && Team1Ft < Team2Ft) || (Team2Guid == teamKey && Team1Ft > Team2Ft))
                return MatchResult.Lose;
            
            return MatchResult.UnResolved;
}
    }

    public static class _BaseMatchViewModelExtensions
    {
        public static List<MatchPersonViewModel> GetMatchPersonViewModels(this BaseMatchViewModel baseMatchViewModel, Guid teamKey, DateTime viewDate)
        {
            var matchPersonViewModels = new List<MatchPersonViewModel>();

            var campaignMatchVs = baseMatchViewModel.VersionEntity.CampaignStage.MatchVs.Where(m => (m.Team1Guid == teamKey || m.Team2Guid == teamKey)).Where(e => e.IsActive && !e.IsMarkedForDeletion && e.EffectiveFrom <= viewDate && e.EffectiveTo >= viewDate);

            foreach (var campaignMatchV in campaignMatchVs)
            {
                var matchEventVs = campaignMatchV.MatchEvents.Where(m => m.TeamPrimaryKey == teamKey).GroupBy(m => m.PersonPrimaryKey);

                foreach (var matchEventV in matchEventVs)
                {
                    if (!matchPersonViewModels.Any(m => m.PersonGuid == matchEventV.First().PersonPrimaryKey))
                    {
                        var personViewModel = matchEventV.First().Person.ToViewModel(viewDate);

                        MatchEventStartType? matchEventStartType = null;

                        if (baseMatchViewModel.Team1Starters.Any(t => t.Entity.PersonPrimaryKey == personViewModel.HeaderKey) || baseMatchViewModel.Team2Starters.Any(t => t.Entity.PersonPrimaryKey == personViewModel.HeaderKey))
                            matchEventStartType = MatchEventStartType.Started;
                        else if (baseMatchViewModel.Team1Substitutes.Any(t => t.Entity.PersonPrimaryKey == personViewModel.HeaderKey) || baseMatchViewModel.Team2Substitutes.Any(t => t.Entity.PersonPrimaryKey == personViewModel.HeaderKey))
                            matchEventStartType = MatchEventStartType.Substitute;

                        matchPersonViewModels.Add(new MatchPersonViewModel()
                        {
                            PersonGuid = personViewModel.HeaderKey,
                            PersonName = personViewModel.ToString(),
                            PositionType = matchEventV.First(m => m.MatchEventType == MatchEventType.Started || m.MatchEventType == MatchEventType.Substitute).PositionType,
                            MatchEventStartType = matchEventStartType
                        });
                    }
                }
            }

            matchPersonViewModels.Sort();
            return matchPersonViewModels;
        }

        public static List<MatchEventPersonViewModel> GetMatchEventPersonViewModels(this BaseMatchViewModel baseMatchViewModel, bool isTeam1, DateTime viewDate)
        {
            var matchEventPersonViewModels = new List<MatchEventPersonViewModel>();

            var matchEvents = isTeam1
                ? baseMatchViewModel.VersionEntity.MatchEvents.Where(m => m.TeamPrimaryKey == baseMatchViewModel.VersionEntity.Team1Guid && m.MatchEventType != MatchEventType.Started && m.MatchEventType != MatchEventType.Substitute)
                : baseMatchViewModel.VersionEntity.MatchEvents.Where(m => m.TeamPrimaryKey == baseMatchViewModel.VersionEntity.Team2Guid && m.MatchEventType != MatchEventType.Started && m.MatchEventType != MatchEventType.Substitute);

            foreach (var matchEvent in matchEvents)
            {
                MatchEventInRunningType? matchEventInRunningType = null;

                switch (matchEvent.MatchEventType)
                {
                    case MatchEventType.Booked:
                        matchEventInRunningType = MatchEventInRunningType.Booked;
                        break;

                    case MatchEventType.BroughtOn:
                        matchEventInRunningType = MatchEventInRunningType.BroughtOn;
                        break;

                    case MatchEventType.OwnGoal:
                        matchEventInRunningType = MatchEventInRunningType.OwnGoal;
                        break;

                    case MatchEventType.Scored:
                        matchEventInRunningType = MatchEventInRunningType.Scored;
                        break;

                    case MatchEventType.SentOff:
                        matchEventInRunningType = MatchEventInRunningType.SentOff;
                        break;

                    case MatchEventType.TakenOff:
                        matchEventInRunningType = MatchEventInRunningType.TakenOff;
                        break;
                }

                matchEventPersonViewModels.Add(new MatchEventPersonViewModel()
                {
                    PrimaryKey = matchEvent.PrimaryKey,
                    PersonPrimaryKey = matchEvent.PersonPrimaryKey,
                    PersonName = matchEvent.Person.ToViewModel(viewDate).ToString(),
                    MatchEventInRunningType = matchEventInRunningType,
                    Minute = matchEvent.Minute,
                    Extra = matchEvent.Extra
                });
            }

            matchEventPersonViewModels.Sort();
            return matchEventPersonViewModels;
        }

        public static IEnumerable<BaseTeamViewModel> GetTeamViewModels(this IEnumerable<BaseMatchViewModel> matchViewModels)
        {
            var team1ViewModels = matchViewModels.GroupBy(m => m.Team1ViewModel.HeaderKey).Select(m => m.First().Team1ViewModel);
            var team2ViewModels = matchViewModels.GroupBy(m => m.Team2ViewModel.HeaderKey).Select(m => m.First().Team2ViewModel);

            return team1ViewModels.Concat(team2ViewModels).GroupBy(m => m.HeaderKey).Select(t => t.First());
        }

        public static IEnumerable<LeagueTableItemViewModel> GetAccumulatedDataFromMatches(this IEnumerable<BaseMatchViewModel> matchViewModels, DateTime viewDate, Guid[] teamKeys)
        {
            var leagueTableViewModels = new List<LeagueTableItemViewModel>();
            var teamViewModels = matchViewModels.GetTeamViewModels();
            leagueTableViewModels.ConstructLeagueTableItems(teamViewModels);

            var accumulatedLeagueTableViewModels = new List<LeagueTableItemViewModel>();

            foreach (var group in matchViewModels.OrderBy(m => m.VersionEntity.MatchDate).GroupBy(g => g.VersionEntity.MatchDate))
            {
                var matchDate = group.First().VersionEntity.MatchDate;

                foreach (var matchViewModel in group)
                {
                    var includeTeam1 = !teamKeys.Any() || teamKeys.Contains(matchViewModel.Team1Guid);
                    var includeTeam2 = !teamKeys.Any() || teamKeys.Contains(matchViewModel.Team2Guid);
                    
                    var leagueTableItem1ViewModel = leagueTableViewModels.Single(l => l.TeamViewModel.HeaderKey == matchViewModel.Team1Guid);
                    var leagueTableItem2ViewModel = leagueTableViewModels.Single(l => l.TeamViewModel.HeaderKey == matchViewModel.Team2Guid);

                    matchViewModel.ProcessMatch(leagueTableItem1ViewModel, leagueTableItem2ViewModel, 3, viewDate);

                    foreach (var teamKey in teamKeys)
                    {
                        if (includeTeam1 || includeTeam2)
                        {
                            var accumulatedLeagueTableViewModel = accumulatedLeagueTableViewModels.SingleOrDefault(a => a.ViewDate == matchDate && a.TeamViewModel.HeaderKey == teamKey);

                            if (accumulatedLeagueTableViewModel != null)
                            {
                                if (accumulatedLeagueTableViewModel.MatchViewModel == null)
                                    accumulatedLeagueTableViewModel.MatchViewModel = matchViewModel.Team1Guid == teamKey || matchViewModel.Team2Guid == teamKey ? matchViewModel : null;
                            }
                            else
                            {
                                accumulatedLeagueTableViewModels.Add(new LeagueTableItemViewModel()
                                {
                                    MatchViewModel = matchViewModel.Team1Guid == teamKey || matchViewModel.Team2Guid == teamKey ? matchViewModel : null,
                                    ViewDate = matchDate,
                                    TeamViewModel = leagueTableViewModels.Single(l => l.TeamViewModel.HeaderKey == teamKey).TeamViewModel
                                });
                            }
                        }
                    }
                }

                leagueTableViewModels.Sort();

                foreach (var item in accumulatedLeagueTableViewModels.Where(l => l.ViewDate == matchDate))
                {
                    var leagueTableItemViewModel = leagueTableViewModels.Single(l => l.TeamViewModel.HeaderKey == item.TeamViewModel.HeaderKey);
                    item.Position = leagueTableViewModels.IndexOf(leagueTableItemViewModel) + 1;
                }
            }

            return accumulatedLeagueTableViewModels;
        }

        public static IEnumerable<LeagueTableItemViewModel> GetStandings(this IEnumerable<BaseMatchViewModel> matchViewModels, int viewType, DateTime viewDate, int matchCount = 0)
        {
            var leagueTableViewModels = new List<LeagueTableItemViewModel>();
            leagueTableViewModels.ConstructLeagueTableItems(matchViewModels.GetTeamViewModels());

            foreach (var matchViewModel in matchViewModels.OrderByDescending(m => m.MatchDate))
            {
                var leagueTableItem1ViewModel = leagueTableViewModels.Single(l => l.TeamViewModel.HeaderKey == matchViewModel.Team1Guid);
                var leagueTableItem2ViewModel = leagueTableViewModels.Single(l => l.TeamViewModel.HeaderKey == matchViewModel.Team2Guid);

                matchViewModel.ProcessMatch(leagueTableItem1ViewModel, leagueTableItem2ViewModel, viewType, viewDate, matchCount);
            }

            leagueTableViewModels.Sort();

            LeagueTableItemViewModel previousLeagueTableItemViewModel = null;

            foreach (var leagueTableViewModel in leagueTableViewModels)
            {
                if (previousLeagueTableItemViewModel != null)
                    leagueTableViewModel.Position = leagueTableViewModel.Equals(previousLeagueTableItemViewModel) ? previousLeagueTableItemViewModel.Position : previousLeagueTableItemViewModel.Position + 1;
                else
                    leagueTableViewModel.Position = 1;

                previousLeagueTableItemViewModel = leagueTableViewModel;
            }

            return leagueTableViewModels;
        }

        private static void ProcessMatch(this BaseMatchViewModel matchViewModel, LeagueTableItemViewModel leagueTableItem1ViewModel, LeagueTableItemViewModel leagueTableItem2ViewModel, int viewType, DateTime viewDate, int matchCount = 0)
        {
            if (matchViewModel.Team1Ft != null && matchViewModel.Team2Ft != null && matchViewModel.MatchDate <= viewDate.ToEndOfDay())
            {
                switch (Math.Sign((short)matchViewModel.Team1Ft - (short)matchViewModel.Team2Ft))
                {
                    case 1:
                        if (leagueTableItem1ViewModel.IsIncluded(true, viewType, matchCount))
                            leagueTableItem1ViewModel.HomeWins++;

                        if (leagueTableItem2ViewModel.IsIncluded(false, viewType, matchCount))
                            leagueTableItem2ViewModel.AwayLosses++;
                        break;

                    case 0:
                        if (leagueTableItem1ViewModel.IsIncluded(true, viewType, matchCount))
                            leagueTableItem1ViewModel.HomeDraws++;

                        if (leagueTableItem2ViewModel.IsIncluded(false, viewType, matchCount))
                            leagueTableItem2ViewModel.AwayDraws++;
                        break;

                    case -1:
                        if (leagueTableItem1ViewModel.IsIncluded(true, viewType, matchCount))
                            leagueTableItem1ViewModel.HomeLosses++;

                        if (leagueTableItem2ViewModel.IsIncluded(false, viewType, matchCount))
                            leagueTableItem2ViewModel.AwayWins++;
                        break;
                }

                if (leagueTableItem1ViewModel.IsIncluded(true, viewType, matchCount))
                {
                    leagueTableItem1ViewModel.HomeScored += (int)matchViewModel.Team1Ft;
                    leagueTableItem1ViewModel.HomeConceded += (int)matchViewModel.Team2Ft;
                }

                if (leagueTableItem2ViewModel.IsIncluded(false, viewType, matchCount))
                {
                    leagueTableItem2ViewModel.AwayConceded += (int)matchViewModel.Team1Ft;
                    leagueTableItem2ViewModel.AwayScored += (int)matchViewModel.Team2Ft;
                }
            }
        }

        private static bool IsIncluded(this LeagueTableItemViewModel item, bool isHome, int viewType, int matchCount)
        {
            switch (viewType)
            {
                case 1:
                    return isHome && (matchCount == 0 || item.HomePlayed < matchCount);

                case 2:
                    return !isHome && (matchCount == 0 || item.AwayPlayed < matchCount);

                case 3:
                    return matchCount == 0 || item.Played < matchCount;

                default:
                    return false;
            }
        }
    }
}