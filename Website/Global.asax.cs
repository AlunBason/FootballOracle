using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FootballOracle.Models.DbContexts.Membership;
using FootballOracle.Models.DbContexts.Pf;
using Website;

namespace FootballOracle.Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Bootstrapper.Initialise();

            var membershipDbContext = new MembershipDbContext();
            membershipDbContext.Database.Initialize(false);

            var pfDbContext = new PfDbContext();
            pfDbContext.Database.Initialize(false);
        }
    }
}
