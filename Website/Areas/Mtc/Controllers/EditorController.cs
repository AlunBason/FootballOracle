using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Matches;

namespace FootballOracle.Website.Areas.Mtc.Controllers
{
    [RouteArea("Mtc")]
    [RoutePrefix("Editor")]
    [Authorize]
    public class EditorController : BaseMatchController
    {
        #region Constructor
        public EditorController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region Home
        // GET: /Matches/Editor/Home
        [HttpGet, Route("Home/{hk}/{pk}")]
        public async Task<ActionResult> Home(string hk, string pk)
        {
            var viewModel = (MatchEditorViewModel)TempData["MatchEditorViewModel"];

            if (viewModel == null)
                viewModel = await SetModelsByPrimaryKey<MatchEditorViewModel>(pk, hk);

            viewModel.MatchViewModels = viewModel.HeaderEntity.Versions.OrderByDescending(v => v.EffectiveFrom).ThenByDescending(v => v.IsActive).ToViewModels(DateTime.Now);

            viewModel.VersionEntity.ToMatchEditorViewModel(viewModel);

            return View(viewModel);
        }
        #endregion

        #region Create
        [HttpGet, Route("Create")]
        public ActionResult Create()
        {
            if (TempData["Create"] != null)
                return View((MatchEditorViewModel)TempData["Create"]);

            return View(MatchEditorViewModel.CreateNew<MatchEditorViewModel>());
        }

        [ValidateAntiForgeryToken]
        [HttpPost, Route("Create")]
        public ActionResult Create(MatchEditorViewModel viewModel, string Command)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            if (Command == "Next")
            {
                TempData["Create2"] = viewModel;
                return RedirectToAction("Create2");
            }

            return RedirectToAction("Create");
        }

        [HttpGet, Route("Create2")]
        public async Task<ActionResult> Create2()
        {
            var viewModel = (MatchEditorViewModel)TempData["Create2"];

            if (viewModel == null)
                return RedirectHome();

            viewModel.CompetitionPickerData = await DbProvider.GetCompetitionPickerData();

            if (!ModelState.IsValid)
                return View(viewModel);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost, Route("Create2")]
        public ActionResult Create2(MatchEditorViewModel viewModel, string Command)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            if (Command == "Previous")
            {
                TempData["Create"] = viewModel;
                return RedirectToAction("Create");
            }

            if (Command == "Next")
            {
                TempData["Create3"] = viewModel;
                return RedirectToAction("Create3");
            }

