using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Website.Attributes;
using FootballOracle.Website.Controllers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using FootballOracle.Models.ViewModels.Standard.Admin;
using FootballOracle.Services.Import;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;

namespace FootballOracle.Website.Areas.Adm.Controllers
{
    [RouteArea("Adm")]
    [RoutePrefix("Home")]
    [AuthorizeAdmin]
    public class HomeController : BaseController
    {
        #region Constructor
        public HomeController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        // GET: Adm/Home
        [Route]
        public ActionResult Index()
        {
            return View();
        }

        [Route("UnresolvedMatches")]
        public async Task<ActionResult> UnresolvedMatches()
        {
            var matchViewModels = await DbProvider.GetUnresolvedMatches();

            return View(matchViewModels);
        }

        [Route("DeleteMatch/{hk}")]
        public async Task<ActionResult> DeleteMatch(string hk)
        {
            var matchKey = hk.ToGuid();
            var match = await DbProvider.GetMatch(matchKey);

            if (match != null)
            {
                for (int i = match.LookupMatches.Count - 1; i >= 0; i--)
                    DbProvider.Remove(match.LookupMatches.ElementAt(i));

                for (int i = match.Versions.Count - 1; i >= 0; i--)
                {
                    var matchV = match.Versions.ElementAt(i);

                    for (int j = matchV.MatchEvents.Count - 1; j >= 0; j--)
                        DbProvider.Remove(matchV.MatchEvents.ElementAt(j));

                    DbProvider.Remove(match.Versions.ElementAt(i));
                }

                DbProvider.Remove(match);

                DbProvider.SaveChanges();
            }

            return RedirectToAction("UnresolvedMatches");
        }

        [Route("DeleteAndImportFixtures")]
        public async Task<ActionResult> DeleteAndImportFixtures()
        {
            var viewModel = new DeleteAndImportFixturesViewModel();

            viewModel.CampaignViewModels = await DbProvider.GetEspnCampaigns();

            return View(viewModel);
        }

        [Route("DeleteAndImportFixtures")]
        [HttpPost]
        public async Task<ActionResult> DeleteAndImportFixtures(DeleteAndImportFixturesViewModel viewModel)
        {
            viewModel.CampaignViewModels = await DbProvider.GetEspnCampaigns();

            foreach (var campaignViewModel in viewModel.CampaignViewModels)
            {
                foreach (var campaignStageViewModel in campaignViewModel.CampaignStageViewModels)
                {
                    //delete unresolved matches 
                    var matchVs = campaignStageViewModel.Entity.MatchVs.Where(w => w.Team1FT == null && w.Team2FT == null);

                    for (var index = matchVs.Count() - 1; index >= 0; index--)
                        DeleteMatch(matchVs.ElementAt(index).Match);

                    var espn = new Espn(DbProvider, User);

                    var competitionViewModel = campaignViewModel.Entity.Competition.ToViewModel(DateTime.Now);

                    await espn.Import(competitionViewModel.VersionEntity, true, false, DateTime.Today);
                }
            }

            DbProvider.SaveChanges();

            return View(viewModel);
        }

        private void DeleteMatch(Match match)
        {
            for (var index = match.LookupMatches.Count() - 1; index >= 0; index--)
                DbProvider.Remove(match.LookupMatches.ElementAt(index));

            for (var index = match.Versions.Count() - 1; index >= 0; index--)
                DbProvider.Remove(match.Versions.ElementAt(index));

            DbProvider.Remove(match);
        }
    }
}