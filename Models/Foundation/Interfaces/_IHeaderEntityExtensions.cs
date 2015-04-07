using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Interfaces;
using FootballOracle.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballOracle.Foundation.Interfaces
{
    public static class _IHeaderEntityExtensions
    {
        public static IEnumerable<TVersion> GetApprovedVersions<TVersion>(this IEnumerable<IHeaderEntity<TVersion>> headers, DateTime viewDate)
            where TVersion : IApprovableEntity
        {
            foreach (var header in headers)
                yield return header.GetApprovedVersion<TVersion>(viewDate);
        }

        public static TVersion GetApprovedVersion<TVersion>(this IHeaderEntity<TVersion> header, DateTime viewDate)
            where TVersion : IApprovableEntity
        {
            return header.GetApprovedVersions<TVersion>().SingleOrDefault(h => h.EffectiveFrom <= viewDate && h.EffectiveTo >= viewDate);
        }

        public static IEnumerable<TVersion> GetApprovedVersions<TVersion>(this IHeaderEntity<TVersion> header)
            where TVersion : IApprovableEntity
        {
            return header.Versions.Where(h => h.IsActive);
        }

        public static TViewModel ToViewModel<TViewModel, THeader, TVersion>(this IHeaderEntity<TVersion> header, DateTime viewDate)
            where THeader : IHeaderEntity<TVersion>
            where TVersion : IApprovableEntity
            where TViewModel : IApprovableViewModel<THeader, TVersion>, new()
        {
            var version = header.GetApprovedVersion<TVersion>(viewDate);

            return version != null ? version.ToViewModel<TViewModel, THeader, TVersion>(viewDate) : default(TViewModel);
        }

        public static void NormaliseVersionEffectiveDates<TVersion>(this IHeaderEntity<TVersion> header, DateTime? latestEffectiveToDate = null)
            where TVersion : IApprovableEntity
        {
            var versions = header.GetApprovedVersions<TVersion>();

            DateTime? previousEffectiveFrom = null;

            foreach (var item in versions.OrderByDescending(v => v.EffectiveFrom))
            {
                if (previousEffectiveFrom != null)
                    item.EffectiveTo = ((DateTime)previousEffectiveFrom).AddSeconds(-1);

                previousEffectiveFrom = item.EffectiveFrom;
            }

            if (latestEffectiveToDate != null)
                versions.OrderByDescending(v => v.EffectiveFrom).First().EffectiveTo = (DateTime)latestEffectiveToDate;

        }

        public static IEnumerable<TVersion> GetApprovedVersionsByEffectiveDate<THeader, TVersion>(this THeader headerEntity, DateTime effectiveFrom)
            where THeader : IHeaderEntity<TVersion>
            where TVersion : IApprovableEntity
        {
            return headerEntity.Versions.Where(w => w.EffectiveFrom == effectiveFrom && w.IsActive);
        }
    }
}
