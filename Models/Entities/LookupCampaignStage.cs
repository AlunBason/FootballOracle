using FootballOracle.Foundation;
using FootballOracle.Foundation.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.Entities
{
    public class LookupCampaignStage : BaseEntity
    {
        public ImportSite ImportSite { get; set; }

        public Guid CampaignStageKey { get; set; }

        [StringLength(100)]
        public string LookupId { get; set; }

        #region Navigation properties
        //public virtual Campaign Campaign { get; set; }
        public virtual CampaignStage CampaignStage { get; set; }
        #endregion
    }
}
