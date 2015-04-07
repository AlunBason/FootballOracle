using System.Collections.Generic;
using FootballOracle.Models.ViewModels.Standard.Campaigns;

namespace FootballOracle.Models.ViewModels.Standard.Admin
{
    public class DeleteAndImportFixturesViewModel
    {
        public IEnumerable<BaseCampaignViewModel> CampaignViewModels { get; set; }
    }
}
