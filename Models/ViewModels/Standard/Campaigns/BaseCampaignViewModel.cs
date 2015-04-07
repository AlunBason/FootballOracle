using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Competitions;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballOracle.Models.ViewModels.Standard.Campaigns
{
    public class BaseCampaignViewModel
    {
        public Campaign Entity { get; set; }
        public DateTime ViewDate { get; set; }

        public IEnumerable<BaseCampaignStageViewModel> CampaignStageViewModels { get; set; }
        public BaseCampaignStageViewModel SelectedCampaignStageViewModel { get; set; }

        public void SetCampaignStages(int viewType = 3, Guid? campaignStageKey = null)
        {
            CampaignStageViewModels = Entity.CampaignStages.OrderByDescending(o => o.IsDefault).ThenByDescending(t => t.StageOrder).ThenBy(t => t.Description).ToViewModels(ViewDate);
            
            if (!CampaignStageViewModels.Any())
                return;

            if (campaignStageKey != null)
                SelectedCampaignStageViewModel = CampaignStageViewModels.Single(s => s.Entity.PrimaryKey == campaignStageKey);
            else
            {
                foreach (var item in CampaignStageViewModels)
                {
                    if (item.MatchViewModels.Any())
                    {
                        SelectedCampaignStageViewModel = item;
                        break;
                    }
                }
            }

            SelectedCampaignStageViewModel.ViewType = viewType;
        }

        public int ResultGroupsPerPage
        {
            get { return 3; }
        }

        public int FixtureGroupsPerPage
        {
            get { return 3; }
        }

        public int ViewType { get; set; }
        public int MatchCount { get; set; }

        private IEnumerable<BaseMatchViewModel> matchViewModels;
        public IEnumerable<BaseMatchViewModel> MatchViewModels
        {
            get 
            { 
                if (matchViewModels == null)
                {
                    matchViewModels = new List<BaseMatchViewModel>();

                    foreach (var item in CampaignStageViewModels)
                        matchViewModels = matchViewModels.Concat(item.MatchViewModels);
                }

                return matchViewModels;
            }
        }

        private IEnumerable<BaseMatchViewModel> resultMatchViewModels;
        public IEnumerable<BaseMatchViewModel> ResultMatchViewModels
        {
            get { return resultMatchViewModels = resultMatchViewModels ?? MatchViewModels.Where(m => m.VersionEntity.Team1FT != null && m.VersionEntity.Team2FT != null); }
        }
        
        public int ResultsPage { get; set; }

        private IEnumerable<BaseMatchViewModel> fixtureMatchViewModels;
        public IEnumerable<BaseMatchViewModel> FixtureMatchViewModels
        {
            get { return fixtureMatchViewModels = fixtureMatchViewModels ?? MatchViewModels.Where(m => m.VersionEntity.Team1FT == null && m.VersionEntity.Team2FT == null && m.MatchDate >= DateTime.Today); }
        }

        public IEnumerable<IGrouping<DateTime?, BaseMatchViewModel>> ResultGroups
        {
            get { return ResultMatchViewModels.OrderByDescending(m => m.MatchDate).ThenBy(m => m.ToString()).GroupBy(m => (DateTime?)m.VersionEntity.MatchDate); }
        }

        public int ResultsPages
        {
            get { return ResultMatchViewModels != null ? (ResultGroups.Count() + ResultGroupsPerPage - 1) / ResultGroupsPerPage : 0; }
        }

        public IEnumerable<IGrouping<DateTime?, BaseMatchViewModel>> FixtureGroups
        {
            get { return FixtureMatchViewModels.OrderBy(m => m.MatchDate).ThenBy(m => m.ToString()).GroupBy(m => (DateTime?)m.VersionEntity.MatchDate); }
        }

        public int FixturesPages
        {
            get { return FixtureMatchViewModels != null ? (FixtureGroups.Count() + ResultGroupsPerPage - 1) / ResultGroupsPerPage : 0; }
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", Entity.StartDate.Year.ToString(), Entity.EndDate.Year.ToString());
        }
        
        public string CampaignDisplayWithCompetition
        {
            get { return string.Format("{0} {1}/{2}", Entity.Competition.GetApprovedVersion<CompetitionV>(Entity.StartDate).CompetitionName,  Entity.StartDate.Year.ToString(), Entity.EndDate.Year.ToString()); }
        }

        public BaseCompetitionViewModel GetApprovedCompetitionViewModel(DateTime viewDate)
        {
            return Entity.Competition.ToViewModel(viewDate);
        }
    }

    public static class _BaseCampaignViewModelExtensions
    {
        public static IEnumerable<PersonTableItemViewModel> GetPersonTableItemViewModels(this BaseCampaignViewModel campaignViewModel, DateTime viewDate)
        {
            var personTableItemViewModels = new List<PersonTableItemViewModel>();

            var matchEventViewModels = campaignViewModel.MatchViewModels.Select(m => m.VersionEntity.MatchEvents.ToViewModels(viewDate));

            foreach (var matchEventViewModel in matchEventViewModels)
                matchEventViewModel._ForEach(me => personTableItemViewModels.UpdatePersonTableItemViewModels(me, viewDate));

            personTableItemViewModels.Sort();
            return personTableItemViewModels;
        }

        public static IEnumerable<PersonTableItemViewModel> GetPersonTableItemViewModels(this BaseCampaignViewModel campaignViewModel, Guid teamGuid, DateTime viewDate)
        {
            var personTableItemViewModels = new List<PersonTableItemViewModel>();

            //var matchViewModels = campaignViewModel.Entity..MatchVs.Where(m => m.Team1Guid == teamGuid || m.Team2Guid == teamGuid).ToViewModels(viewDate);

            //var matchEventViewModels = matchViewModels.Select(m => m.VersionEntity.MatchEvents.Where(me => me.TeamPrimaryKey == teamGuid).ToViewModels(viewDate));

            //foreach (var matchEventsViewModel in matchEventViewModels)
            //    matchEventsViewModel._ForEach(me => personTableItemViewModels.UpdatePersonTableItemViewModels(me, viewDate));

            //personTableItemViewModels.Sort();
            return personTableItemViewModels;
        }

        public static void UpdatePersonTableItemViewModels(this List<PersonTableItemViewModel> personTableItemViewModels, MatchEventViewModel matchEventViewModels, DateTime viewDate)
        {
            var personTableItemViewModel = personTableItemViewModels.SingleOrDefault(p => p.PersonViewModel.HeaderKey == matchEventViewModels.Entity.PersonPrimaryKey);

            if (personTableItemViewModel == null)
            {
                personTableItemViewModels.Add(new PersonTableItemViewModel()
                {
                    PersonViewModel = matchEventViewModels.PersonViewModel,
                    Started = matchEventViewModels.Entity.MatchEventType == MatchEventType.Started ? 1 : 0,
                    BroughtOn = matchEventViewModels.Entity.MatchEventType == MatchEventType.BroughtOn ? 1 : 0,
                    TakenOff = matchEventViewModels.Entity.MatchEventType == MatchEventType.TakenOff ? 1 : 0,
                    Goals = matchEventViewModels.Entity.MatchEventType == MatchEventType.Scored ? 1 : 0,
                    OwnGoals = matchEventViewModels.Entity.MatchEventType == MatchEventType.OwnGoal ? 1 : 0,
                    Booked = matchEventViewModels.Entity.MatchEventType == MatchEventType.Booked ? 1 : 0,
                    SentOff = matchEventViewModels.Entity.MatchEventType == MatchEventType.SentOff ? 1 : 0
                });
            }
            else
            {
                personTableItemViewModel.Started += matchEventViewModels.Entity.MatchEventType == MatchEventType.Started ? 1 : 0;
                personTableItemViewModel.BroughtOn += matchEventViewModels.Entity.MatchEventType == MatchEventType.BroughtOn ? 1 : 0;
                personTableItemViewModel.TakenOff += matchEventViewModels.Entity.MatchEventType == MatchEventType.TakenOff ? 1 : 0;
                personTableItemViewModel.Goals += matchEventViewModels.Entity.MatchEventType == MatchEventType.Scored ? 1 : 0;
                personTableItemViewModel.OwnGoals += matchEventViewModels.Entity.MatchEventType == MatchEventType.OwnGoal ? 1 : 0;
                personTableItemViewModel.Booked += matchEventViewModels.Entity.MatchEventType == MatchEventType.Booked ? 1 : 0;
                personTableItemViewModel.SentOff += matchEventViewModels.Entity.MatchEventType == MatchEventType.SentOff ? 1 : 0;
            }
        }
    }
}