using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using FootballOracle.Models.Membership;
using FootballOracle.Models.Migrations.Membership;

namespace FootballOracle.Models.DbContexts.Membership
{
    // Enable-Migrations -ContextTypeName MembershipDbContext -MigrationsDirectory Migrations\Membership

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MembershipDbContext : IdentityDbContext<ApplicationUser>
    {
        public MembershipDbContext()
            : base("MembershipConnection", throwIfV1Schema: false)
        {
        }

        static MembershipDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<MembershipDbContext>(new MembershipDbInitialiser());
        }

        public static MembershipDbContext Create()
        {
            return new MembershipDbContext();
        }
    }
}
