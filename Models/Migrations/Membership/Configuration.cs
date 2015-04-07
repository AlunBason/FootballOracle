namespace FootballOracle.Models.Migrations.Membership
{
    using System.Data.Entity.Migrations;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using FootballOracle.Models.DbContexts.Membership;
    using FootballOracle.Models.Membership;
    using MySql.Data.Entity;

    // Add-Migration -ConfigurationTypeName FootballOracle.Models.Migrations.Membership.Configuration "InitialDatabaseCreation"
    // Update-Database -ConfigurationTypeName FootballOracle.Models.Migrations.Membership.Configuration

    public sealed class Configuration : DbMigrationsConfiguration<MembershipDbContext>
    {
        public Configuration()
        {
            CodeGenerator = new MySqlMigrationCodeGenerator();
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySqlMigrationSqlGenerator());
            MigrationsDirectory = @"Migrations\Membership";
        }

        protected override void Seed(MembershipDbContext context)
        {
            InitializeIdentityForEF(context);
        }

        private void InitializeIdentityForEF(MembershipDbContext db)
        {
            var roleStore = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            const string name = "administrator@footballoracle.co.uk";
            const string password = "S3pt197!";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name, EmailConfirmed = true };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }
    }
}
