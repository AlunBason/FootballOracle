using System;
using System.Collections.Generic;
using FootballOracle.Models.ViewModels.Standard;

namespace FootballOracle.Models.ViewModels.Approvable.Competitions
{
    public class CompetitionLookupCampaignStagesViewModel : BaseCompetitionViewModel
    {
        public Guid SelectedCampaignStageKey { get; set; }
        public ManageCampaignStageViewModel SelectedCampaignStageViewModel { get; set; }
        public List<ManageLookupCampaignStageViewModel> ManageLookupCampaignStageViewModels { get; set; }
    }
}
