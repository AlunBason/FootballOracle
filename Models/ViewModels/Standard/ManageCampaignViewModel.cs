using System;
using System.ComponentModel.DataAnnotations;
using FootballOracle.Models.Attributes;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class ManageCampaignViewModel
    {
        public Guid CampaignKey { get; set; }
        public Guid CompetitionKey { get; set; }

        [HideLabel]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [HideLabel]
        public DateTime? EndDate { get; set; }

        public bool IsNew { get; set; }
    }
}
