using System;
using System.Linq;
using System.Web.Mvc;
using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.ViewModels.Approvable.Venues;
using FootballOracle.Website.Controllers;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using System.Threading.Tasks;

namespace FootballOracle.Website.Areas.Ven.Controllers
{
    [RouteArea("Ven")]
    [RoutePrefix("Editor")]
    [Authorize]
    public class EditorController : BaseVenueController
    {
        #region Constructor
        public EditorController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region Create
        // GET: /Venues/Editor/Create
        [HttpGet, Route("Create")]
        public ActionResult Create()
        {
            return View(VenueEditorViewModel.CreateNew<VenueEditorViewModel>());
        }

        [ValidateAntiForgeryToken]
        // POST: /Venues/Editor/Create
        [HttpPost, Route("Create")]
        public ActionResult Create(VenueEditorViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var venueV = viewModel.ToVenueV(UserId, UserId);

            DbProvider.Add(new Venue() { PrimaryKey = viewModel.HeaderKey });
            DbProvider.Add(venueV);
            DbProvider.SaveChanges();

            return RedirectToDetailsIndex(venueV.HeaderKey);
        }
        #endregion

        #region Summary
        // GET: /Teams/Editor/Summary
        [Route("Summary/{hk}/{pk}")]
        public async Task<ActionResult> Summary(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<VenueVersionSummaryViewModel>(pk, hk);
            viewModel.GetEntityData();
            viewModel.EditableViewModels = viewModel.VersionEntity.Venue.GetEditableVersions(DateTime.Now);

            return View(viewModel);
        }
        #endregion

        #region Edit
        // GET: /Teams/Editor/Edit
        [Route("Edit/{hk}/{pk}")]
        public async Task<ActionResult> Edit(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<VenueEditorViewModel>(pk, hk);
            viewModel.GetEntityData();
            viewModel.EditableViewModels = viewModel.VersionEntity.Venue.GetEditableVersions(DateTime.Now);

            return View(viewModel);
        }

        [HttpPost]
        [Route("Edit/{hk}/{pk}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VenueEditorViewModel viewModel)
        {
            await SetModelsByPrimaryKey(viewModel);
            viewModel.EditableViewModels = viewModel.VersionEntity.Venue.GetEditableVersions(DateTime.Now);

            var validate = DoEditValidation(viewModel);

            if (validate != null)
                return validate;

            var query = viewModel.HeaderEntity.GetApprovedVersionsByEffectiveDate<Venue, VenueV>(viewModel.EffectiveFrom);

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
                    SetSaveChangesMessage(SaveChangesMessageType.ChangesSaved);
                    return RedirectToEditor("Summary", entityV);
                }
                else
                {
                    entityV.IsActive = false;

                    var newEntityV = viewModel.ToVenueV(entityV.OwnerUserId, UserId);
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

                var newEntityV = viewModel.ToVenueV(UserId, UserId);
                newEntityV.EffectiveTo = Date.HighDate;

                DbProvider.Add(newEntityV);
                viewModel.HeaderEntity.NormaliseVersionEffectiveDates<VenueV>();
                DbProvider.SaveChanges();
                SetSaveChangesMessage(SaveChangesMessageType.ChangesSaved);
                return RedirectToEditor("Summary", newEntityV);
            }
        }

        private ViewResult DoEditValidation(VenueEditorViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            return null;
        }
        #endregion

        #region Activate
        // GET: /Venues/Editor/Activate
        [Route("Activate/{hk}/{pk}")]
        public async Task<ActionResult> Activate(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<VenueEditorViewModel>(pk, hk);
            viewModel.GetEntityData();

            var entityVs = viewModel.HeaderEntity.Versions.Where(o => o.HeaderKey == viewModel.HeaderKey && o.EffectiveFrom == viewModel.VersionEntity.EffectiveFrom);

            foreach (var item in entityVs)
                item.IsActive = item.PrimaryKey == viewModel.PrimaryKey;

            DbProvider.SaveChanges();
            SetSaveChangesMessage(SaveChangesMessageType.RecordActivated);
            return RedirectToEditor("Edit", viewModel);
        }
        #endregion

        #region Delete
        // GET: /Venues/Editor/Delete
        [Route("Delete/{hk}/{pk}")]
        public async Task<ActionResult> Delete(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<VenueEditorViewModel>(pk, hk);
            viewModel.GetEntityData();

            var activeEntity = viewModel.VersionEntity.Venue.GetApprovedVersion<VenueV>(viewModel.ViewDate);

            if (viewModel.VersionEntity.CanDelete(User))
            {
                DbProvider.Remove(viewModel.VersionEntity);
                SetSaveChangesMessage(SaveChangesMessageType.RecordDeleted);
                DbProvider.SaveChanges();
            }

            return RedirectToEditor("Edit", activeEntity);
        }
        #endregion
    }
}