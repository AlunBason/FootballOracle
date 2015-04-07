using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Organisations;
using FootballOracle.Website.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FootballOracle.Website.Areas.Org.Controllers
{
    [RouteArea("Org")]
    [RoutePrefix("Editor")]
    [Authorize]
    public class EditorController : BaseOrganisationController
    {
        #region Constructor
        public EditorController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region Summary
        // GET: /Organisations/Editor/Summary
        [Route("Summary/{hk}/{pk}")]
        public async Task<ActionResult> Summary(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<OrganisationVersionSummaryViewModel>(pk, hk);
            viewModel.GetEntityData();
            viewModel.EditableViewModels = viewModel.VersionEntity.Organisation.GetEditableVersions(DateTime.Now);

            return View(viewModel);
        }
        #endregion

        #region Create
        // GET: /Organisations/Editor/Create
        [HttpGet, Route("Create")]
        public ActionResult Create()
        {
            return View(OrganisationEditorViewModel.CreateNew<OrganisationEditorViewModel>());
        }

        [ValidateAntiForgeryToken]
        // POST: /Organisations/Editor/Create
        [HttpPost, Route("Create")]
        public ActionResult Create(OrganisationEditorViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var organisationV = viewModel.ToOrganisationV(UserId, UserId);
            organisationV.EffectiveTo = Date.HighDate;
            DbProvider.Add(new Organisation() { PrimaryKey = organisationV.HeaderKey });
            DbProvider.Add(organisationV);
            DbProvider.SaveChanges();

            return RedirectToDetailsIndex(organisationV.HeaderKey);
        }
        #endregion

        #region Edit
        // GET: /Teams/Editor/Edit
        [Route("Edit/{hk}/{pk}")]
        public async Task<ActionResult> Edit(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<OrganisationEditorViewModel>(pk, hk);
            viewModel.GetEntityData();
            viewModel.EditableViewModels = viewModel.VersionEntity.Organisation.GetEditableVersions(DateTime.Now);

            return View(viewModel);
        }

        [HttpPost]
        [Route("Edit/{hk}/{pk}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OrganisationEditorViewModel viewModel)
        {
            await SetModelsByPrimaryKey(viewModel);
            viewModel.EditableViewModels = viewModel.VersionEntity.Organisation.GetEditableVersions(DateTime.Now);

            var validate = DoEditValidation(viewModel);

            if (validate != null)
                return validate;

            var query = viewModel.HeaderEntity.GetApprovedVersionsByEffectiveDate<Organisation, OrganisationV>(viewModel.EffectiveFrom);

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
                    return RedirectToAction("Summary", new { hk = entityV.HeaderKey.ToShortGuid(), pk = entityV.PrimaryKey.ToShortGuid() });
                }
                else
                {
                    entityV.IsActive = false;

                    var newEntityV = viewModel.ToOrganisationV(entityV.OwnerUserId, UserId);
                    newEntityV.EffectiveTo = viewModel.EffectiveTo;

                    DbProvider.Add(newEntityV);

                    DbProvider.SaveChanges();
                    SetSaveChangesMessage(SaveChangesMessageType.ChangesSaved);
                    return RedirectToAction("Summary", new { hk = newEntityV.HeaderKey.ToShortGuid(), pk = newEntityV.PrimaryKey.ToShortGuid() });
                }
            }
            else
            {
                if (viewModel.Equals(viewModel.VersionEntity))
                {
                    ModelState.AddModelError("EffectiveFrom", "Only the effective date has changed.");
                    return View(viewModel);
                }

                var newEntityV = viewModel.ToOrganisationV(UserId, UserId);
                newEntityV.EffectiveTo = Date.HighDate;

                DbProvider.Add(newEntityV);
                viewModel.HeaderEntity.NormaliseVersionEffectiveDates<OrganisationV>();

                DbProvider.SaveChanges();
                SetSaveChangesMessage(SaveChangesMessageType.ChangesSaved);
                return RedirectToAction("Summary", new { hk = newEntityV.HeaderKey.ToShortGuid(), pk = newEntityV.PrimaryKey.ToShortGuid() });
            }
        }

        private ViewResult DoEditValidation(OrganisationEditorViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            //Can't have country and parent organisation
            if (viewModel.ParentOrganisationGuid != null && viewModel.CountryGuid != null)
            {
                ModelState.AddModelError("ParentOrganisationGuid", "Only one of country and parent organisation can be set.");
                return View(viewModel);
            }

            return null;
        }
        #endregion

        #region Activate
        [Route("Activate/{hk}/{pk}")]
        public async Task<ActionResult> Activate(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<OrganisationEditorViewModel>(pk, hk);
            viewModel.GetEntityData();

            var entityVs = viewModel.HeaderEntity.Versions.Where(o => o.EffectiveFrom == viewModel.VersionEntity.EffectiveFrom);

            foreach (var item in entityVs)
                item.IsActive = item.PrimaryKey == viewModel.PrimaryKey;

            DbProvider.SaveChanges();
            SetSaveChangesMessage(SaveChangesMessageType.RecordActivated);
            return RedirectToAction("Summary", new { hk = viewModel.ShortHeaderKey, pk = viewModel.ShortPrimaryKey });
        }
        #endregion

        #region Delete
        // GET: /Teams/Editor/Delete
        [Route("Delete/{hk}/{pk}")]
        public async Task<ActionResult> Delete(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<OrganisationEditorViewModel>(pk, hk);
            viewModel.GetEntityData();

            var activeVersion = viewModel.VersionEntity.Organisation.GetApprovedVersion<OrganisationV>(viewModel.ViewDate); ;

            if (viewModel.VersionEntity.CanDelete(User))
            {
                DbProvider.Remove(viewModel.VersionEntity);
                SetSaveChangesMessage(SaveChangesMessageType.RecordDeleted);
                DbProvider.SaveChanges();
            }

            return RedirectToAction("Summary", new { hk = activeVersion.HeaderKey.ToShortGuid(), pk = activeVersion.PrimaryKey.ToShortGuid() });
        }
        #endregion
    }
}