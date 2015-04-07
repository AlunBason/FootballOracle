using System.Collections.Generic;
using FootballOracle.Models.ViewModels.Standard.Campaigns;

namespace FootballOracle.Models.ViewModels.Approvable.Competitions
{
    public class CompetitionCampaignsViewModel : BaseCompetitionViewModel
    {
        public IEnumerable<BaseCampaignViewModel> ChildCampaignViewModels { get; set; }
    }
}
