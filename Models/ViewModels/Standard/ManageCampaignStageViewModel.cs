using System;
using FootballOracle.Models.Attributes;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class ManageCampaignStageViewModel
    {
        public Guid PrimaryKey { get; set; }
        public Guid CampaignKey { get; set; }

        [HideLabel]
        public string Description { get; set; }

        [HideLabel]
        public int StageOrder { get; set; }

        [HideLabel]
        public bool IsDefault { get; set; }

        [HideLabel]
        public bool IsLeague { get; set; }

        [HideLabel]
        public int LegCount { get; set; }

        public bool IsNew { get; set; }
    }
}
