using FootballOracle.Models.RepositoryProviders.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Security.Principal;

namespace FootballOracle.Models.ViewModels.Base
{
    public abstract class BaseViewModel
    {
        public IPrincipal User { get; set; }
        public IRepositoryProvider DbProvider { get; set; }
        public DateTime ViewDate { get; set; }
        public string ReferringUrl { get; set; }

        public Guid UserId
        {
            get { return User != null && User.Identity.IsAuthenticated ? new Guid(User.Identity.GetUserId()) : Guid.Empty; }
        }

        public bool IsAdmin
        {
            get { return User.IsAdmin(); }
        }

    }
}
