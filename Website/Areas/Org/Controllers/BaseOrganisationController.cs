using System;
using System.Linq;
using System.Threading.Tasks;
using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Organisations;
using FootballOracle.Website.Controllers;
using FootballOracle.Models.Interfaces;

namespace FootballOracle.Website.Areas.Org.Controllers
{
    public abstract class BaseOrganisationController : ApprovableController<Organisation, OrganisationV>
    {
        #region Constructor
        public BaseOrganisationController(IRepositoryProvider provider)
            : base(provider, AreaType.Org)
        {
        }
        #endregion

        protected override async Task<TViewModel> GetViewModel<TViewModel>(Guid headerKey, DateTime viewDate)
        {
            return (await DbProvider.GetOrganisation(headerKey, viewDate)).ToViewModel<TViewModel, Organisation, OrganisationV>(viewDate);
        }

        protected override async Task<TViewModel> GetViewModel<TViewModel>(Guid primaryKey, Guid headerKey)
        {
            return (await DbProvider.GetOrganisation(primaryKey, headerKey)).ToViewModel<TViewModel, Organisation, OrganisationV>(DateTime.Now); ;
        }

        protected override async Task SetTabVisibility(IApprovableViewModel<Organisation, OrganisationV> viewModel)
        {
            ((BaseOrganisationViewModel)viewModel).HasChildCompetitions = (await DbProvider.GetCompetitionsByOrganisationAsync(viewModel.HeaderKey, viewModel.ViewDate)).Any();
            ((BaseOrganisationViewModel)viewModel).HasChildCountries = (await DbProvider.GetCountriesByOrganisationAsync(viewModel.HeaderKey, viewModel.ViewDate)).Any();
            ((BaseOrganisationViewModel)viewModel).HasChildOrganisations = (await DbProvider.GetChildOrganisations(viewModel.HeaderKey, viewModel.ViewDate)).Any();
        }
    }
}