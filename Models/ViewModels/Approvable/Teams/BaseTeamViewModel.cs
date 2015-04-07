using System;
using System.Collections.Generic;
using System.Linq;
using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Competitions;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using FootballOracle.Models.ViewModels.Approvable.Venues;
using FootballOracle.Models.ViewModels.Base;
using FootballOracle.Models.ViewModels.Standard;
using FootballOracle.Models.ViewModels.Standard.Campaigns;

namespace FootballOracle.Models.ViewModels.Approvable.Teams
{
    public class BaseTeamViewModel : BaseApprovableViewModel<Team, TeamV>, ISearchResult, IApprovableLinkData
    {
        private string teamName = string.Empty;

        public override string ToString()
        {
 	         if (teamName == string.Empty && VersionEntity.TeamNames.Any())
                 teamName = VersionEntity.TeamNames.Where(f => f.TeamNameType == TeamNameType.Primary).OrderBy(o => o.LanguageType).First().Description;

             return teamName;
        }

        private string shortName = string.Empty;
        public string ShortName
        {
            get
            {
                if (shortName == string.Empty)
                {
                    var entity = VersionEntity.TeamNames.FirstOrDefault(a => a.TeamNameType == TeamNameType.ShortName);
                    shortName = entity != null ? entity.Description : ToString().Truncate(10);
                }

                return shortName;
            }
        }

        private string fullName = string.Empty;
        public string FullName
        {
            get
            {
                if (fullName == string.Empty)
                {
                    var entity = VersionEntity.TeamNames.FirstOrDefault(a => a.TeamNameType == TeamNameType.FullName);
                    fullName = entity != null ? entity.Description : ToString();
                }

                return fullName;
            }
        }

        private string nickName = string.Empty;
        public string NickName
        {
            get
            {
                if (nickName == string.Empty)
                {
                    var entity = VersionEntity.TeamNames.FirstOrDefault(a => a.TeamNameType == TeamNameType.Nickname);

                    if (entity != null)
                        nickName = entity.Description;
                }

                return nickName;
            }
        }

        public BaseCountryViewModel CountryViewModel
        {
            get { return VersionEntity.Country != null ? VersionEntity.Country.ToViewModel(ViewDate) : null; }
        }

        public BaseVenueViewModel VenueViewModel
        {
            get { return VersionEntity.HomeVenue != null ? VersionEntity.HomeVenue.ToViewModel(ViewDate) : null; }
        }

        public BaseCompetitionViewModel  LatestCompetitionViewModel
        {
            get
            {
                var lastMatchV = VersionEntity.Team.Team1MatchVs.Union(VersionEntity.Team.Team2MatchVs).OrderByDescending(t => t.MatchDate).FirstOrDefault();

                if (lastMatchV == null)
                    return null;

                return lastMatchV.CampaignStage.Campaign.Competition.ToViewModel(ViewDate);
            }
        }
        
        public static BaseTeamViewModel Create()
        {
            return Create(Guid.NewGuid());
        }

        public static BaseTeamViewModel Create(Guid headerGuid)
        {
            return new BaseTeamViewModel()
            {
                HeaderKey = headerGuid,
                EffectiveFrom = Date.LowDate,
                EffectiveTo = Date.HighDate
            };
        }

        private IEnumerable<BaseCampaignViewModel> campaignViewModels;
        public IEnumerable<BaseCampaignViewModel> CampaignViewModels
        {
            get 
            { 
                return campaignViewModels = campaignViewModels ?? VersionEntity.Team.Team1MatchVs
                    .Concat(VersionEntity.Team.Team2MatchVs)
                    .Select(m => m.CampaignStage.Campaign)
                    .Distinct()
                    .OrderByDescending(o => o.StartDate)
                    .ToViewModels(ViewDate); 
            }
        }

        public IEnumerable<DateRangePickerViewModel> CampaignPickerData
        {
            get
            {
                var campaignPickerData = new List<DateRangePickerViewModel>();

                foreach (var campaign in CampaignViewModels.OrderByDescending(c => c.Entity.StartDate))
                {
                    campaignPickerData.Add(new DateRangePickerViewModel()
                    {
                        EndDateString = campaign.Entity.EndDate.ToUrlString(),
                        Description = string.Format("{0}/{1}", campaign.Entity.StartDate.Year, campaign.Entity.EndDate.Year)
                    });
                }

                return campaignPickerData;
            }
        }

