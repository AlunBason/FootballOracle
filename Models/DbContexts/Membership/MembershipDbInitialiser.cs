using System.Data.Entity;
using FootballOracle.Models.Migrations.Membership;

namespace FootballOracle.Models.DbContexts.Membership
{
    public class MembershipDbInitialiser : MigrateDatabaseToLatestVersion<MembershipDbContext, Configuration>
    {
    }
}
