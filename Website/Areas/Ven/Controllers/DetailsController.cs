using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Venues;
using FootballOracle.Website.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FootballOracle.Website.Areas.Ven.Controllers
{
    [RouteArea("Ven")]
    [RoutePrefix("Details")]
    public class DetailsController : BaseVenueController
    {
        #region Constructor
        public DetailsController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region Index
        [Route("{hk}/{dt?}")]
        public ActionResult Index(string hk, DateTime? dt)
        {
            if (dt != null && dt != DateTime.Today)
                return RedirectToAction("Summary", new { hk = hk, dt = ((DateTime)dt).ToUrlString() });
            else
                return RedirectToAction("Summary", new { hk = hk });
        }
        #endregion

        #region Summary
        [Route("Summary/{hk}/{dt?}")]
        public async Task<ActionResult> Summary(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<VenueSummaryViewModel>(hk, dt);
            await viewModel.SetMatchViewModels();
            viewModel.SetSelectedDateRangePickerViewModel();

            return View(viewModel);
        }
        #endregion
    }
}