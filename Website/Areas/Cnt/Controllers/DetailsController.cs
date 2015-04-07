using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using FootballOracle.Website.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FootballOracle.Website.Areas.Cnt.Controllers
{
    [RouteArea("Cnt")]
    [RoutePrefix("Details")]
    public class DetailsController : BaseCountryController
    {
        #region Constructor
        public DetailsController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region Index
        [Route("{hk}/{dt?}")]
        public async Task<ActionResult> Index(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<BaseCountryViewModel>(hk, dt);

            if (viewModel.HasChildOrganisations)
                return ApprovableRedirect("Organisations", hk, dt);

            if (viewModel.HasChildTeams)
                return ApprovableRedirect("Teams", hk, dt);

            if (viewModel.HasChildVenues)
                return ApprovableRedirect("Venues", hk, dt);

            return ApprovableRedirect("Summary", hk, dt);
        }
        #endregion

        #region Organisations
        [Route("Organisations/{hk}/{dt?}")]
        public async Task<ActionResult> Organisations(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<CountryOrganisationsViewModel>(hk, dt);
            viewModel.ChildOrganisationViewModels = await DbProvider.GetOrganisationsByCountry(viewModel.HeaderKey, viewModel.ViewDate);

            return View(viewModel);
        }
        #endregion

        #region Teams
        [Route("Teams/{hk}/{dt?}/{sl?}")]
        public async Task<ActionResult> Teams(string hk, DateTime? dt, char? sl)
        {
            var viewModel = await SetModels<CountryTeamsViewModel>(hk, dt);
            var childTeamViewModels = await DbProvider.GetCountryTeamViewModels(viewModel.HeaderKey, viewModel.ViewDate);

            if (childTeamViewModels.Count() > 25)
            {
                viewModel.SelectedInitialLetter = !String.IsNullOrWhiteSpace(sl.ToString()) ? sl.ToString().ToUpper()[0] : 'A';
                viewModel.InitialLetters = childTeamViewModels.Select(c => c.ToString().ToUpper().ElementAt(0)).Distinct();
                viewModel.ChildTeamViewModels = childTeamViewModels.Where(c => c.ToString()[0] == viewModel.SelectedInitialLetter);
            }
            else
                viewModel.ChildTeamViewModels = childTeamViewModels;

            return View(viewModel);
        }
        #endregion

        #region Venues
        [Route("Venues/{hk}/{dt?}/{sl?}")]
        public async Task<ActionResult> Venues(string hk, DateTime? dt, char? sl)
        {
            var viewModel = await SetModels<CountryVenuesViewModel>(hk, dt);
            var childVenueViewModels = await DbProvider.GetCountryVenueViewModels(viewModel.HeaderKey, viewModel.ViewDate);

            if (childVenueViewModels.Count() > 25)
            {
                viewModel.SelectedInitialLetter = !String.IsNullOrWhiteSpace(sl.ToString()) ? sl.ToString().ToUpper()[0] : 'A';
                viewModel.InitialLetters = childVenueViewModels.Select(c => c.ToString().ToUpper().ElementAt(0)).Distinct();
                viewModel.ChildVenueViewModels = childVenueViewModels.Where(c => c.ToString()[0] == viewModel.SelectedInitialLetter);
            }
            else
                viewModel.ChildVenueViewModels = childVenueViewModels;

            return View(viewModel);
        }
        #endregion

        #region Summary
        [Route("Summary/{hk}/{dt?}")]
        public async Task<ActionResult> Summary(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<BaseCountryViewModel>(hk, dt);

            return View(viewModel);
        }
        #endregion
    }
}