using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace FootballOracle.Foundation.Interfaces
{
    public static class _IApprovableEntityExtensions
    {
        public static bool CanDelete(this IApprovableEntity entityV, IPrincipal user)
        {
            return (entityV.ModifiedUserId == user.GetUserId()) || user.IsAdmin();
        }

        public static IEnumerable<TViewModel> ToViewModels<TViewModel, THeader, TVersion>(this IEnumerable<TVersion> entityVs, DateTime viewDate)
            where THeader : IHeaderEntity<TVersion>
            where TViewModel : IApprovableViewModel<THeader, TVersion>, new()
            where TVersion : IApprovableEntity
        {
            foreach (var entityV in entityVs)
                yield return entityV.ToViewModel<TViewModel, THeader, TVersion>(viewDate);
        }

        public static TViewModel ToViewModel<TViewModel, THeader, TVersion>(this TVersion entityV, DateTime viewDate)
            where THeader : IHeaderEntity<TVersion>
            where TViewModel : IApprovableViewModel<THeader, TVersion>, new()
            where TVersion : IApprovableEntity
        {
            if (entityV == null)
                return new TViewModel();

            return new TViewModel()
            {
                PrimaryKey = entityV.PrimaryKey,
                ViewDate = viewDate,
                HeaderKey = entityV.HeaderKey,
                WebAddress = entityV.WebAddress,
                EffectiveFrom = entityV.EffectiveFrom,
                EffectiveTo = entityV.EffectiveTo,
                VersionEntity = entityV
            };
        }
    }
}
