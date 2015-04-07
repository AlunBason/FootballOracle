using System.Collections.Generic;
using FootballOracle.Models.ViewModels.Approvable.Competitions;

namespace FootballOracle.Models.ViewModels.Approvable.Organisations
{
    public class OrganisationCompetitionsViewModel : BaseOrganisationViewModel
    {
        public IEnumerable<BaseCompetitionViewModel> ChildCompetitionViewModels { get; set; }
    }
}