        public BaseCampaignViewModel SelectedCampaignViewModel { get; set; }
        public string SelectedCampaignDate { get; set; }

        public BaseCampaignViewModel SetSelectedCampaign()
        {
            if (!CampaignViewModels.Any())
                return null;

            var selectedCampaignViewModel = CampaignViewModels.FirstOrDefault(c => c.Entity.StartDate <= ViewDate && c.Entity.EndDate >= ViewDate && c.Entity.CampaignStages.Any(a => a.IsDefault));

            if (selectedCampaignViewModel == null)
            {
                selectedCampaignViewModel = CampaignViewModels.OrderByDescending(c => c.Entity.StartDate).First();
                ViewDate = selectedCampaignViewModel.Entity.EndDate;
            }

            SelectedCampaignDate = ViewDate.ToUrlString();

            return selectedCampaignViewModel;
        }

        public IApprovableLinkData ParentLinkData
        {
            get 
            { 
                if (VersionEntity.Country != null)
                    return CountryViewModel;

                return null;
            }
        }

        public AreaType AreaType
        {
            get { return AreaType.Tms; }
        }

        public override byte[] ResourceBytes
        {
            get { return VersionEntity.Resource != null ? VersionEntity.Resource.ResourceBytes : null; }
        }

        public override Team HeaderEntity
        {
            get { return VersionEntity.Team; }
        }

        public double? LeagueForm { get; set; }
        public double? HomeLeagueForm { get; set; }
        public double? AwayLeagueForm { get; set; }
        public double? GoalsScoredForm { get; set; }
        public double? GoalsConcededForm { get; set; }
        public double? HomeGoalsScoredForm { get; set; }
        public double? HomeGoalsConcededForm { get; set; }
        public double? AwayGoalsScoredForm { get; set; }
        public double? AwayGoalsConcededForm { get; set; }

        public IEnumerable<InformationItemViewModel> TeamStatistics { get; set; }

