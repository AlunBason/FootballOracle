using FootballOracle.Foundation.Entities;
using FootballOracle.Models.ViewModels.Standard.Campaigns;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.Entities
{
    public class CampaignStage : BaseEntity
    {
        public Guid CampaignKey { get; set; }

        public int StageOrder { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        public bool IsDefault { get; set; }
        public bool IsLeague { get; set; }
        public int LegCount { get; set; }

        #region Navigation properties
        public virtual Campaign Campaign { get; set; }
        public virtual ICollection<MatchV> MatchVs { get; set; }
        public virtual ICollection<LookupCampaignStage> LookupCampaignStages { get; set; }
        #endregion
    }

    public static class CampaignStageExtensions
    {
        public static BaseCampaignStageViewModel ToViewModel(this CampaignStage campaignStage, DateTime viewDate)
        {
            return new BaseCampaignStageViewModel()
            {
                Entity = campaignStage,
                ViewDate = viewDate
            };
        }

        public static IEnumerable<BaseCampaignStageViewModel> ToViewModels(this IEnumerable<CampaignStage> campaignStages, DateTime viewDate)
        {
            foreach (var campaignStage in campaignStages)
                yield return campaignStage.ToViewModel(viewDate);
        }
    }
}
