using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using FootballOracle.Website.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FootballOracle.Website.Areas.Cnt.Controllers
{
    [RouteArea("Cnt")]
    [RoutePrefix("Editor")]
    [Authorize]
    public class EditorController : BaseCountryController
    {
        #region Constructor
        public EditorController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region Summary
        // GET: /Countries/Editor/Summary
        [Route("Summary/{hk}/{pk}")]
        public async Task<ActionResult> Summary(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<CountryVersionSummaryViewModel>(pk, hk);
            viewModel.GetEntityData();
            viewModel.EditableViewModels = viewModel.VersionEntity.Country.GetEditableVersions(DateTime.Now);

            return View(viewModel);
        }
        #endregion

        #region Edit
        // GET: /Countries/Editor/Edit
        [Route("Edit/{hk}/{pk}")]
        public async Task<ActionResult> Edit(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<CountryEditorViewModel>(pk, hk);
            viewModel.GetEntityData();
            viewModel.EditableViewModels = viewModel.VersionEntity.Country.GetEditableVersions(DateTime.Now);

            return View(viewModel);
        }

        [HttpPost]
        [Route("Edit/{hk}/{pk}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CountryEditorViewModel viewModel, HttpPostedFileBase imageData)
        {
            await SetModelsByPrimaryKey(viewModel);
            viewModel.EditableViewModels = viewModel.VersionEntity.Country.GetEditableVersions(DateTime.Now);

            var validate = DoEditValidation(viewModel);

            if (validate != null)
                return validate;

            var resourceGuid = AddImageData(imageData);

            if (resourceGuid != null)
                viewModel.ResourceGuid = resourceGuid;

            var query = viewModel.HeaderEntity.GetApprovedVersionsByEffectiveDate<Country, CountryV>(viewModel.EffectiveFrom);

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

                    var newEntityV = viewModel.ToCountryV(entityV.OwnerUserId, UserId);
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

                var newEntityV = viewModel.ToCountryV(UserId, UserId);
                newEntityV.EffectiveTo = Date.HighDate;

                DbProvider.Add(newEntityV);
                (await DbProvider.GetCountry(viewModel.HeaderKey, viewModel.ViewDate)).Country.NormaliseVersionEffectiveDates<CountryV>();
                DbProvider.SaveChanges();
                SetSaveChangesMessage(SaveChangesMessageType.ChangesSaved);
                return RedirectToEditor("Summary", newEntityV);
            }
        }

        private ViewResult DoEditValidation(CountryEditorViewModel viewModel)
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
        // GET: /Countries/Editor/Activate
        [Route("Activate/{hk}/{pk}")]
        public async Task<ActionResult> Activate(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<CountryEditorViewModel>(pk, hk);
            viewModel.GetEntityData();

            var entityVs = viewModel.HeaderEntity.GetApprovedVersionsByEffectiveDate<Country, CountryV>(viewModel.VersionEntity.EffectiveFrom);

            foreach (var item in entityVs)
                item.IsActive = item.PrimaryKey == viewModel.PrimaryKey;

            DbProvider.SaveChanges();
            SetSaveChangesMessage(SaveChangesMessageType.RecordActivated);
            return RedirectToEditor("Summary", viewModel);
        }
        #endregion

        #region Delete
        // GET: /Countries/Editor/Delete
        [Route("Delete/{hk}/{pk}")]
        public async Task<ActionResult> Delete(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<CountryEditorViewModel>(pk, hk);
            viewModel.GetEntityData();

            var activeEntity = viewModel.VersionEntity.Country.GetApprovedVersion<CountryV>(viewModel.ViewDate);

            if (viewModel.VersionEntity.CanDelete(User))
            {
                DbProvider.Remove(viewModel.VersionEntity);
                SetSaveChangesMessage(SaveChangesMessageType.RecordDeleted);
                DbProvider.SaveChanges();
            }

            return RedirectToEditor("Summary", activeEntity);
        }
        #endregion
    }
}