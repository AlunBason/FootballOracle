using System;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.ViewModels.Standard.Campaigns
{
    public class ImportCampaignViewModel
    {
        public ImportCampaignViewModel()
        {
            IncludeFixtures = true;
            IncludeResults = true;
        }

        [ScaffoldColumn(true)]
        public Guid HeaderKey { get; set; }

        [Display(Name = "Include fixtures")]
        public bool IncludeFixtures { get; set; }

        [Display(Name = "Include results")]
        public bool IncludeResults { get; set; }

        public DateTime ImportDate { get; set; }
    }
}
