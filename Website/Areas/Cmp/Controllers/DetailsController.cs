using FootballOracle.Foundation;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Competitions;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using FootballOracle.Models.ViewModels.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FootballOracle.Models.ViewModels.Approvable.Teams;

namespace FootballOracle.Website.Areas.Cmp.Controllers
{
    [RouteArea("Cmp")]
    [RoutePrefix("Details")]
    public class DetailsController : BaseCompetitionController
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
            var viewModel = await SetModels<BaseCompetitionViewModel>(hk, dt);

            if (viewModel.VersionEntity.CompetitionType == CompetitionType.Cup)
                return ApprovableRedirect("CupSummary", hk, dt);

            return ApprovableRedirect("LeagueSummary", hk, dt);
        }
        #endregion

        #region LeagueSummary
        [Route("LeagueSummary/{hk}/{dt?}/{vt?}")]
        public async Task<ActionResult> LeagueSummary(string hk, DateTime? dt, int? vt)
        {
            var viewModel = await SetModels<CompetitionLeagueSummaryViewModel>(hk, dt);
            viewModel.ResultsPage = 1;
            viewModel.ViewType = vt ?? 3;

            viewModel.SetCampaigns(viewModel.ViewType);

            return View(viewModel);
        }
        #endregion

        #region CupSummary
        [Route("CupSummary/{hk}/{dt?}/{sk?}")]
        public async Task<ActionResult> CupSummary(string hk, DateTime? dt, string sk)
        {
            var viewModel = await SetModels<CompetitionCupSummaryViewModel>(hk, dt);

            viewModel.SetCampaigns(3, sk.ToNullableGuid());

            return View(viewModel);
        }
        #endregion

        #region Players
        [Route("Players/{hk}/{dt?}")]
        public async Task<ActionResult> Players(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<CompetitionPlayersViewModel>(hk, dt);

            viewModel.SetCampaigns();
             
            //viewModel.GoalsEventTotals = await DbProvider.GetTopPersonCampaignEventTotalsByMatchEventType(viewModel.SelectedCampaignViewModel.Entity.PrimaryKey, MatchEventType.Scored);
            //viewModel.SentOffEventTotals = await DbProvider.GetTopPersonCampaignEventTotalsByMatchEventType(viewModel.SelectedCampaignViewModel.Entity.PrimaryKey, MatchEventType.SentOff);
            //viewModel.BookedEventTotals = await DbProvider.GetTopPersonCampaignEventTotalsByMatchEventType(viewModel.SelectedCampaignViewModel.Entity.PrimaryKey, MatchEventType.Booked);
            

            return View(viewModel);
        }
        #endregion

        #region Matches
        [Route("Matches/{hk}/{dt?}/{rp?}/{fp?}")]
        public async Task<ActionResult> Matches(string hk, DateTime? dt, int? rp, int? fp)
        {
            var viewModel = await SetModels<CompetitionMatchesViewModel>(hk, dt);

            viewModel.SetCampaigns();

            viewModel.ResultsPage = rp ?? 1;
            viewModel.FixturesPage = fp ?? 1;

            return View(viewModel);
        }
        #endregion

        #region Form
        [Route("Form/{hk}/{dt?}/{vt?}/{mc?}")]
        public async Task<ActionResult> Form(string hk, DateTime? dt, int? vt, int? mc)
        {
            var viewModel = await SetModels<CompetitionFormViewModel>(hk, dt);

            viewModel.ViewType = vt ?? 3;
            viewModel.MatchCount = mc ?? 5;

            viewModel.SetCampaigns(viewModel.ViewType);

            //viewModel.SelectedCampaignViewModel.LeagueTableItemViewModels = viewModel.SelectedCampaignViewModel.ResultMatchViewModels.GetStandings(viewModel.ViewType, viewModel.ViewDate, viewModel.MatchCount);
            //viewModel.SelectedCampaignViewModel.LeagueTableItemViewModels.Select(s => s.TeamViewModel).SetFormData();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Form/{hk}/{dt?}/{vt?}/{mc?}")]
        public ActionResult Form(CompetitionFormViewModel viewModel)
        {
            return RedirectToAction("Form", new { id = viewModel.ShortHeaderKey, vt = viewModel.ViewType, mc = viewModel.MatchCount, dt = viewModel.ViewDate.ToUrlString() });
        }
        #endregion
    }
}