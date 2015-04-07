using System;
using System.Linq;
using System.Web.Mvc;
using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.ViewModels.Standard.Campaigns;
using FootballOracle.Models.ViewModels.Approvable.Competitions;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using FootballOracle.Services.Import;
using FootballOracle.Website.Attributes;
using FootballOracle.Website.Controllers;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using FootballOracle.Models.ViewModels.Standard;
using System.Collections.Generic;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using System.Threading.Tasks;

namespace FootballOracle.Website.Areas.Cnt.Controllers
{
    [RouteArea("Cnt")]
    [RoutePrefix("Admin")]
    [AuthorizeAdmin]
    public class AdminController : BaseCountryController
    {
        #region Constructor
        public AdminController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region Index
        [Route("{hk}/{dt?}")]
        public ActionResult Index(string hk, DateTime? dt)
        {
            var viewModel = SetModels<BaseCountryViewModel>(hk, dt);

            return View(viewModel);
        }
        #endregion

        #region MatchValidation
        // GET: /Competitions/Details/MatchValidation
        [Route("ManageTeamLookups/{hk}/{dt?}")]
        public async Task<ActionResult> ManageTeamLookups(string hk, DateTime? dt)
        {
            var viewModel = await SetModels<CountryManageTeamLookupsViewModel>(hk, dt);

            var teamLookupsViewModels = new List<TeamLookupsViewModel>();

            var teamViewModels = viewModel.TeamViewModels.OrderBy(t => t.ToString());

            foreach (var teamViewModel in teamViewModels)
            {
                var espnLookup = await DbProvider.GetLookupTeam(teamViewModel.HeaderKey, ImportSite.Espn);
                var soccerbaseLookup = await DbProvider.GetLookupTeam(teamViewModel.HeaderKey, ImportSite.Soccerbase);

                teamLookupsViewModels.Add(new TeamLookupsViewModel()
                {
                    TeamKey = teamViewModel.HeaderKey,
                    TeamViewModel = teamViewModel,
                    EspnLookupId = espnLookup != null ? espnLookup.LookupId : null,
                    SoccerbaseLookupId = soccerbaseLookup != null ? soccerbaseLookup.LookupId : null
                });
            }

            viewModel.TeamLookupsViewModels = teamLookupsViewModels;

            return View(viewModel);
        }

        [Route("ManageTeamLookups/{hk}/{dt?}")]
        [HttpPost]
        public async Task<ActionResult> ManageTeamLookups(CountryManageTeamLookupsViewModel viewModel)
        {
            await SetModelsByPrimaryKey(viewModel);

            foreach (var teamLookupViewModel in viewModel.TeamLookupsViewModels)
            {
                var espnLookup = await DbProvider.GetLookupTeam(teamLookupViewModel.TeamKey, ImportSite.Espn);

                if (espnLookup != null)
                {
                    if (string.IsNullOrWhiteSpace(teamLookupViewModel.EspnLookupId))
                        DbProvider.Remove(espnLookup);
                    else
                        espnLookup.LookupId = teamLookupViewModel.EspnLookupId;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(teamLookupViewModel.EspnLookupId))
                        DbProvider.Add(new LookupTeam()
                        {
                            PrimaryKey = Guid.NewGuid(),
                            TeamGuid = teamLookupViewModel.TeamKey,
                            ImportSite = ImportSite.Espn,
                            LookupId = teamLookupViewModel.EspnLookupId
                        });
                }
                
                var soccerbaseLookup = await DbProvider.GetLookupTeam(teamLookupViewModel.TeamKey, ImportSite.Soccerbase);

                if (soccerbaseLookup != null)
                {
                    if (string.IsNullOrWhiteSpace(teamLookupViewModel.SoccerbaseLookupId))
                        DbProvider.Remove(soccerbaseLookup);
                    else
                        soccerbaseLookup.LookupId = teamLookupViewModel.SoccerbaseLookupId;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(teamLookupViewModel.SoccerbaseLookupId))
                        DbProvider.Add(new LookupTeam()
                        {
                            PrimaryKey = Guid.NewGuid(),
                            TeamGuid = teamLookupViewModel.TeamKey,
                            ImportSite = ImportSite.Soccerbase,
                            LookupId = teamLookupViewModel.SoccerbaseLookupId
                        });
                }
            }
            DbProvider.SaveChanges();

            return RedirectToAction("ManageTeamLookups", new { hk = viewModel.ShortHeaderKey, dt = viewModel.ViewDate.ToUrlString() });
        }
        #endregion
    }
}