        public void SetFormData(int matchCount = 10)
        {
            //var selectedCampaignViewModel = SetSelectedCampaign();

            //var resultMatchViewModels = selectedCampaignViewModel.ResultMatchViewModels
            //    .Where(w => w.Team1Guid == HeaderKey || w.Team2Guid == HeaderKey)
            //    .OrderByDescending(o => o.MatchDate);

            var resultMatchViewModels = HeaderEntity.Team1MatchVs.Concat(HeaderEntity.Team2MatchVs)
                .Where(w => w.Team1FT != null && w.Team2FT != null)
                .OrderByDescending(o => o.MatchDate)
                .Take(10)
                .ToViewModels(ViewDate);

            var homeCount = 0;
            var awayCount = 0;
            var allCount = 0;

            double formTotal = 0;
            double homeFormTotal = 0;
            double awayFormTotal = 0;

            double homeGoalsScoredFormTotal = 0;
            double homeGoalsConcededFormTotal = 0;
            double awayGoalsScoredFormTotal = 0;
            double awayGoalsConcededFormTotal = 0;
            double goalsScoredFormTotal = 0;
            double goalsConcededFormTotal = 0;

            for (var index = 0; index <= resultMatchViewModels.Count() - 1; index++)
            {
                if (homeCount > matchCount && awayCount > matchCount)
                    continue;

                var matchViewModel = resultMatchViewModels.ElementAt(index);

                var isHomeTeam = matchViewModel.Team1Guid == HeaderKey;
                var isHomeWin = matchViewModel.Team1Ft > matchViewModel.Team2Ft;
                var isDraw = matchViewModel.Team1Ft == matchViewModel.Team2Ft;
                var isAwayWin = matchViewModel.Team1Ft < matchViewModel.Team2Ft;

                if (isHomeTeam)
                {
                    if (homeCount < matchCount)
                    {
                        var multiplier = Math.Pow(0.8, homeCount);

                        if (isHomeWin)
                            homeFormTotal += 3 * multiplier;

                        if (isDraw)
                            homeFormTotal += multiplier;

                        homeGoalsScoredFormTotal += (double)(matchViewModel.Team1Ft * multiplier);
                        homeGoalsConcededFormTotal += (double)(matchViewModel.Team2Ft * multiplier);

                        homeCount++;
                    }
                }

                if (!isHomeTeam)
                {
                    if (awayCount < matchCount)
                    {
                        var multiplier = Math.Pow(0.8, awayCount);

                        if (isAwayWin)
                            awayFormTotal += 3 * multiplier;

                        if (isDraw)
                            awayFormTotal += multiplier;

                        awayGoalsScoredFormTotal += (double)(matchViewModel.Team2Ft * multiplier);
                        awayGoalsConcededFormTotal += (double)(matchViewModel.Team1Ft * multiplier);

                        awayCount++;
                    }
                }

                if (allCount < matchCount)
                {
                    var multiplier = Math.Pow(0.8, allCount);

                    if ((isHomeTeam && isHomeWin) || (!isHomeTeam && isAwayWin))
                        formTotal += 3 * multiplier;

                    if (isDraw)
                        formTotal += multiplier;

                    goalsScoredFormTotal += isHomeTeam ? (double)(matchViewModel.Team1Ft * multiplier) : (double)(matchViewModel.Team2Ft * multiplier);
                    goalsConcededFormTotal += isHomeTeam ? (double)(matchViewModel.Team2Ft * multiplier) : (double)(matchViewModel.Team1Ft * multiplier);

                    allCount++;
                }
            }
            
            HomeLeagueForm = homeFormTotal / (3 * PredictorDataViewModel.GetFormMultiplier(homeCount));
            AwayLeagueForm = awayFormTotal / (3 * PredictorDataViewModel.GetFormMultiplier(awayCount));
            LeagueForm = formTotal / (3 * PredictorDataViewModel.GetFormMultiplier(allCount));

            HomeGoalsScoredForm = homeGoalsScoredFormTotal / PredictorDataViewModel.GetFormMultiplier(homeCount);
            HomeGoalsConcededForm = homeGoalsConcededFormTotal / PredictorDataViewModel.GetFormMultiplier(homeCount);
            
            AwayGoalsScoredForm = awayGoalsScoredFormTotal / PredictorDataViewModel.GetFormMultiplier(homeCount);
            AwayGoalsConcededForm = awayGoalsConcededFormTotal / PredictorDataViewModel.GetFormMultiplier(homeCount); 

            GoalsScoredForm = goalsScoredFormTotal / PredictorDataViewModel.GetFormMultiplier(allCount);
            GoalsConcededForm = goalsConcededFormTotal / PredictorDataViewModel.GetFormMultiplier(allCount); 
        }

