using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Organisations;
using FootballOracle.Website.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FootballOracle.Website.Areas.Org.Controllers
{
    [RouteArea("Org")]
    [RoutePrefix("Details")]
    public class DetailsController : BaseOrganisationController
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
            var viewModel = await SetModels<BaseOrganisationViewModel>(hk, dt);

            if (viewModel.HasChildCountries)
                return ApprovableRedirect("Countries", hk, dt);

            if (viewModel.HasChildCompetitions)
                return ApprovableRedirect("Competitions", hk, dt);

            if (viewModel.HasChildOrganisations)
                return ApprovableRedirect("Organisations", hk, dt);

            return ApprovableRedirect("Competitions", hk, dt);
        }
        #endregion

        #region Competitions
        [Route("Competitions/{hk}/{dt?}")]
        public async Task<ActionResult> Competitions(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<OrganisationCompetitionsViewModel>(hk, dt);
            viewModel.ChildCompetitionViewModels = (await DbProvider.GetCompetitionsByOrganisationAsync(viewModel.HeaderKey, viewModel.ViewDate))
                .OrderByDescending(c => c.EffectiveTo)
                .ThenBy(c => c.ToString());

            return View(viewModel);
        }
        #endregion

        #region Countries
        [Route("Countries/{hk}/{dt?}/{sl?}")]
        public async Task<ActionResult> Countries(string hk, DateTime? dt, char? sl)
        {
            var viewModel = await SetModels<OrganisationCountriesViewModel>(hk, dt);

            var childCountryViewModels = (await DbProvider.GetCountriesByOrganisationAsync(viewModel.HeaderKey, viewModel.ViewDate))
                .OrderByDescending(c => c.EffectiveTo)
                .ThenBy(c => c.ToString());

            if (childCountryViewModels.Count() > 25)
            {
                viewModel.SelectedInitialLetter = !String.IsNullOrWhiteSpace(sl.ToString()) ? sl.ToString().ToUpper()[0] : 'A';
                viewModel.InitialLetters = childCountryViewModels.Select(c => c.ToString().ToUpper().ElementAt(0)).Distinct();
                viewModel.ChildCountryViewModels = childCountryViewModels.Where(c => c.ToString()[0] == viewModel.SelectedInitialLetter);
            }
            else
                viewModel.ChildCountryViewModels = childCountryViewModels;

            return View(viewModel);
        }
        #endregion

        #region Organisations
        [Route("Organisations/{hk}/{dt?}")]
        public async Task<ActionResult> Organisations(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<OrganisationChildOrganisationsViewModel>(hk, dt);

            var organisations = await DbProvider.GetChildOrganisations(viewModel.HeaderKey, viewModel.ViewDate);

            viewModel.ChildOrganisationViewModels = organisations.OrderByDescending(o => o.EffectiveTo).ThenBy(t => t.ToString());

            return View(viewModel);
        }
        #endregion

        #region Versions
        [Route("Versions/{hk}/{dt?}")]
        public async Task<ActionResult> Versions(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<BaseOrganisationViewModel>(hk, dt);

            return View(viewModel);
        }
        #endregion

        #region Abstract overrides
        
        #endregion
    }
}