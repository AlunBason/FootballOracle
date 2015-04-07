using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using FootballOracle.Foundation;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.RepositoryProviders.Interfaces;

namespace FootballOracle.Website.Controllers
{
    public abstract class BaseController : Controller
    {
        public BaseController(IRepositoryProvider dbProvider)
        {
            DbProvider = dbProvider;
        }

        protected IRepositoryProvider DbProvider;

        protected Guid UserId 
        {
            get { return User.Identity.IsAuthenticated ? new Guid(User.Identity.GetUserId()) : Guid.Empty; }
        }

        protected bool IsAdmin 
        {
            get { return User.IsAdmin(); }
        }

        protected void SetSaveChangesMessage(SaveChangesMessageType saveChangesMessageType)
        {
            TempData["SaveChangesMessageType"] = saveChangesMessageType;
        }

        protected ActionResult RedirectToDetailsIndex(Guid hk)
        {
            return RedirectToAction("Index", "Details", new { hk = hk.ToShortGuid() });
        }

        protected ActionResult RedirectHome()
        {
            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }
        
        protected string LogFilePath
        {
            get { return Server.MapPath("~/Logs"); }
        }
	}
}