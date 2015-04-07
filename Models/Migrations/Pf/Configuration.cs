namespace FootballOracle.Models.Migrations.Pf
{
    using System.Data.Entity.Migrations;
    using FootballOracle.Models.DbContexts.Pf;
    using MySql.Data.Entity;

    // Add-Migration -ConfigurationTypeName FootballOracle.Models.Migrations.Pf.Configuration "InitialDatabaseCreation"
    // Update-Database -ConfigurationTypeName FootballOracle.Models.Migrations.Pf.Configuration

    public sealed class Configuration : DbMigrationsConfiguration<PfDbContext>
    {
        public Configuration()
        {
            CodeGenerator = new MySqlMigrationCodeGenerator();
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySqlMigrationSqlGenerator());
            MigrationsDirectory = @"Migrations\Pf";
        }
    }
}
