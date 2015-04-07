using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Teams;
using FootballOracle.Models.ViewModels.Standard;
using FootballOracle.Website.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FootballOracle.Models.ViewModels.Approvable.People;

namespace FootballOracle.Website.Areas.Tms.Controllers
{
    [Authorize]
    [RouteArea("Tms")]
    [RoutePrefix("Editor")]
    public class EditorController : BaseTeamController
    {
        #region Constructor
        public EditorController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region Summary
        // GET: /Teams/Editor/Summary
        [Route("Summary/{hk}/{pk}")]
        public async Task<ActionResult> Summary(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<TeamVersionSummaryViewModel>(pk, hk);
            viewModel.GetEntityData();
            viewModel.EditableViewModels = viewModel.VersionEntity.Team.GetEditableVersions(DateTime.Now);

            return View(viewModel);
        }
        #endregion

        #region Edit
        // GET: /Teams/Editor/Edit
        [Route("Edit/{hk}/{pk}")]
        public async Task<ActionResult> Edit(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<TeamEditorViewModel>(pk, hk);
            viewModel.GetEntityData();
            viewModel.EditableViewModels = viewModel.VersionEntity.Team.GetEditableVersions(DateTime.Now);

            return View(viewModel);
        }

        [HttpPost]
        [Route("Edit/{hk}/{pk}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TeamEditorViewModel viewModel, HttpPostedFileBase imageData)
        {
            await SetModelsByPrimaryKey(viewModel);
            viewModel.EditableViewModels = viewModel.VersionEntity.Team.GetEditableVersions(DateTime.Now);

            var validate = DoEditValidation(viewModel);

            if (validate != null)
                return validate;

            var resourceGuid = AddImageData(imageData);

            if (resourceGuid != null)
                viewModel.ResourceGuid = resourceGuid;

            var query = viewModel.HeaderEntity.GetApprovedVersionsByEffectiveDate<Team, TeamV>(viewModel.EffectiveFrom);

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
                    entityV.SetData(viewModel, DbProvider);

                    DbProvider.SaveChanges();
                    SetSaveChangesMessage(SaveChangesMessageType.ChangesSaved);
                    return RedirectToEditor("Summary", entityV);
                }
                else
                {
                    entityV.IsActive = false;

                    var newEntityV = viewModel.ToTeamV(entityV.OwnerUserId, UserId);
                    newEntityV.EffectiveTo = viewModel.EffectiveTo;

                    DbProvider.Add(newEntityV);
                    DbProvider.SaveChanges();
                    SetSaveChangesMessage(SaveChangesMessageType.ChangesSaved);
                    return RedirectToEditor("Summary", newEntityV);
                }
            }
            else
            {
                if (viewModel.Equals(viewModel.VersionEntity))
                {
                    ModelState.AddModelError("EffectiveFrom", "Only the effective date has changed.");
                    return View(viewModel);
                }

                var newEntityV = viewModel.ToTeamV(UserId, UserId);
                newEntityV.EffectiveTo = Date.HighDate;

                DbProvider.Add(newEntityV);
                viewModel.HeaderEntity.NormaliseVersionEffectiveDates<TeamV>();
                DbProvider.SaveChanges();
                SetSaveChangesMessage(SaveChangesMessageType.ChangesSaved);
                return RedirectToEditor("Summary", newEntityV);
            }
        }

        private ViewResult DoEditValidation(TeamEditorViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            return null;
        }

        private Guid? AddImageData(HttpPostedFileBase imageData)
        {
            if (imageData == null)
                return null;

            var imageBytes = imageData.ToBytes();

            var resourceGuid = Guid.NewGuid();
            var newResource = new Resource()
            {
                PrimaryKey = (Guid)resourceGuid,
                ResourceBytes = imageBytes
            };

            DbProvider.Add(newResource);

            return resourceGuid;
        }
        #endregion

        #region Activate
        // GET: /Teams/Editor/Activate
        [Route("Activate/{hk}/{pk}")]
        public async Task<ActionResult> Activate(string hk, string pk)
        {
            var viewModel = (await SetModelsByPrimaryKey<TeamEditorViewModel>(pk, hk));
            viewModel.GetEntityData();

            var entityVs = viewModel.HeaderEntity.Versions.Where(o => o.HeaderKey == viewModel.HeaderKey && o.EffectiveFrom == viewModel.VersionEntity.EffectiveFrom);

            foreach (var item in entityVs)
                item.IsActive = item.PrimaryKey == viewModel.PrimaryKey;

            DbProvider.SaveChanges();
            SetSaveChangesMessage(SaveChangesMessageType.RecordActivated);
            return RedirectToEditor("Summary", viewModel);
        }
        #endregion

