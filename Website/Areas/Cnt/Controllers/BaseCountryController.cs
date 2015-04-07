using System;
using System.Threading.Tasks;
using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using FootballOracle.Website.Controllers;
using System.Linq;
using FootballOracle.Models.Interfaces;

namespace FootballOracle.Website.Areas.Cnt.Controllers
{
    public abstract class BaseCountryController : ApprovableController<Country, CountryV>
    {
        #region Constructor
        public BaseCountryController(IRepositoryProvider provider)
            : base(provider, AreaType.Cnt)
        {
        }
        #endregion

        protected override async Task<TViewModel> GetViewModel<TViewModel>(Guid headerKey, DateTime viewDate)
        {
            return (await DbProvider.GetCountry(headerKey, viewDate)).ToViewModel<TViewModel, Country, CountryV>(viewDate);
        }

        protected override async Task<TViewModel> GetViewModel<TViewModel>(Guid primaryKey, Guid headerKey)
        {
            return (await DbProvider.GetCountry(primaryKey, headerKey)).ToViewModel<TViewModel, Country, CountryV>(DateTime.Now); ;
        }

        protected override async Task SetTabVisibility(IApprovableViewModel<Country, CountryV> viewModel)
        {
            ((BaseCountryViewModel)viewModel).HasChildOrganisations = (await DbProvider.GetOrganisationsByCountry(viewModel.HeaderKey, viewModel.ViewDate)).Any();
            ((BaseCountryViewModel)viewModel).HasChildTeams = (await DbProvider.GetCountryTeamViewModels(viewModel.HeaderKey, viewModel.ViewDate)).Any();
            ((BaseCountryViewModel)viewModel).HasChildVenues = (await DbProvider.GetCountryVenueViewModels(viewModel.HeaderKey, viewModel.ViewDate)).Any();
        }
    }
}