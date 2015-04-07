using FootballOracle.Foundation;
using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FootballOracle.Website.Controllers
{
    public abstract class ApprovableController<THeader, TVersion> : BaseController
        where THeader : IHeaderEntity<TVersion>
        where TVersion : BaseApprovableEntity, IApprovableEntity
    {
        #region Constructor
        public ApprovableController(IRepositoryProvider provider, AreaType areaType)
            : base(provider)
        {
            this.AreaType = areaType;
        }
        #endregion

        protected AreaType AreaType { get; private set; }
        protected abstract Task<TViewModel> GetViewModel<TViewModel>(Guid headerKey, DateTime viewDate)
            where TViewModel : IApprovableViewModel<THeader, TVersion>, new();

        protected abstract Task<TViewModel> GetViewModel<TViewModel>(Guid primaryKey, Guid headerKey)
            where TViewModel : IApprovableViewModel<THeader, TVersion>, new();
        
        protected virtual async Task SetTabVisibility(IApprovableViewModel<THeader, TVersion> viewModel)
        {
            await Task.FromResult<object>(null);
        }

        protected async Task<TViewModel> SetModels<TViewModel>(string hk, DateTime? dt)
            where TViewModel : IApprovableViewModel<THeader, TVersion>, new()
        {
            var headerKey = hk.ToGuid();
            var viewDate = dt ?? DateTime.Now;
            var approvableViewModel = await GetViewModel<TViewModel>(headerKey, viewDate);
            approvableViewModel.Breadcrumbs = GetBreadcrumbs(AreaType, approvableViewModel.VersionEntity, approvableViewModel.ViewDate);
            approvableViewModel.User = User;
            approvableViewModel.DbProvider = DbProvider;
            await SetTabVisibility(approvableViewModel);

            return approvableViewModel;
        }

        protected async Task<TViewModel> SetModelsByPrimaryKey<TViewModel>(string pk, string hk)
            where TViewModel : IApprovableViewModel<THeader, TVersion>, new()
        {
            var primaryKey = pk.ToGuid();
            var headerKey = hk.ToGuid();
            var approvableViewModel = await GetViewModel<TViewModel>(primaryKey, headerKey);
            approvableViewModel.Breadcrumbs = GetBreadcrumbs(AreaType, approvableViewModel.VersionEntity, approvableViewModel.ViewDate);
            approvableViewModel.User = User;
            approvableViewModel.DbProvider = DbProvider;
            await SetTabVisibility(approvableViewModel);

            return approvableViewModel;
        }

        protected async Task SetModels<TViewModel>(TViewModel viewModel)
            where TViewModel : IApprovableViewModel<THeader, TVersion>, new()
        {
            var approvableViewModel = await GetViewModel<TViewModel>(viewModel.HeaderKey, viewModel.ViewDate);
            viewModel.VersionEntity = approvableViewModel.VersionEntity;
            viewModel.Breadcrumbs = GetBreadcrumbs(AreaType, viewModel.VersionEntity, DateTime.Now);
            viewModel.User = User;
            viewModel.DbProvider = DbProvider;
            await SetTabVisibility(viewModel);
        }

        protected async Task SetModelsByPrimaryKey<TViewModel>(TViewModel viewModel)
            where TViewModel : IApprovableViewModel<THeader, TVersion>, new()
        {
            var approvableViewModel = await GetViewModel<TViewModel>(viewModel.PrimaryKey, viewModel.HeaderKey);

            viewModel.VersionEntity = approvableViewModel.VersionEntity;
            viewModel.Breadcrumbs = GetBreadcrumbs(AreaType, viewModel.VersionEntity, DateTime.Now);
            viewModel.User = User;
            viewModel.DbProvider = DbProvider;
            await SetTabVisibility(viewModel);
        }

        protected IList<BreadcrumbViewModel> GetBreadcrumbs(AreaType area, TVersion entityV, DateTime viewDate)
        {
            var breadcrumbs = new List<BreadcrumbViewModel>();

            switch (area)
            {
                case AreaType.Cmp:
                    breadcrumbs.CompetitionBreadcrumb(entityV as CompetitionV, viewDate);
                    break;

                case AreaType.Cnt:
                    breadcrumbs.CountryBreadcrumb(entityV as CountryV, viewDate);
                    break;

                case AreaType.Org:
                    breadcrumbs.OrganisationBreadcrumb(entityV as OrganisationV, viewDate);
                    break;

                case AreaType.Ppl:
                    breadcrumbs.PersonBreadcrumb(entityV as PersonV, viewDate);
                    break;

                case AreaType.Mtc:
                    breadcrumbs.MatchBreadcrumb(entityV as MatchV, viewDate);
                    break;

                case AreaType.Tms:
                    breadcrumbs.TeamBreadcrumb(entityV as TeamV, viewDate);
                    break;

                case AreaType.Ven:
                    breadcrumbs.VenueBreadcrumb(entityV as VenueV, viewDate);
                    break;
            }

            return breadcrumbs;
        }

        protected RedirectToRouteResult ApprovableRedirect(string actionName, string hk, DateTime? dt)
        {
            if (dt != null)
                return RedirectToAction(actionName, new { hk = hk, dt = ((DateTime)dt).ToUrlString() });
            else
                return RedirectToAction(actionName, new { hk = hk });
        }

        protected RedirectToRouteResult RedirectToEditor(string actionName, IApprovableViewModel<THeader, TVersion> viewModel)
        {
            return RedirectToAction(actionName, "Editor", new { hk = viewModel.ShortHeaderKey, pk = viewModel.ShortPrimaryKey });
        }

        protected RedirectToRouteResult RedirectToEditor(string actionName, IApprovableEntity entity)
        {
            return RedirectToAction(actionName, "Editor", new { hk = entity.HeaderKey.ToShortGuid(), pk = entity.PrimaryKey.ToShortGuid() });
        }
    }
}