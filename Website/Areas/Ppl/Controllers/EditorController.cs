using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.People;
using FootballOracle.Website.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FootballOracle.Website.Areas.Ppl.Controllers
{
    [RouteArea("Ppl")]
    [RoutePrefix("Editor")]
    [Authorize]
    public class EditorController : BasePersonController
    {
        #region Constructor
        public EditorController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region Create
        // GET: /People/Editor/Create
        [HttpGet, Route("Create")]
        public ActionResult Create()
        {
            return View(PersonEditorViewModel.CreateNew<PersonEditorViewModel>());
        }

        [ValidateAntiForgeryToken]
        // POST: /People/Editor/Create
        [HttpPost, Route("Create")]
        public ActionResult Create(PersonEditorViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var personV = viewModel.ToPersonV(UserId, UserId);
            DbProvider.Add(Person.Create<Person>());
            DbProvider.Add(personV);
            DbProvider.SaveChanges();

            return RedirectToDetailsIndex(personV.HeaderKey);
        }
        #endregion

        #region Summary
        // GET: /People/Editor/Summary
        [Route("Summary/{hk}/{pk}")]
        public async Task<ActionResult> Summary(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<PersonVersionSummaryViewModel>(pk, hk);
            viewModel.GetEntityData();
            viewModel.EditableViewModels = viewModel.VersionEntity.Person.GetEditableVersions(DateTime.Now);

            return View(viewModel);
        }
        #endregion

        #region Edit
        // GET: /People/Editor/Edit
        [Route("Edit/{hk}/{pk}")]
        public async Task<ActionResult> Edit(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<PersonEditorViewModel>(pk, hk);
            viewModel.GetEntityData();
            viewModel.EditableViewModels = viewModel.VersionEntity.Person.GetEditableVersions(DateTime.Now);

            return View(viewModel);
        }

        [HttpPost]
        [Route("Edit/{hk}/{pk}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PersonEditorViewModel viewModel)
        {
            await SetModelsByPrimaryKey(viewModel);
            viewModel.EditableViewModels = viewModel.VersionEntity.Person.GetEditableVersions(DateTime.Now);

            var validate = DoEditValidation(viewModel);

            if (validate != null)
                return validate;

            var query = viewModel.HeaderEntity.GetApprovedVersionsByEffectiveDate<Person, PersonV>(viewModel.EffectiveFrom);

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

                    var newEntityV = viewModel.ToPersonV(entityV.OwnerUserId, UserId);
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

                var newEntityV = viewModel.ToPersonV(UserId, UserId);
                newEntityV.EffectiveTo = Date.HighDate;

                DbProvider.Add(newEntityV);
                viewModel.HeaderEntity.NormaliseVersionEffectiveDates<PersonV>();
                DbProvider.SaveChanges();

                return RedirectToEditor("Summary", newEntityV);
            }
        }

        private ViewResult DoEditValidation(PersonEditorViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            return null;
        }
        #endregion

        #region Activate
        // GET: /People/Editor/Activate
        [Route("Activate/{hk}/{pk}")]
        public async Task<ActionResult> Activate(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<PersonEditorViewModel>(pk, hk);
            viewModel.GetEntityData();

            var entityVs = viewModel.HeaderEntity.Versions.Where(o => o.HeaderKey == viewModel.HeaderKey && o.EffectiveFrom == viewModel.VersionEntity.EffectiveFrom);

            foreach (var item in entityVs)
                item.IsActive = item.PrimaryKey == viewModel.PrimaryKey;

            DbProvider.SaveChanges();

            return RedirectToEditor("Summary", viewModel);
        }
        #endregion

        #region Delete
        // GET: /People/Editor/Delete
        [Route("Delete/{hk}/{pk}")]
        public async Task<ActionResult> Delete(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<PersonEditorViewModel>(pk, hk);
            viewModel.GetEntityData();

            var activeEntity = viewModel.VersionEntity.Person.GetApprovedVersion<PersonV>(viewModel.ViewDate);

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