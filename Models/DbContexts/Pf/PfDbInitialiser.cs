using System.Data.Entity;
using FootballOracle.Models.Migrations.Pf;

namespace FootballOracle.Models.DbContexts.Pf
{
    public class PfDbInitialiser : MigrateDatabaseToLatestVersion<PfDbContext, Configuration>
    {
    }
}
