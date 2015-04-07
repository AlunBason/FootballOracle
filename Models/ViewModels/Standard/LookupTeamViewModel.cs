using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballOracle.Foundation;
using FootballOracle.Models.Attributes;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class LookupTeamViewModel
    {
        public Guid PrimaryKey { get; set; }
        
        public ImportSite? ImportSite { get; set; }

        public Guid TeamGuid { get; set; }

        [HideLabel]
        public string LookupId { get; set; }
    }
}
