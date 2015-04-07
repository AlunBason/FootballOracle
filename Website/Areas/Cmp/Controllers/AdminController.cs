using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Competitions;
using FootballOracle.Models.ViewModels.Standard;
using FootballOracle.Models.ViewModels.Standard.Campaigns;
using FootballOracle.Services.Import;
using FootballOracle.Website.Attributes;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using FootballOracle.Foundation;

namespace FootballOracle.Website.Areas.Cmp.Controllers
{
    [RouteArea("Cmp")]
    [RoutePrefix("Admin")]
    [AuthorizeAdmin]
    public class AdminController : BaseCompetitionController
    {
        #region Constructor
        public AdminController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region Index
        // GET: Competitions/Admin
        [Route("{hk}/{dt?}")]
        public async Task<ActionResult> Index(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<BaseCompetitionViewModel>(hk, dt);

            viewModel.SetCampaigns();

            return View(viewModel);
        }
        #endregion

        #region Import
        // GET: /Competitions/Details/Import
        [Route("Import/{hk}/{dt?}")]
        public async Task<ActionResult> Import(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<CompetitionImportViewModel>(hk, dt);

            viewModel.ImportCampaignViewModel = new ImportCampaignViewModel()
            {
                HeaderKey = viewModel.HeaderKey,
                ImportDate = viewModel.ViewDate
            };

            viewModel.SetCampaigns();

            return View(viewModel);
        }
        
        [Route("Import/{hk}/{dt?}")]
        [HttpPost]
        public async Task<ActionResult> Import(CompetitionImportViewModel viewModel)
        {
            var competitionImportViewModel = await SetModels<CompetitionImportViewModel>(viewModel.ImportCampaignViewModel.HeaderKey.ToShortGuid(), viewModel.ImportCampaignViewModel.ImportDate);
            //competitionImportViewModel.SelectedCampaignViewModel = competitionImportViewModel.ToBaseCampaignViewModel();
            competitionImportViewModel.ImportCampaignViewModel = viewModel.ImportCampaignViewModel;

            ModelState.Remove("CompetitionName");
            if (!ModelState.IsValid)
                return View(competitionImportViewModel);

            var importCampaignViewModel = viewModel.ImportCampaignViewModel;
            var espn = new Espn(DbProvider, User);
            await espn.Import(competitionImportViewModel.VersionEntity, importCampaignViewModel.IncludeFixtures, importCampaignViewModel.IncludeResults, viewModel.ImportCampaignViewModel.ImportDate);

            return RedirectToAction("Import", importCampaignViewModel.HeaderKey);
        }
        #endregion

        #region MatchValidation
        // GET: /Competitions/Details/MatchValidation
        [Route("MatchValidation/{hk}/{dt?}")]
        public async Task<ActionResult> MatchValidation(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<CompetitionMatchValidationViewModel>(hk, dt);

            viewModel.SetCampaigns();

            viewModel.MatchesWithoutVenues = await DbProvider.GetMatchesWithoutVenues(viewModel.SelectedCampaignViewModel.Entity.PrimaryKey, viewModel.ViewDate);
            viewModel.MatchDatesWithoutEvents = await DbProvider.GetMatchDatesWithoutEvents(viewModel.SelectedCampaignViewModel.Entity.PrimaryKey);

            return View(viewModel);
        }

        [Route("MatchValidation/{hk}/{dt?}")]
        [HttpPost]
        public async Task<ActionResult> MatchValidation(CompetitionMatchValidationViewModel viewModel)
        {
            await SetModelsByPrimaryKey(viewModel);

            viewModel.SetCampaigns();

            if (viewModel.FixMatchesWithoutVenues)
            {
                var matchesWithoutVenues = await DbProvider.GetMatchesWithoutVenues(viewModel.SelectedCampaignViewModel.Entity.PrimaryKey, viewModel.ViewDate);

                foreach (var matchViewModel in matchesWithoutVenues)
                {
                    var team1 = matchViewModel.VersionEntity.Team1.GetApprovedVersion<TeamV>(matchViewModel.VersionEntity.MatchDate);

                    if (team1.HomeVenueGuid != null)
                        matchViewModel.VersionEntity.VenueGuid = team1.HomeVenueGuid;
                }
            }

            if (viewModel.FixMatchesWithoutEvents > 0)
            {
                viewModel.MatchDatesWithoutEvents = await DbProvider.GetMatchDatesWithoutEvents(viewModel.SelectedCampaignViewModel.Entity.PrimaryKey);

                var soccerbase = new Soccerbase(DbProvider, User);

                foreach (DateTime matchDate in viewModel.MatchDatesWithoutEvents.Take(viewModel.FixMatchesWithoutEvents))
                    await soccerbase.Import(matchDate, matchDate);
            }

            DbProvider.SaveChanges();

            return RedirectToAction("MatchValidation", new { hk = viewModel.ShortHeaderKey, dt = viewModel.ViewDate.ToUrlString() });
        }
        #endregion
    }
}