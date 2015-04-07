using FootballOracle.Foundation;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using FootballOracle.Models.ViewModels.Approvable.Teams;
using FootballOracle.Models.ViewModels.Standard.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FootballOracle.Website.Areas.Tms.Controllers
{
    [RouteArea("Tms")]
    [RoutePrefix("Details")]
    public class DetailsController : BaseTeamController
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
            return ApprovableRedirect("Summary", hk, dt);
        }
        #endregion

        #region Summary
        [Route("Summary/{hk}/{dt?}")]
        public async Task<ActionResult> Summary(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<TeamSummaryViewModel>(hk, dt);

            viewModel.SelectedCampaignViewModel = viewModel.SetSelectedCampaign();

            if (viewModel.SelectedCampaignViewModel != null)
            {
                await viewModel.SetCampaignMatches();
                viewModel.SelectedCampaignViewModel.SetCampaignStages(3);
                //viewModel.SelectedCampaignViewModel.SelectedCampaignStageViewModel. = viewModel.SelectedCampaignViewModel.ResultMatchViewModels.GetStandings(3, viewModel.ViewDate);
                //viewModel.SelectedCampaignViewModel.LeagueTableItemViewModels.Select(s => s.TeamViewModel).SetFormData();
            }

            return View(viewModel);
        }
        #endregion

        #region Players
        [Route("Players/{hk}/{dt?}")]
        public async Task<ActionResult> Players(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<TeamPlayersViewModel>(hk, dt);
            viewModel.SelectedCampaignViewModel = viewModel.SetSelectedCampaign();

            return View(viewModel);
        }
        #endregion

        #region Progress
        [Route("Progress/{hk}/{dt?}")]
        public async Task<ActionResult> Progress(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<TeamProgressViewModel>(hk, dt);

            viewModel.SelectedCampaignViewModel = viewModel.SetSelectedCampaign();

            if (viewModel.SelectedCampaignViewModel != null)
            {
                viewModel.SelectedCampaignViewModel.SetCampaignStages(3);

                var teamPositionChartData = new List<PositionDateData>();
                var teamKeys = new[] { viewModel.HeaderKey };
                var results = viewModel.SelectedCampaignViewModel.ResultMatchViewModels;
                var accumulatedCampaignData = results.GetAccumulatedDataFromMatches(viewModel.ViewDate, teamKeys);
                var teamCount = results.Select(r => r.Team1Guid).Concat(results.Select(r => r.Team2Guid)).Distinct().Count();

                foreach (var item in accumulatedCampaignData)
                {
                    teamPositionChartData.Add(new PositionDateData()
                    {
                        Tooltip = string.Format("{0}<br/>{1}<br/>Position: {2}", item.ViewDate.ToDisplayString(), item.MatchViewModel.ToString(), item.Position.AddOrdinal()),
                        GroupData = new CodePickerViewModel() { Code = viewModel.HeaderKey, Description = viewModel.ToString() },
                        DateValue = item.ViewDate,
                        Position = item.Position
                    });
                }

                viewModel.TeamPositionChartData = teamPositionChartData;
                viewModel.TeamCount = teamCount;
            }

            return View(viewModel);
        }
        #endregion

        #region Statistics
        [Route("Statistics/{hk}/{dt?}")]
        public async Task<ActionResult> Statistics(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<TeamStatisticsViewModel>(hk, dt);

            viewModel.SetTeamStatistics();

            return View(viewModel);
        }
        #endregion
    }
}