        public void SetTeamStatistics()
        {
            var teamStatistics = new List<InformationItemViewModel>();

            var selectedCampaignViewModel = SetSelectedCampaign();

            selectedCampaignViewModel.SetCampaignStages(3);

            var resultMatchViewModels = selectedCampaignViewModel.SelectedCampaignStageViewModel.ResultMatchViewModels
                .Where(w => w.Team1Guid == HeaderKey || w.Team2Guid == HeaderKey)
                .OrderByDescending(o => o.MatchDate);

            var winningRun = new StatisticItemViewModel();
            var homeWinningRun = new StatisticItemViewModel();
            var awayWinningRun = new StatisticItemViewModel();
            
            var unbeatenRun = new StatisticItemViewModel();
            var homeUnbeatenRun = new StatisticItemViewModel();
            var awayUnbeatenRun = new StatisticItemViewModel();

            var scoringRun = new StatisticItemViewModel();
            var homeScoringRun = new StatisticItemViewModel();
            var awayScoringRun = new StatisticItemViewModel();

            int losingRun = 0;
            bool hasLosingRun = true;

            int nonWinningRun = 0;
            bool hasNonWinningRun = true;

            int nonScoringRun = 0;
            bool hasNonScoringRun = true;

            foreach (var matchViewModel in resultMatchViewModels)
            {
                var isHomeMatch = matchViewModel.Team1Guid == HeaderKey;
                var matchResultForTeam = matchViewModel.GetMatchResult(HeaderKey);

                if (!winningRun.HasItem && !homeWinningRun.HasItem && !awayWinningRun.HasItem)
                    break;

                ProcessWinningRun(winningRun, matchResultForTeam);
                ProcessHomeWinningRun(homeWinningRun, matchResultForTeam, isHomeMatch);
                ProcessAwayWinningRun(awayWinningRun, matchResultForTeam, isHomeMatch);
                
                ProcessUnbeatenRun(unbeatenRun, matchResultForTeam);
                ProcessHomeUnbeatenRun(homeUnbeatenRun, matchResultForTeam, isHomeMatch);
                ProcessAwayUnbeatenRun(awayUnbeatenRun, matchResultForTeam, isHomeMatch);

                ProcessScoringRun(scoringRun, matchViewModel, isHomeMatch);
                ProcessHomeScoringRun(homeScoringRun, matchViewModel, isHomeMatch);
                ProcessAwayScoringRun(awayScoringRun, matchViewModel, isHomeMatch);

                //Losing run
                if (hasLosingRun)
                {
                    if (matchResultForTeam == MatchResult.Lose)
                        losingRun++;
                    else
                        hasLosingRun = false;
                }

                //Unbeaten run
                if (hasNonWinningRun)
                {
                    if (matchResultForTeam == MatchResult.Lose || matchResultForTeam == MatchResult.Draw)
                        nonWinningRun++;
                    else
                        hasNonWinningRun = false;
                }

                //Non-scoring run
                if (hasNonScoringRun)
                {
                    if ((isHomeMatch && matchViewModel.Team1Ft == 0) || (!isHomeMatch && matchViewModel.Team2Ft == 0))
                        nonScoringRun++;
                    else
                        hasNonScoringRun = false;
                }
            }

            var competitionName = selectedCampaignViewModel.Entity.Competition.ToViewModel(ViewDate).ToString();

            if (winningRun.ItemCount > 1)
                teamStatistics.Add(new InformationItemViewModel(string.Format("Have won the last {0} {1} matches.", winningRun.ItemCount, competitionName), DisplayType.Success));

            if (homeWinningRun.ItemCount > 1)
                teamStatistics.Add(new InformationItemViewModel(string.Format("Have won the last {0} {1} matches at home.", homeWinningRun.ItemCount, competitionName), DisplayType.Success));

            if (awayWinningRun.ItemCount > 1)
                teamStatistics.Add(new InformationItemViewModel(string.Format("Have won the last {0} {1} matches away.", awayWinningRun.ItemCount, competitionName), DisplayType.Success));

            if (unbeatenRun.ItemCount > 1)
                teamStatistics.Add(new InformationItemViewModel(string.Format("Unbeaten for the last {0} {1} matches.", unbeatenRun.ItemCount, competitionName), DisplayType.Success));

            if (homeUnbeatenRun.ItemCount > 1)
                teamStatistics.Add(new InformationItemViewModel(string.Format("Unbeaten for the last {0} {1} matches at home.", homeUnbeatenRun.ItemCount, competitionName), DisplayType.Success));

            if (awayUnbeatenRun.ItemCount > 1)
                teamStatistics.Add(new InformationItemViewModel(string.Format("Unbeaten for the last {0} {1} matches away.", awayUnbeatenRun.ItemCount, competitionName), DisplayType.Success));

            if (scoringRun.ItemCount > 1)
                teamStatistics.Add(new InformationItemViewModel(string.Format("Have scored in the last {0} {1} matches.", scoringRun.ItemCount, competitionName), DisplayType.Success));

            if (homeScoringRun.ItemCount > 1)
                teamStatistics.Add(new InformationItemViewModel(string.Format("Have scored in the last {0} {1} matches at home.", homeScoringRun.ItemCount, competitionName), DisplayType.Success));

            if (awayScoringRun.ItemCount > 1)
                teamStatistics.Add(new InformationItemViewModel(string.Format("Have scored in the last {0} {1} matches away.", awayScoringRun.ItemCount, competitionName), DisplayType.Success));




            if (losingRun > 1)
                teamStatistics.Add(new InformationItemViewModel(string.Format("Have lost the last {0} {1} matches.", losingRun, competitionName), DisplayType.Danger));

            if (nonWinningRun > 1)
                teamStatistics.Add(new InformationItemViewModel(string.Format("Haven't won for the last {0} {1} matches.", nonWinningRun, competitionName), DisplayType.Danger));

            if (nonScoringRun > 2)
                teamStatistics.Add(new InformationItemViewModel(string.Format("Haven't scored in the last {0} {1} matches.", nonScoringRun, competitionName), DisplayType.Danger));

            TeamStatistics = teamStatistics;
        }

