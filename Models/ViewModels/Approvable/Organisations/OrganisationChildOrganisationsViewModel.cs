using System.Collections.Generic;

namespace FootballOracle.Models.ViewModels.Approvable.Organisations
{
    public class OrganisationChildOrganisationsViewModel : BaseOrganisationViewModel
    {
        public IEnumerable<BaseOrganisationViewModel> ChildOrganisationViewModels { get; set; }
    }
}
