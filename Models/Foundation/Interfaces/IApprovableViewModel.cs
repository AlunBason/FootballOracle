using System;
using System.Collections.Generic;
using System.Security.Principal;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.RepositoryProviders.Interfaces;

namespace FootballOracle.Models.Interfaces
{
    public interface IApprovableViewModel<THeader, TVersion>
        where THeader : IHeaderEntity<TVersion>
        where TVersion : IApprovableEntity
    {
        IPrincipal User { get; set; }
        IRepositoryProvider DbProvider { get; set; }
        Guid PrimaryKey { get; set; }
        string ShortPrimaryKey { get; }
        string ShortHeaderKey { get; }
        DateTime ViewDate { get; set; }
        Guid HeaderKey { get; set; }
        DateTime EffectiveFrom { get; set; }
        DateTime EffectiveTo { get; set; }
        string WebAddress { get; set; }
        TVersion VersionEntity { get; set; }
        THeader HeaderEntity { get; }

        IList<BreadcrumbViewModel> Breadcrumbs { get; set; }

        bool IsParentDisplayed { get; set; }
    }
}
