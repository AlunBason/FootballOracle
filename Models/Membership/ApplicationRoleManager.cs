using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using FootballOracle.Models.DbContexts;
using FootballOracle.Models.DbContexts.Membership;

namespace FootballOracle.Models.Membership
{
    // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var manager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<MembershipDbContext>()));

            return manager;
        }
    }
}
