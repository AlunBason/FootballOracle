using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.People;
using FootballOracle.Website.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FootballOracle.Website.Areas.Ppl.Controllers
{
    [RouteArea("Ppl")]
    [RoutePrefix("Details")]
    public class DetailsController : BasePersonController
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
                return RedirectToAction("Summary", new { hk = hk, cg = " ", dt = ((DateTime)dt).ToUrlString() });
            else
                return RedirectToAction("Summary", new { hk = hk });
        }
        #endregion

        #region Summary
        [Route("Summary/{hk}/{dt?}")]
        public async Task<ActionResult> Summary(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<PersonSummaryViewModel>(hk, dt);

            await viewModel.SetMatchEventViewModels();

            return View(viewModel);
        }
        #endregion
	}
}