        private void ProcessWinningRun(StatisticItemViewModel winningRun, MatchResult matchResultForTeam)
        {
            if (winningRun.HasItem)
            {
                if (matchResultForTeam == MatchResult.Win)
                    winningRun.ItemCount++;
                else
                    winningRun.HasItem = false;
            }
        }

        private void ProcessHomeWinningRun(StatisticItemViewModel homeWinningRun, MatchResult matchResultForTeam, bool isHomeMatch)
        {
            if (homeWinningRun.HasItem && isHomeMatch)
            {
                if (matchResultForTeam == MatchResult.Win)
                    homeWinningRun.ItemCount++;
                else
                    homeWinningRun.HasItem = false;
            }
        }

        private void ProcessAwayWinningRun(StatisticItemViewModel awayWinningRun, MatchResult matchResultForTeam, bool isHomeMatch)
        {
            if (awayWinningRun.HasItem && !isHomeMatch)
            {
                if (matchResultForTeam == MatchResult.Win)
                    awayWinningRun.ItemCount++;
                else
                    awayWinningRun.HasItem = false;
            }
        }

        private void ProcessUnbeatenRun(StatisticItemViewModel unbeatenRun, MatchResult matchResultForTeam)
        {
            if (unbeatenRun.HasItem)
            {
                if (matchResultForTeam == MatchResult.Win || matchResultForTeam == MatchResult.Draw)
                    unbeatenRun.ItemCount++;
                else
                    unbeatenRun.HasItem = false;
            }
        }

        private void ProcessHomeUnbeatenRun(StatisticItemViewModel homeUnbeatenRun, MatchResult matchResultForTeam, bool isHomeMatch)
        {
            if (homeUnbeatenRun.HasItem && isHomeMatch)
            {
                if (matchResultForTeam == MatchResult.Win || matchResultForTeam == MatchResult.Draw)
                    homeUnbeatenRun.ItemCount++;
                else
                    homeUnbeatenRun.HasItem = false;
            }
        }

        private void ProcessAwayUnbeatenRun(StatisticItemViewModel awayUnbeatenRun, MatchResult matchResultForTeam, bool isHomeMatch)
        {
            if (awayUnbeatenRun.HasItem && !isHomeMatch)
            {
                if (matchResultForTeam == MatchResult.Win || matchResultForTeam == MatchResult.Draw)
                    awayUnbeatenRun.ItemCount++;
                else
                    awayUnbeatenRun.HasItem = false;
            }
        }

        private void ProcessScoringRun(StatisticItemViewModel scoringRun, BaseMatchViewModel matchViewModel, bool isHomeMatch)
        {
            if (scoringRun.HasItem)
            {
                if ((isHomeMatch && matchViewModel.Team1Ft > 0) || (!isHomeMatch && matchViewModel.Team2Ft > 0))
                    scoringRun.ItemCount++;
                else
                    scoringRun.HasItem = false;
            }
        }

        private void ProcessHomeScoringRun(StatisticItemViewModel homeScoringRun, BaseMatchViewModel matchViewModel, bool isHomeMatch)
        {
            if (homeScoringRun.HasItem && isHomeMatch)
            {
                if ((matchViewModel.Team1Ft > 0))
                    homeScoringRun.ItemCount++;
                else
                    homeScoringRun.HasItem = false;
            }
        }

        private void ProcessAwayScoringRun(StatisticItemViewModel awayScoringRun, BaseMatchViewModel matchViewModel, bool isHomeMatch)
        {
            if (awayScoringRun.HasItem && !isHomeMatch)
            {
                if ((matchViewModel.Team2Ft > 0))
                    awayScoringRun.ItemCount++;
                else
                    awayScoringRun.HasItem = false;
            }
        }
    }

    public static class BaseTeamViewModelExtensions
    {
        public static void SetFormData(this IEnumerable<BaseTeamViewModel> teamViewModels, int matchCount = 10)
        {
            teamViewModels._ForEach(f => f.SetFormData(matchCount));
        }
    }
}