using System.Collections.Generic;
using FootballOracle.Models.ViewModels.Standard.Campaigns;
using FootballOracle.Models.ViewModels.Standard;

namespace FootballOracle.Models.ViewModels.Approvable.Teams
{
    public class TeamPlayersViewModel : BaseTeamViewModel
    {
        public IEnumerable<PersonTableItemViewModel> PersonTableItemViewModels
        {
            get { return SelectedCampaignViewModel.GetPersonTableItemViewModels(HeaderKey, ViewDate); }
        }
    }
}
