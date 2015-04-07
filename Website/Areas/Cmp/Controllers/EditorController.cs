using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Competitions;
using FootballOracle.Models.ViewModels.Standard;
using FootballOracle.Website.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FootballOracle.Models.ViewModels.Standard.Campaigns;

namespace FootballOracle.Website.Areas.Cmp.Controllers
{
    [RouteArea("Cmp")]
    [RoutePrefix("Editor")]
    [Authorize]
    public class EditorController : BaseCompetitionController
    {
        #region Constructor
        public EditorController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region Create
        // GET: /Competitions/Editor/Create
        [HttpGet, Route("Create")]
        public ActionResult Create()
        {
            return View(CompetitionEditorViewModel.CreateNew<CompetitionEditorViewModel>());
        }

        [ValidateAntiForgeryToken]
        // POST: /Competitions/Editor/Create
        [HttpPost, Route("Create")]
        public ActionResult Create(CompetitionEditorViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var competitionV = viewModel.ToCompetitionV(UserId, UserId);
            competitionV.CampaignPeriodType = PeriodType.Year;
            competitionV.CampaignPeriodValue = 1;

            DbProvider.Add(new Competition() { PrimaryKey = viewModel.HeaderKey });
            DbProvider.Add(competitionV);
            DbProvider.SaveChanges();

            return RedirectToDetailsIndex(competitionV.HeaderKey);
        }
        #endregion

        #region Summary
        // GET: /Competitions/Editor/Summary
        [Route("Summary/{hk}/{pk}")]
        public async Task<ActionResult> Summary(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<CompetitionVersionSummaryViewModel>(pk, hk);
            viewModel.GetEntityData();
            viewModel.EditableViewModels = viewModel.VersionEntity.Competition.GetEditableVersions(viewModel.ViewDate);

            return View(viewModel);
        }
        #endregion

        #region Edit
        // GET: /Competitions/Editor/Edit
        [Route("Edit/{hk}/{pk}")]
        public async Task<ActionResult> Edit(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<CompetitionEditorViewModel>(pk, hk);
            viewModel.GetEntityData();
            viewModel.EditableViewModels = viewModel.VersionEntity.Competition.GetEditableVersions(viewModel.ViewDate);

            return View(viewModel);
        }

        [HttpPost]
        [Route("Edit/{hk}/{pk}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CompetitionEditorViewModel viewModel)
        {
            await SetModelsByPrimaryKey(viewModel);
            viewModel.EditableViewModels = viewModel.VersionEntity.Competition.GetEditableVersions(viewModel.ViewDate);

            var validate = DoEditValidation(viewModel);

            if (validate != null)
                return validate;

            var query = viewModel.HeaderEntity.GetApprovedVersionsByEffectiveDate<Competition, CompetitionV>(viewModel.EffectiveFrom);

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

                    var newEntityV = viewModel.ToCompetitionV(entityV.OwnerUserId, UserId);
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

                var newEntityV = viewModel.ToCompetitionV(UserId, UserId);
                newEntityV.EffectiveTo = Date.HighDate;

                DbProvider.Add(newEntityV);
                var competitionV = await DbProvider.GetCompetition(viewModel.HeaderKey, viewModel.ViewDate);
                competitionV.Competition.NormaliseVersionEffectiveDates<CompetitionV>();
                DbProvider.SaveChanges();

                return RedirectToEditor("Summary", newEntityV);
            }
        }

        private ViewResult DoEditValidation(CompetitionEditorViewModel viewModel)
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
            var viewModel = await SetModelsByPrimaryKey<CompetitionEditorViewModel>(hk, pk);
            viewModel.GetEntityData();

            var entityVs = viewModel.HeaderEntity.Versions.Where(o => o.HeaderKey == viewModel.HeaderKey && o.EffectiveFrom == viewModel.VersionEntity.EffectiveFrom);

            foreach (var item in entityVs)
                item.IsActive = item.PrimaryKey == viewModel.PrimaryKey;

            DbProvider.SaveChanges();

            return RedirectToEditor("Summary", viewModel);
        }
        #endregion

