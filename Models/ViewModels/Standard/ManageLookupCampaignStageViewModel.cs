using System;
using FootballOracle.Foundation;
using FootballOracle.Models.Attributes;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class ManageLookupCampaignStageViewModel
    {
        public Guid PrimaryKey { get; set; }
        public Guid CampaignStageKey { get; set; }

        public ImportSite? ImportSite { get; set; }

        [HideLabel]
        public string LookupId { get; set; }

        public bool IsNew { get; set; }
    }
}
