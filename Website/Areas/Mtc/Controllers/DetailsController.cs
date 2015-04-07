using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using FootballOracle.Models.ViewModels.Standard;
using FootballOracle.Models.ViewModels.Standard.Charts;
using FootballOracle.Services.Import;
using FootballOracle.Website.Attributes;

namespace FootballOracle.Website.Areas.Mtc.Controllers
{
    [RouteArea("Mtc")]
    [RoutePrefix("Details")]
    public class DetailsController : BaseMatchController
    {
        #region Constructor
        public DetailsController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region Index
        [Route("{hk}")]
        public async Task<ActionResult> Index(string hk)
        {
            var viewModel = await SetModels<BaseMatchViewModel>(hk, DateTime.Now);

            if (viewModel.VersionEntity.Team1FT == null && viewModel.VersionEntity.Team2FT == null)
                return ApprovableRedirect("Predictor", hk, null);

            return ApprovableRedirect("Summary", hk, null);
        }
        #endregion

        #region Summary
        [Route("Summary/{hk}")]
        public async Task<ActionResult> Summary(string hk)
        {
            var viewModel = await SetModels<MatchSummaryViewModel>(hk, DateTime.Now);

            var otherMatchesByCompetitionAndDate = await DbProvider.GetMatchesByCompetitionAndDate((Guid)viewModel.CompetitionGuid, viewModel.MatchDate);

            viewModel.OtherMatchesByCompetitionAndDate = otherMatchesByCompetitionAndDate.Where(w => w.HeaderKey != viewModel.HeaderKey);

            return View(viewModel);
        }
        #endregion

        #region HeadToHead
        [Route("HeadToHead/{hk}")]
        public async Task<ActionResult> HeadToHead(string hk)
        {
            var viewModel = await SetModels<MatchHeadToHeadViewModel>(hk, DateTime.Now);
            viewModel.HeadToHeadMatchViewModels = await DbProvider.GetHeadToHeadMatches(viewModel.Team1Guid, viewModel.Team2Guid, viewModel.ViewDate);

            return View(viewModel);
        }
        #endregion

        #region Predictor
        [Route("Predictor/{hk}")]
        public async Task<ActionResult> Predictor(string hk)
        {
            var viewModel = await SetModels<MatchPredictorViewModel>(hk, DateTime.Now);

            viewModel.Team1ViewModel.SetFormData();
            viewModel.Team2ViewModel.SetFormData();

            return View(viewModel);
        }
        #endregion

        #region Progress
        [Route("Progress/{hk}")]
        public async Task<ActionResult> Progress(string hk)
        {
            var viewModel = await SetModels<MatchProgressViewModel>(hk, DateTime.Now);
            var teamPositionChartData = new List<PositionDateData>();
            var selectedCampaignViewModel = viewModel.VersionEntity.CampaignStage.Campaign.ToViewModel(viewModel.MatchDate);
            var teamKeys = new[] { viewModel.Team1Guid, viewModel.Team2Guid };
            var results = selectedCampaignViewModel.ResultMatchViewModels;
            var accumulatedCampaignData = results.GetAccumulatedDataFromMatches(viewModel.ViewDate, teamKeys);
            var teamCount = results.Select(r => r.Team1Guid).Concat(results.Select(r => r.Team2Guid)).Distinct().Count();
            int position1 = teamCount;
            int position2 = teamCount;
            viewModel.TeamCount = teamCount;

            foreach (var group in accumulatedCampaignData.GroupBy(g => g.ViewDate))
            {
                var matchDate = group.First().ViewDate;
                var team1Played = false;
                var team2Played = false;
                var team1Tooltip = string.Empty;
                var team2Tooltip = string.Empty;
                
                foreach(var item in group)
                {
                    if (item.TeamViewModel.HeaderKey == viewModel.Team1Guid)
                    {
                        team1Played = true;
                        team1Tooltip = item.MatchViewModel != null ? string.Format("{0}<br/>{1}", item.ViewDate.ToDisplayString(), item.MatchViewModel.ToString()) : string.Empty;
                        position1 = item.Position;
                    }

                    if (item.TeamViewModel.HeaderKey == viewModel.Team2Guid)
                    {
                        team2Played = true;
                        team2Tooltip = item.MatchViewModel != null ? string.Format("{0}<br/>{1}", item.ViewDate.ToDisplayString(), item.MatchViewModel.ToString()) : string.Empty;
                        position2 = item.Position;
                    }
                }

                if (team1Played || team2Played)
                {
                    teamPositionChartData.Add(new PositionDateData()
                    {
                        Tooltip = team1Played ? string.Format("{0}<br/>Position: {1}", team1Tooltip, position1.AddOrdinal()) : string.Format("Position: {0}", position1.AddOrdinal()),
                        GroupData = new CodePickerViewModel() { Code = viewModel.Team1Guid, Description = viewModel.Team1ViewModel.ToString() },
                        DateValue = matchDate,
                        Position = position1
                    });

                    teamPositionChartData.Add(new PositionDateData()
                    {
                        Tooltip = team2Played ? string.Format("{0}<br/>Position: {1}", team2Tooltip, position2.AddOrdinal()) : string.Format("Position: {0}", position2.AddOrdinal()),
                        GroupData = new CodePickerViewModel() { Code = viewModel.Team2Guid, Description = viewModel.Team2ViewModel.ToString() },
                        DateValue = matchDate,
                        Position = position2
                    });
                }
            }

            viewModel.TeamPositionChartData = teamPositionChartData;

            return View(viewModel);
        }
        #endregion

        #region Import
        [AuthorizeAdmin]
        [Route("Import/{hk}")]
        public async Task<ActionResult> Import(string hk)
        {
            var viewModel = await SetModels<BaseMatchViewModel>(hk, DateTime.Now);

            var espn = new Espn(DbProvider, User);
            await espn.ImportMatchDetail(viewModel.HeaderKey);

            return View(viewModel);
        }
        #endregion
    }
}