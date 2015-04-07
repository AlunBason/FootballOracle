using System.Collections.Generic;
using FootballOracle.Models.ViewModels.Standard;

namespace FootballOracle.Models.ViewModels.Approvable.Competitions
{
    public class CompetitionManageCampaignsViewModel : BaseCompetitionViewModel
    {
        public List<ManageCampaignViewModel> ManageCampaignViewModels { get; set; }
    }
}
