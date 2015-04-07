using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Standard;
using FootballOracle.Services.Import;
using FootballOracle.Website.Attributes;
using FootballOracle.Website.Controllers;
using System;
using System.Web.Mvc;

namespace FootballOracle.Website.Areas.Mtc.Controllers
{
    [RouteArea("Mtc")]
    [RoutePrefix("Admin")]
    [AuthorizeAdmin]
    public class AdminController : BaseController
    {
        #region Constructor
        public AdminController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region ImportMatchDetails
        [Route("ImportMatchDetails/{sd?}/{ed?}")]
        public ActionResult ImportMatchDetails(DateTime? sd, DateTime? ed)
        {
            var importMatchDetailsViewModel = new ImportMatchDetailsViewModel()
            {
                StartDate = sd ?? DateTime.Today.AddDays(-1),
                EndDate = ed ?? DateTime.Today.AddDays(-1)
            };

            return View(importMatchDetailsViewModel);
        }

        [Route("ImportMatchDetails")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ImportMatchDetails(ImportMatchDetailsViewModel importMatchDetailsViewModel)
        {
            var soccerbase = new Soccerbase(DbProvider, User);
            soccerbase.LogFilePath = this.LogFilePath;

            soccerbase.Import(importMatchDetailsViewModel.StartDate, importMatchDetailsViewModel.EndDate);

            return RedirectToAction("ImportMatchDetails", new { sd = importMatchDetailsViewModel.StartDate.ToUrlString(), ed = importMatchDetailsViewModel.EndDate.ToUrlString() });
        }
        #endregion
    }
}