        #region Delete
        // GET: /Competition/Editor/Delete
        [Route("Delete/{hk}/{pk}")]
        public async Task<ActionResult> Delete(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<CompetitionEditorViewModel>(pk, hk);
            viewModel.GetEntityData();

            var activeEntity = viewModel.VersionEntity.Competition.GetApprovedVersion<CompetitionV>(viewModel.ViewDate);

            if (viewModel.VersionEntity.CanDelete(User))
            {
                DbProvider.Remove(viewModel.VersionEntity);
                SetSaveChangesMessage(SaveChangesMessageType.RecordDeleted);
                DbProvider.SaveChanges();
            }

            return RedirectToEditor("Summary", activeEntity);
        }
        #endregion

        #region ManageLookups
        // GET: /Teams/Editor/ManageLookups
        [Route("ManageLookups/{hk}/{pk}")]
        public async Task<ActionResult> ManageLookups(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<CompetitionManageLookupsViewModel>(pk, hk);

            viewModel.LookupCompetitionViewModels = new List<LookupCompetitionViewModel>();

            var lookups = await DbProvider.GetLookupCompetitionByCompetitionKey(viewModel.HeaderKey);

            foreach (var lookup in lookups)
            {
                viewModel.LookupCompetitionViewModels.Add(new LookupCompetitionViewModel()
                {
                    PrimaryKey = lookup.PrimaryKey,
                    ImportSite = lookup.ImportSite,
                    CompetitionGuid = lookup.CompetitionGuid,
                    LookupId = lookup.LookupId
                });
            }

            viewModel.LookupCompetitionViewModels.Add(new LookupCompetitionViewModel()
            {
                PrimaryKey = Guid.NewGuid(),
                ImportSite = null,
                CompetitionGuid = viewModel.HeaderKey,
                LookupId = string.Empty
            });


            return View(viewModel);
        }