            return RedirectToAction("Create2");
        }

        [HttpGet, Route("Create3")]
        public async Task<ActionResult> Create3()
        {
            var viewModel = (MatchEditorViewModel)TempData["Create3"];

            if (viewModel == null)
                return RedirectHome();

            var competition = await DbProvider.GetCompetition((Guid)viewModel.CompetitionGuid, viewModel.ViewDate);
            viewModel.CompetitionName = competition.ToString();

            viewModel.SetVenueName(DbProvider);
            await viewModel.SetTeamPickerData(DbProvider);

            if (!ModelState.IsValid)
                return View(viewModel);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost, Route("Create3")]
        public async Task<ActionResult> Create3(MatchEditorViewModel viewModel, string Command)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            if (Command == "Previous")
            {
                TempData["Create2"] = viewModel;
                return RedirectToAction("Create2");
            }

            if (Command == "Save")
            {
                var competitionV = await DbProvider.GetCompetition((Guid)viewModel.CompetitionGuid, viewModel.ViewDate);
                var campaignGuid = competitionV.Competition.Campaigns.Single(c => c.StartDate <= viewModel.MatchDate && c.EndDate > viewModel.MatchDate).PrimaryKey;
                
                var newMatchV = viewModel.ToMatchV(UserId, UserId, campaignGuid);
                var newMatch = new Match() { PrimaryKey = viewModel.HeaderKey };

                DbProvider.Add(newMatch);
                DbProvider.Add(newMatchV);
                DbProvider.SaveChanges();

                return RedirectToDetailsIndex(newMatch.PrimaryKey);
            }

            return View(viewModel);
        }
        #endregion

        #region Edit
        // GET: /Matches/Editor/Edit
        [HttpGet, Route("Edit/{hk}/{pk}")]
        public async Task<ActionResult> Edit(string hk, string pk)
        {
            var viewModel = (MatchEditorViewModel)TempData["MatchEditorViewModel"];

            if (viewModel == null)
                viewModel = await SetModelsByPrimaryKey<MatchEditorViewModel>(pk, hk);

            viewModel.VersionEntity.ToMatchEditorViewModel(viewModel);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost, Route("Edit/{hk}/{pk}")]
        public async Task<ActionResult> Edit(MatchEditorViewModel viewModel, string Command)
        {
            await SetModelsByPrimaryKey(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            if (Command == "Next")
            {
                TempData["MatchEditorViewModel"] = viewModel;
                return RedirectToAction("Edit2");
            }

            return RedirectToAction("Edit");
        }

        [HttpGet, Route("Edit2")]
        public async Task<ActionResult> Edit2()
        {
            var viewModel = (MatchEditorViewModel)TempData["MatchEditorViewModel"];

            if (viewModel == null)
                return RedirectHome();

            viewModel.CompetitionPickerData = await DbProvider.GetCompetitionPickerData();

            var competition = await DbProvider.GetCompetition((Guid)viewModel.CompetitionGuid, viewModel.ViewDate);
            viewModel.CompetitionName = competition.ToString();

            viewModel.SetVenueName(DbProvider);

            if (!ModelState.IsValid)
                return View(viewModel);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost, Route("Edit2")]
        public async Task<ActionResult> Edit2(MatchEditorViewModel viewModel, string Command)
        {
            await SetModelsByPrimaryKey(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            if (Command == "Previous")
            {
                TempData["MatchEditorViewModel"] = viewModel;
                return RedirectToAction("Edit", new { id = viewModel.ShortHeaderKey, dt = viewModel.ViewDate.ToUrlString() });
            }

            if (Command == "Next")
            {
                TempData["MatchEditorViewModel"] = viewModel;
                return RedirectToAction("Edit3");
            }

            return RedirectToAction("Edit3");
        }

        [HttpGet, Route("Edit3")]
        public async Task<ActionResult> Edit3()
        {
            var viewModel = (MatchEditorViewModel)TempData["MatchEditorViewModel"];

            if (viewModel == null)
                return RedirectHome();

            var competition = await DbProvider.GetCompetition((Guid)viewModel.CompetitionGuid, viewModel.ViewDate);
            viewModel.CompetitionName = competition.ToString();
            
            viewModel.SetVenueName(DbProvider);
            await viewModel.SetTeamPickerData(DbProvider);

            if (!ModelState.IsValid)
                return View(viewModel);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost, Route("Edit3")]
        public async Task<ActionResult> Edit3(MatchEditorViewModel viewModel, string Command)
        {
            await SetModelsByPrimaryKey(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            if (Command == "Previous")
            {
                TempData["MatchEditorViewModel"] = viewModel;
                return RedirectToAction("Edit2");
            }

            if (Command == "Save")
            {
                viewModel.MatchViewModels = viewModel.HeaderEntity.Versions.OrderByDescending(v => v.EffectiveFrom).ThenByDescending(v => v.IsActive).ToViewModels(DateTime.Now);

                var competitionV = await DbProvider.GetCompetition((Guid)viewModel.CompetitionGuid, viewModel.ViewDate);
                var campaignGuid = competitionV.Competition.Campaigns.Single(c => c.StartDate <= viewModel.MatchDate && c.EndDate > viewModel.MatchDate).PrimaryKey;

                var query = viewModel.HeaderEntity.GetApprovedVersionsByEffectiveDate<Match, MatchV>(Date.LowDate);

                if (query.Any())
                {
                    var entityV = query.Single();

                    if (viewModel.Equals(entityV))
                    {
                        ModelState.AddModelError(string.Empty, "No changes have been made.");
                        return View(viewModel);
                    }

                    if (entityV.ModifiedUserId == UserId)
                    {
                        entityV.SetData(viewModel);
                        entityV.DateModified = DateTime.Now;

                        DbProvider.SaveChanges();
                        return RedirectToEditor("Home", entityV);
                    }
                    else
                    {
                        entityV.IsActive = false;

                        var newEntityV = viewModel.ToMatchV(entityV.OwnerUserId, UserId, campaignGuid);
                        DbProvider.Add(newEntityV);

                        foreach (var matchEvent in entityV.MatchEvents)
                        {
                            var newMatchEvent = new MatchEvent()
                            {
                                PrimaryKey = Guid.NewGuid(),
                                MatchVPrimaryKey = newEntityV.PrimaryKey,
                                TeamPrimaryKey = matchEvent.TeamPrimaryKey,
                                PersonPrimaryKey = matchEvent.PersonPrimaryKey,
                                PositionType = matchEvent.PositionType,
                                MatchEventType = matchEvent.MatchEventType,
                                Minute = matchEvent.Minute,
                                Extra = matchEvent.Extra
                            };

                            DbProvider.Add(newMatchEvent);
                        }

                        DbProvider.SaveChanges();
                        return RedirectToEditor("Home", newEntityV);
                    }
                }
            }

            return View(viewModel);
        }
        #endregion

        #region Activate
        // GET: /Matches/Editor/Activate
        [Route("Activate/{hk}/{pk}")]
        public async Task<ActionResult> Activate(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<MatchEditorViewModel>(pk, hk);
            viewModel.GetEntityData();

            var entityVs = viewModel.HeaderEntity.Versions.Where(o => o.EffectiveFrom == viewModel.VersionEntity.EffectiveFrom);

            foreach (var item in entityVs)
                item.IsActive = item.PrimaryKey == viewModel.PrimaryKey;

            DbProvider.SaveChanges();

            return RedirectToEditor("Home", viewModel);
        }
        #endregion

        #region EditPlayers
        // GET: /Matches/Editor/EditPlayers
        [HttpGet, Route("EditPlayers/{hk}/{pk}/{tm}")]
        public async Task<ActionResult> EditPlayers(string hk, string pk, int tm)
        {
            var viewModel = await SetModelsByPrimaryKey<MatchPlayersEditorViewModel>(pk, hk);
            viewModel.TeamNumber = tm;

            var teamGuid = viewModel.TeamNumber == 1 ? viewModel.VersionEntity.Team1Guid : viewModel.VersionEntity.Team2Guid;
            viewModel.MatchPersonViewModels = viewModel.GetMatchPersonViewModels(teamGuid, viewModel.ViewDate);

            return View(viewModel);
        }
        
        // POST: /Matches/Editor/EditPlayers
        [HttpPost, Route("EditPlayers/{hk}/{pk}/{tm}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPlayers(MatchPlayersEditorViewModel viewModel)
        {
            await SetModelsByPrimaryKey(viewModel);

            var teamGuid = viewModel.TeamNumber == 1 ? viewModel.VersionEntity.Team1Guid : viewModel.VersionEntity.Team2Guid;
            var matchEvents = viewModel.VersionEntity.MatchEvents.Where(m => m.TeamPrimaryKey == teamGuid);

            if (viewModel.VersionEntity.ModifiedUserId == UserId)
            {
                if (viewModel.MatchPersonViewModels != null)
                {
                    foreach (var item in viewModel.MatchPersonViewModels)
                    {
                        if (item.MatchEventStartType == null)
                        {
                            var query = from m in matchEvents
                                        where m.PersonPrimaryKey == item.PersonGuid
                                        select m;

                            if (query.Any())
                            {
                                var primaryKey = query.First().PrimaryKey;
                                var header = await DbProvider.GetMatchEventByPrimaryKey(primaryKey);

                                for (var i = query.Count() - 1; i >= 0; i--)
                                    DbProvider.Remove(query.ElementAt(i));

                                DbProvider.Remove(header);
                            }
                        }
                        else
                        {
                            var query = from m in matchEvents
                                        where m.PersonPrimaryKey == item.PersonGuid
                                        && (m.MatchEventType == MatchEventType.Substitute || m.MatchEventType == MatchEventType.Started)
                                        select m;

                            if (query.Any())
                            {
                                query.First().MatchEventType = (MatchEventType)item.MatchEventStartType.ToMatchEventType();
                                query.First().PositionType = item.PositionType;
                            }
                        }
                    }
                }

                if (viewModel.NewMatchPersonViewModels != null)
                {
                    foreach (var item in viewModel.NewMatchPersonViewModels.Where(m => m.MatchEventStartType != null))
                    {
                        DbProvider.Add(new MatchEvent()
                        {
                            PrimaryKey = Guid.NewGuid(),
                            MatchVPrimaryKey = viewModel.PrimaryKey,
                            PersonPrimaryKey = item.PersonGuid,
                            PositionType = item.PositionType,
                            TeamPrimaryKey = teamGuid,
                            MatchEventType = (MatchEventType)item.MatchEventStartType.ToMatchEventType(),
                            Minute = null,
                            Extra = null
                        });
                    }
                }

                DbProvider.SaveChanges();

                return RedirectToAction("EditPlayers", new { hk = viewModel.ShortHeaderKey, pk = viewModel.ShortPrimaryKey, tm = viewModel.TeamNumber });
            }
            else
            {
                var previousVersion = await DbProvider.GetMatch(viewModel.VersionEntity.PrimaryKey, viewModel.VersionEntity.HeaderKey);

                var newMatchV = MatchV.CreateNewVersion<MatchV>(previousVersion.OwnerUserId, UserId);
                newMatchV.HeaderKey = previousVersion.HeaderKey;
                newMatchV.DateCreated = previousVersion.DateCreated;
                newMatchV.SetData(previousVersion);

                foreach (var matchEvent in previousVersion.MatchEvents)
                {
                    var newMatchEvent = new MatchEvent()
                    {
                        PrimaryKey = Guid.NewGuid(),
                        MatchVPrimaryKey = newMatchV.PrimaryKey,
                        TeamPrimaryKey = matchEvent.TeamPrimaryKey,
                        PersonPrimaryKey = matchEvent.PersonPrimaryKey,
                        PositionType = matchEvent.PositionType,
                        MatchEventType = matchEvent.MatchEventType,
                        Minute = matchEvent.Minute,
                        Extra = matchEvent.Extra
                    };

                    DbProvider.Add(newMatchEvent);
                }

                DbProvider.SaveChanges();

                return RedirectToAction("EditPlayers", new { hk = newMatchV.HeaderKey.ToShortGuid(), pk = newMatchV.PrimaryKey.ToShortGuid(), tm = viewModel.TeamNumber });
            }
            
        }
        #endregion

        #region EditEvents
        // GET: /Matches/Editor/EditEvents
        [HttpGet, Route("EditEvents/{hk}/{pk}")]
        public async Task<ActionResult> EditEvents(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<MatchPlayersEventsEditorViewModel>(pk, hk);

            viewModel.Team1MatchEventPersonViewModels = viewModel.GetMatchEventPersonViewModels(true, viewModel.ViewDate);
            viewModel.Team2MatchEventPersonViewModels = viewModel.GetMatchEventPersonViewModels(false, viewModel.ViewDate);

            viewModel.NewTeam1MatchEventPersonViewModel = new MatchEventPersonViewModel();
            viewModel.NewTeam2MatchEventPersonViewModel = new MatchEventPersonViewModel();

            viewModel.Team1PersonViewModels = viewModel.Team1Starters.Concat(viewModel.Team1Substitutes)
                .Select(t => t.PersonViewModel)
                .OrderBy(t => t.VersionEntity.Surname)
                .ThenBy(t => t.VersionEntity.Forenames)
                .Select(t => new CodePickerViewModel() { Code = t.HeaderKey, Description = t.ToString() });

            viewModel.Team2PersonViewModels = viewModel.Team2Starters.Concat(viewModel.Team2Substitutes)
                .Select(t => t.PersonViewModel)
                .OrderBy(t => t.VersionEntity.Surname)
                .ThenBy(t => t.VersionEntity.Forenames)
                .Select(t => new CodePickerViewModel() { Code = t.HeaderKey, Description = t.ToString() });

            return View(viewModel);
        }

        // POST: /Matches/Editor/EditEvents
        [HttpPost, Route("EditEvents/{hk}/{pk}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEvents(MatchPlayersEventsEditorViewModel viewModel)
        {
            await SetModelsByPrimaryKey(viewModel);

            if (viewModel.Team1MatchEventPersonViewModels != null)
            {
                foreach (var item in viewModel.Team1MatchEventPersonViewModels)
                {
                    var matchEvent = await DbProvider.GetMatchEventByPrimaryKey((Guid)item.PrimaryKey);

                    if (item.MatchEventInRunningType == null)
                        DbProvider.Remove(matchEvent);
                    else
                        UpdateMatchEvent(matchEvent, item);

                    DbProvider.SaveChanges();
                }
            }

            if (viewModel.Team2MatchEventPersonViewModels != null)
            {
                foreach (var item in viewModel.Team2MatchEventPersonViewModels)
                {
                    var matchEvent = await DbProvider.GetMatchEventByPrimaryKey((Guid)item.PrimaryKey);

                    if (item.MatchEventInRunningType == null)
                        DbProvider.Remove(matchEvent);
                    else
                        UpdateMatchEvent(matchEvent, item);

                    DbProvider.SaveChanges();
                }
            }

            if (viewModel.NewTeam1MatchEventPersonViewModel != null)
            {
                DbProvider.Add(new MatchEvent()
                {
                    PrimaryKey = Guid.NewGuid(),
                    MatchVPrimaryKey = viewModel.PrimaryKey,
                    PersonPrimaryKey = (Guid)viewModel.NewTeam1MatchEventPersonViewModel.PersonPrimaryKey,
                    TeamPrimaryKey = viewModel.VersionEntity.Team1Guid,
                    MatchEventType = (MatchEventType)viewModel.NewTeam1MatchEventPersonViewModel.MatchEventInRunningType.ToMatchEventType(),
                    Minute = viewModel.NewTeam1MatchEventPersonViewModel.Minute,
                    Extra = viewModel.NewTeam1MatchEventPersonViewModel.Extra
                });

                DbProvider.SaveChanges();
            }

            if (viewModel.NewTeam2MatchEventPersonViewModel != null)
            {
                DbProvider.Add(new MatchEvent()
                {
                    PrimaryKey = Guid.NewGuid(),
                    MatchVPrimaryKey = viewModel.PrimaryKey,
                    PersonPrimaryKey = (Guid)viewModel.NewTeam2MatchEventPersonViewModel.PersonPrimaryKey,
                    TeamPrimaryKey = viewModel.VersionEntity.Team2Guid,
                    MatchEventType = (MatchEventType)viewModel.NewTeam2MatchEventPersonViewModel.MatchEventInRunningType.ToMatchEventType(),
                    Minute = viewModel.NewTeam2MatchEventPersonViewModel.Minute,
                    Extra = viewModel.NewTeam2MatchEventPersonViewModel.Extra
                });

                DbProvider.SaveChanges();
            }

            return RedirectToEditor("EditEvents", viewModel);
        }
        #endregion

        #region Private methods
        private void UpdateMatchEvent(MatchEvent matchEvent, MatchEventPersonViewModel viewModel)
        {
            if (viewModel.PersonPrimaryKey != matchEvent.PersonPrimaryKey 
                || viewModel.MatchEventInRunningType.ToMatchEventType() != matchEvent.MatchEventType
                || viewModel.Minute != matchEvent.Minute
                || viewModel.Extra != matchEvent.Extra)
            {
                matchEvent.PersonPrimaryKey = (Guid)viewModel.PersonPrimaryKey;
                matchEvent.MatchEventType = (MatchEventType)viewModel.MatchEventInRunningType.ToMatchEventType();
                matchEvent.Minute = viewModel.Minute;
                matchEvent.Extra = viewModel.Extra;
            }
        }
        #endregion
    }
}