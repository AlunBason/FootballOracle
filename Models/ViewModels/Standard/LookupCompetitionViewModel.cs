using FootballOracle.Foundation;
using FootballOracle.Models.Attributes;
using System;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class LookupCompetitionViewModel
    {
        public Guid PrimaryKey { get; set; }
        
        public ImportSite? ImportSite { get; set; }

        public Guid CompetitionGuid { get; set; }

        [HideLabel]
        public string LookupId { get; set; }
    }
}
