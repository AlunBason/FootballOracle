using System;
using System.Collections.Generic;
using FootballOracle.Models.ViewModels.Standard;

namespace FootballOracle.Models.ViewModels.Approvable.Competitions
{
    public class CompetitionCampaignStagesViewModel : BaseCompetitionViewModel
    {
        public Guid SelectedCampaignKey { get; set; }
        public ManageCampaignViewModel ManageCampaignViewModel { get; set; }
        public List<ManageCampaignStageViewModel> ManageCampaignStageViewModels { get; set; }
    }
}