        #region Delete
        // GET: /Teams/Editor/Delete
        [Route("Delete/{hk}/{pk}")]
        public async Task<ActionResult> Delete(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<TeamEditorViewModel>(pk, hk);
            viewModel.GetEntityData();

            var headerEntity = viewModel.HeaderEntity;

            if (viewModel.VersionEntity.CanDelete(User))
            {
                var latestEffectiveToDate = headerEntity.GetApprovedVersions<TeamV>().OrderBy(o => o.EffectiveFrom).Last().EffectiveTo;

                DbProvider.Remove(viewModel.VersionEntity);
                SetSaveChangesMessage(SaveChangesMessageType.RecordDeleted);
                headerEntity.NormaliseVersionEffectiveDates<TeamV>(latestEffectiveToDate);
                DbProvider.SaveChanges();
            }

            return RedirectToEditor("Summary", headerEntity.ToViewModel(viewModel.ViewDate));
        }
        #endregion

        #region Create
        // GET: /Teams/Editor/Create
        [HttpGet, Route("Create")]
        public ActionResult Create()
        {
            return View(TeamEditorViewModel.CreateNew<TeamEditorViewModel>());
        }

        [ValidateAntiForgeryToken]
        // POST: /Teams/Editor/Create
        [HttpPost, Route("Create")]
        public ActionResult Create(TeamEditorViewModel viewModel)
        {
            var validate = DoEditValidation(viewModel);

            if (validate != null)
                return validate;

            var teamV = viewModel.ToTeamV(UserId, UserId);

            DbProvider.Add(new Team() { PrimaryKey = viewModel.HeaderKey });
            DbProvider.Add(teamV);
            DbProvider.SaveChanges();

            return RedirectToDetailsIndex(teamV.PrimaryKey);
        }
        #endregion

        #region ManageLookups
        // GET: /Teams/Editor/ManageLookups
        [Route("ManageLookups/{hk}/{pk}")]
        public async Task<ActionResult> ManageLookups(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<TeamManageLookupsViewModel>(pk, hk);

            viewModel.LookupTeamViewModels = new List<LookupTeamViewModel>();

            var lookups = await DbProvider.GetLookupTeamByTeamKey(viewModel.HeaderKey);

            foreach (var lookup in lookups)
            {
                viewModel.LookupTeamViewModels.Add(new LookupTeamViewModel()
                {
                    PrimaryKey = lookup.PrimaryKey,
                    ImportSite = lookup.ImportSite,
                    TeamGuid = lookup.TeamGuid,
                    LookupId = lookup.LookupId
                });
            }

            viewModel.LookupTeamViewModels.Add(new LookupTeamViewModel()
            {
                PrimaryKey = Guid.NewGuid(),
                ImportSite = null,
                TeamGuid = viewModel.HeaderKey,
                LookupId = string.Empty
            });


            return View(viewModel);
        }

        [HttpPost]
        [Route("ManageLookups/{hk}/{pk}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageLookups(TeamManageLookupsViewModel viewModel)
        {
            foreach (var lookup in viewModel.LookupTeamViewModels)
            {
                var lookupTeamEntity = await DbProvider.GetLookupTeamByPrimaryKey(lookup.PrimaryKey);

                if (lookupTeamEntity != null)
                {
                    if (lookup.ImportSite == null)
                        DbProvider.Remove(lookupTeamEntity);
                    else
                    {
                        lookupTeamEntity.ImportSite = (ImportSite)lookup.ImportSite;
                        lookupTeamEntity.LookupId = lookup.LookupId.Trim();
                    }
                }
                else
                {
                    if (lookup.ImportSite != null)
                    {
                        DbProvider.Add(new LookupTeam()
                        {
                            PrimaryKey = Guid.NewGuid(),
                            ImportSite = (ImportSite)lookup.ImportSite,
                            TeamGuid = lookup.TeamGuid,
                            LookupId = lookup.LookupId.Trim()
                        });
                    }
                }
            }
            DbProvider.SaveChanges();

            return RedirectToAction("ManageLookups", new { pk = viewModel.PrimaryKey.ToShortGuid() });
        }
        #endregion
    }
}