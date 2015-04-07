using System;
using System.Collections.Generic;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using System.Linq;
using FootballOracle.Models.ViewModels.Approvable.Teams;

namespace FootballOracle.Models.ViewModels.Standard.Campaigns
{
    public class BaseCampaignStageViewModel
    {
        public CampaignStage Entity { get; set; }
        public DateTime ViewDate { get; set; }
        public int ViewType { get; set; }

        private IEnumerable<BaseMatchViewModel> matchViewModels;
        public IEnumerable<BaseMatchViewModel> MatchViewModels
        {
            get
            {
                if (matchViewModels == null)
                    matchViewModels = Entity.MatchVs.ToViewModels(ViewDate.ToEndOfDay());

                return matchViewModels;
            }
        }

        private IEnumerable<LeagueTableItemViewModel> leagueTableItemViewModels;
        public IEnumerable<LeagueTableItemViewModel> LeagueTableItemViewModels
        {
            get
            {
                leagueTableItemViewModels = MatchViewModels.GetStandings(ViewType, ViewDate);
                leagueTableItemViewModels.Select(s => s.TeamViewModel).SetFormData();

                return leagueTableItemViewModels;
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

        public IEnumerable<IGrouping<DateTime?, BaseMatchViewModel>> FixtureGroups
        {
            get { return FixtureMatchViewModels.OrderBy(m => m.MatchDate).ThenBy(m => m.ToString()).GroupBy(m => (DateTime?)m.VersionEntity.MatchDate); }
        }

        public override string ToString()
        {
            return Entity.Description;
        }
    }
}
