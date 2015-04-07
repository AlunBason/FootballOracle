using FootballOracle.Models.ViewModels.Approvable.Matches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballOracle.Models.ViewModels.Approvable.Teams
{
    public class TeamSummaryViewModel : BaseTeamViewModel
    {
        public IEnumerable<BaseMatchViewModel> CampaignMatches { get; private set; }

        public async Task SetCampaignMatches()
        {
            if (SelectedCampaignViewModel == null)
                CampaignMatches = new List<BaseMatchViewModel>();

            var startDate = SelectedCampaignViewModel.Entity.StartDate;
            var endDate = SelectedCampaignViewModel.Entity.EndDate;

            CampaignMatches = await DbProvider.GetTeamMatchesByDate(startDate, endDate, HeaderKey, ViewDate);
        }

        private IEnumerable<IGrouping<int, BaseMatchViewModel>> campaignMatchGroups;
        public IEnumerable<IGrouping<int, BaseMatchViewModel>> CampaignMatchGroups
        {
            get { return campaignMatchGroups = campaignMatchGroups ?? CampaignMatches.GroupBy(m => m.VersionEntity.MatchDate.Year * 100 + m.VersionEntity.MatchDate.Month);}
        }
    }
}