        [HttpPost]
        [Route("ManageLookups/{hk}/{pk}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageLookups(CompetitionManageLookupsViewModel viewModel)
        {
            foreach (var lookup in viewModel.LookupCompetitionViewModels)
            {
                var lookupEntity = await DbProvider.GetLookupCompetitionByPrimaryKey(lookup.PrimaryKey);

                if (lookupEntity != null)
                {
                    if (lookup.ImportSite == null)
                        DbProvider.Remove(lookupEntity);
                    else
                    {
                        lookupEntity.ImportSite = (ImportSite)lookup.ImportSite;
                        lookupEntity.LookupId = lookup.LookupId.Trim();
                    }
                }
                else
                {
                    if (lookup.ImportSite != null)
                    {
                        DbProvider.Add(new LookupCompetition()
                        {
                            PrimaryKey = Guid.NewGuid(),
                            ImportSite = (ImportSite)lookup.ImportSite,
                            CompetitionGuid = lookup.CompetitionGuid,
                            LookupId = lookup.LookupId.Trim()
                        });
                    }
                }
            }
            DbProvider.SaveChanges();

            return RedirectToAction("ManageLookups", new { pk = viewModel.ShortPrimaryKey });
        }
        #endregion

        #region ManageCampaigns
        // GET: /Teams/Editor/ManageCampaigns
        [Route("ManageCampaigns/{hk}/{pk}")]
        public async Task<ActionResult> ManageCampaigns(string hk, string pk)
        {
            var viewModel = await SetModelsByPrimaryKey<CompetitionManageCampaignsViewModel>(pk, hk);

            viewModel.ManageCampaignViewModels = new List<ManageCampaignViewModel>();

            foreach (var item in viewModel.HeaderEntity.Campaigns.OrderByDescending(o => o.StartDate))
            {
                viewModel.ManageCampaignViewModels.Add(new ManageCampaignViewModel()
                {
                    CampaignKey = item.PrimaryKey,
                    CompetitionKey = viewModel.HeaderKey,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    IsNew = false
                });
            }

            viewModel.ManageCampaignViewModels.Add(new ManageCampaignViewModel()
            {
                CampaignKey = Guid.NewGuid(),
                CompetitionKey = viewModel.HeaderKey,
                StartDate = null,
                EndDate = null,
                IsNew = true
            });

            return View(viewModel);
        }

        [HttpPost]
        [Route("ManageCampaigns/{hk}/{pk}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageCampaigns(CompetitionManageCampaignsViewModel viewModel)
        {
            await SetModelsByPrimaryKey(viewModel);

            foreach (var item in viewModel.ManageCampaignViewModels)
            {
                if (!item.IsNew)
                {
                    var campaign = await DbProvider.GetCampaign(item.CampaignKey);

                    if (campaign == null)
                        continue;

                    if (item.StartDate == null && item.EndDate == null)
                        DbProvider.Remove(campaign);
                    else
                    {
                        item.StartDate = campaign.StartDate;
                        item.EndDate = campaign.EndDate;
                    }
                }
                else
                {
                    if (item.StartDate != null && item.EndDate != null)
                    {
                        DbProvider.Add(new Campaign()
                        {
                            PrimaryKey = Guid.NewGuid(),
                            CompetitionKey = viewModel.HeaderKey,
                            StartDate = (DateTime)item.StartDate,
                            EndDate = (DateTime)item.EndDate
                        });
                    }
                }
            }
            DbProvider.SaveChanges();

            return RedirectToAction("ManageCampaigns", new { pk = viewModel.ShortPrimaryKey });
        }
        #endregion

        #region ManageCampaignStages
        // GET: /Teams/Editor/ManageCampaignStages
        [Route("ManageCampaignStages/{hk}/{pk}/{ck}")]
        public async Task<ActionResult> ManageCampaignStages(string hk, string pk, string ck)
        {
            var viewModel = await SetModelsByPrimaryKey<CompetitionCampaignStagesViewModel>(pk, hk);
            viewModel.SelectedCampaignKey = ck.ToGuid();

            var campaign = await DbProvider.GetCampaign(viewModel.SelectedCampaignKey);

            viewModel.ManageCampaignViewModel = new ManageCampaignViewModel()
            {
                CampaignKey = campaign.PrimaryKey,
                CompetitionKey = campaign.CompetitionKey,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate
            };

            viewModel.ManageCampaignStageViewModels = new List<ManageCampaignStageViewModel>();

            foreach (var item in campaign.CampaignStages.OrderBy(o => o.StageOrder).ThenBy(t => t.Description))
            {
                viewModel.ManageCampaignStageViewModels.Add(new ManageCampaignStageViewModel()
                {
                    PrimaryKey = item.PrimaryKey,
                    CampaignKey = item.CampaignKey,
                    Description = item.Description,
                    StageOrder = item.StageOrder,
                    IsDefault = item.IsDefault,
                    IsLeague = item.IsLeague,
                    LegCount = item.LegCount
                });
            }

            viewModel.ManageCampaignStageViewModels.Add(new ManageCampaignStageViewModel()
            {
                PrimaryKey = Guid.NewGuid(),
                CampaignKey = viewModel.ManageCampaignViewModel.CampaignKey,
                IsNew = true
            });

            return View(viewModel);
        }

        [HttpPost]
        [Route("ManageCampaignStages/{hk}/{pk}/{ck}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageCampaignStages(CompetitionCampaignStagesViewModel viewModel)
        {
            await SetModelsByPrimaryKey(viewModel);

            foreach (var item in viewModel.ManageCampaignStageViewModels)
            {
                if (item.IsNew)
                {
                    if (!string.IsNullOrWhiteSpace(item.Description))
                    {
                        DbProvider.Add(new CampaignStage()
                        {
                            PrimaryKey = Guid.NewGuid(),
                            CampaignKey = viewModel.SelectedCampaignKey,
                            StageOrder = item.StageOrder,
                            Description = item.Description,
                            IsDefault = item.IsDefault,
                            IsLeague = item.IsLeague,
                            LegCount = item.LegCount
                        });
                    }
                }
                else
                {
                    var campaignStage = await DbProvider.GetCampaignStage(item.PrimaryKey);

                    if (string.IsNullOrWhiteSpace(item.Description))
                        DbProvider.Remove(campaignStage);
                    else
                    {
                        campaignStage.StageOrder = item.StageOrder;
                        campaignStage.Description = item.Description;
                        campaignStage.IsDefault = item.IsDefault;
                        campaignStage.IsLeague = item.IsLeague;
                        campaignStage.LegCount = item.LegCount;
                    }
                }
            }

            DbProvider.SaveChanges();

            return RedirectToAction("ManageCampaignStages", new { pk = viewModel.ShortPrimaryKey, hk = viewModel.ShortHeaderKey, ck = viewModel.SelectedCampaignKey.ToShortGuid() });
        }
        #endregion

        #region ManageCampaignStages
        // GET: /Teams/Editor/ManageLookupCampaignStages
        [Route("ManageLookupCampaignStages/{hk}/{pk}/{sk}")]
        public async Task<ActionResult> ManageLookupCampaignStages(string hk, string pk, string sk)
        {
            var viewModel = await SetModelsByPrimaryKey<CompetitionLookupCampaignStagesViewModel>(pk, hk);
            viewModel.SelectedCampaignStageKey = sk.ToGuid();

            var campaignStage = await DbProvider.GetCampaignStage(viewModel.SelectedCampaignStageKey);

            viewModel.SelectedCampaignStageViewModel = new ManageCampaignStageViewModel()
            {
                PrimaryKey = campaignStage.PrimaryKey,
                CampaignKey = campaignStage.CampaignKey,
                Description = campaignStage.Description,
                StageOrder = campaignStage.StageOrder,
                IsDefault = campaignStage.IsDefault,
                IsLeague = campaignStage.IsLeague
            };

            viewModel.ManageLookupCampaignStageViewModels = new List<ManageLookupCampaignStageViewModel>();

            foreach (var item in campaignStage.LookupCampaignStages)
            {
                viewModel.ManageLookupCampaignStageViewModels.Add(new ManageLookupCampaignStageViewModel()
                {
                    PrimaryKey = item.PrimaryKey,
                    CampaignStageKey = item.CampaignStageKey,
                    ImportSite = item.ImportSite,
                    LookupId = item.LookupId
                });
            }

            viewModel.ManageLookupCampaignStageViewModels.Add(new ManageLookupCampaignStageViewModel()
            {
                PrimaryKey = Guid.NewGuid(),
                CampaignStageKey = viewModel.SelectedCampaignStageKey,
                IsNew = true
            });

            return View(viewModel);
        }

        [HttpPost]
        [Route("ManageLookupCampaignStages/{hk}/{pk}/{sk}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageLookupCampaignStages(CompetitionLookupCampaignStagesViewModel viewModel)
        {
            await SetModelsByPrimaryKey(viewModel);

            foreach (var item in viewModel.ManageLookupCampaignStageViewModels)
            {
                if (item.IsNew)
                {
                    if (item.ImportSite != null)
                    {
                        DbProvider.Add(new LookupCampaignStage()
                        {
                            PrimaryKey = Guid.NewGuid(),
                            CampaignStageKey = viewModel.SelectedCampaignStageKey,
                            ImportSite = (ImportSite)item.ImportSite,
                            LookupId = item.LookupId
                        });
                    }
                }
                else
                {
                    var lookupCampaignStage = await DbProvider.GetLookupCampaignStage(item.PrimaryKey);

                    if (item.ImportSite == null)
                        DbProvider.Remove(lookupCampaignStage);
                    else
                    {
                        lookupCampaignStage.ImportSite = (ImportSite)item.ImportSite;
                        lookupCampaignStage.LookupId = item.LookupId;
                    }
                }
            }

            DbProvider.SaveChanges();
            return RedirectToAction("ManageLookupCampaignStages", new { pk = viewModel.ShortPrimaryKey, hk = viewModel.ShortHeaderKey, sk = viewModel.SelectedCampaignStageKey.ToShortGuid() });
        }
        #endregion
    }
}