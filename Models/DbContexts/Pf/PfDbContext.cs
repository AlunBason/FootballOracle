using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MySql.Data.Entity;
using FootballOracle.Models.Entities;
using FootballOracle.Models.Migrations.Pf;

namespace FootballOracle.Models.DbContexts.Pf
{
    // Enable-Migrations -ContextTypeName PfDbContext -MigrationsDirectory Migrations\Pf

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class PfDbContext : DbContext
    {
        public PfDbContext()
            : base("PfDbConnection")
        {
        }

        static PfDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<PfDbContext>(new PfDbInitialiser());
        }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignStage> CampaignStages { get; set; }

        public DbSet<Competition> Competitions { get; set; }
        public DbSet<CompetitionV> CompetitionVs { get; set; }

        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryV> CountryVs { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<LookupCampaignStage> LookupCampaignStages { get; set; }
        public DbSet<LookupCompetition> LookupCompetitions { get; set; }
        public DbSet<LookupMatch> LookupMatches { get; set; }
        public DbSet<LookupPerson> LookupPersons { get; set; }
        public DbSet<LookupTeam> LookupTeams { get; set; }
        public DbSet<LookupVenue> LookupVenues { get; set; }

        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchV> MatchVs { get; set; }

        public DbSet<MatchEvent> MatchEvents { get; set; }

        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<OrganisationV> OrganisationVs { get; set; }

        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonV> PersonVs { get; set; }

        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamV> TeamVs { get; set; }

        public DbSet<TeamName> TeamNames { get; set; }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<VenueV> VenueVs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Campaign>().HasRequired(c => c.Competition).WithMany(c => c.Campaigns).HasForeignKey(c => c.CompetitionKey);

            modelBuilder.Entity<CampaignStage>().HasRequired(c => c.Campaign).WithMany(c => c.CampaignStages).HasForeignKey(c => c.CampaignKey);

            modelBuilder.Entity<CompetitionV>().HasRequired(c => c.Competition).WithMany(c => c.Versions).HasForeignKey(c => c.HeaderKey);
            modelBuilder.Entity<CompetitionV>().HasRequired(c => c.Organisation).WithMany(o => o.CompetitionVs).HasForeignKey(c => c.OrganisationGuid);

            modelBuilder.Entity<CountryV>().HasRequired(c => c.Country).WithMany(c => c.Versions).HasForeignKey(c => c.HeaderKey);
            modelBuilder.Entity<CountryV>().HasRequired(c => c.Organisation).WithMany(o => o.CountryVs).HasForeignKey(c => c.OrganisationGuid);
            modelBuilder.Entity<CountryV>().HasOptional(t => t.Resource).WithMany(c => c.CountryVs).HasForeignKey(t => t.ResourceGuid);

            modelBuilder.Entity<LookupCampaignStage>().HasRequired(c => c.CampaignStage).WithMany(c => c.LookupCampaignStages).HasForeignKey(c => c.CampaignStageKey);
            modelBuilder.Entity<LookupCompetition>().HasRequired(c => c.Competition).WithMany(c => c.LookupCompetitions).HasForeignKey(c => c.CompetitionGuid);
            modelBuilder.Entity<LookupMatch>().HasRequired(c => c.Match).WithMany(m => m.LookupMatches).HasForeignKey(c => c.MatchGuid);
            modelBuilder.Entity<LookupPerson>().HasRequired(c => c.Person).WithMany(p => p.LookupPeople).HasForeignKey(c => c.PersonGuid);
            modelBuilder.Entity<LookupTeam>().HasRequired(c => c.Team).WithMany(c => c.LookupTeams).HasForeignKey(c => c.TeamGuid);
            modelBuilder.Entity<LookupVenue>().HasRequired(c => c.Venue).WithMany(v => v.LookupVenues).HasForeignKey(c => c.VenueGuid);

            modelBuilder.Entity<MatchV>().HasRequired(m => m.CampaignStage).WithMany(c => c.MatchVs).HasForeignKey(m => m.CampaignStageKey);
            modelBuilder.Entity<MatchV>().HasRequired(m => m.Match).WithMany(m => m.Versions).HasForeignKey(m => m.HeaderKey);
            modelBuilder.Entity<MatchV>().HasRequired(m => m.Team1).WithMany(t => t.Team1MatchVs).HasForeignKey(m => m.Team1Guid);
            modelBuilder.Entity<MatchV>().HasRequired(m => m.Team2).WithMany(t => t.Team2MatchVs).HasForeignKey(m => m.Team2Guid).WillCascadeOnDelete(false);
            modelBuilder.Entity<MatchV>().HasOptional(m => m.Venue).WithMany(c => c.MatchVs).HasForeignKey(m => m.VenueGuid);

            modelBuilder.Entity<MatchEvent>().HasRequired(m => m.MatchV).WithMany(m => m.MatchEvents).HasForeignKey(m => m.MatchVPrimaryKey);
            modelBuilder.Entity<MatchEvent>().HasRequired(m => m.Person).WithMany(m => m.MatchEvents).HasForeignKey(m => m.PersonPrimaryKey);
            modelBuilder.Entity<MatchEvent>().HasRequired(m => m.Team).WithMany(m => m.MatchEvents).HasForeignKey(m => m.TeamPrimaryKey);

            modelBuilder.Entity<OrganisationV>().HasOptional(o => o.Country).WithMany(c => c.OrganisationVs).HasForeignKey(o => o.CountryGuid);
            modelBuilder.Entity<OrganisationV>().HasRequired(o => o.Organisation).WithMany(o => o.Versions).HasForeignKey(o => o.HeaderKey);
            modelBuilder.Entity<OrganisationV>().HasOptional(o => o.ParentOrganisation).WithMany(o => o.ParentOrganisationVs).HasForeignKey(o => o.ParentOrganisationGuid);

            modelBuilder.Entity<PersonV>().HasRequired(c => c.Person).WithMany(c => c.Versions).HasForeignKey(c => c.HeaderKey);
            modelBuilder.Entity<PersonV>().HasOptional(c => c.Country).WithMany(o => o.PersonVs).HasForeignKey(c => c.CountryGuid);

            modelBuilder.Entity<TeamV>().HasOptional(t => t.Country).WithMany(c => c.TeamVs).HasForeignKey(t => t.CountryGuid);
            modelBuilder.Entity<TeamV>().HasOptional(t => t.Resource).WithMany(c => c.TeamVs).HasForeignKey(t => t.ResourceGuid);
            modelBuilder.Entity<TeamV>().HasRequired(t => t.Team).WithMany(t => t.Versions).HasForeignKey(t => t.HeaderKey);
            modelBuilder.Entity<TeamV>().HasOptional(t => t.HomeVenue).WithMany(t => t.TeamVs).HasForeignKey(t => t.HomeVenueGuid);

            modelBuilder.Entity<TeamName>().HasRequired(t => t.TeamV).WithMany(t => t.TeamNames).HasForeignKey(t => t.TeamVKey);

            modelBuilder.Entity<VenueV>().HasRequired(v => v.Country).WithMany(c => c.VenueVs).HasForeignKey(v => v.CountryGuid);
            modelBuilder.Entity<VenueV>().HasRequired(v => v.Venue).WithMany(v => v.Versions).HasForeignKey(v => v.HeaderKey);
        }
    }
}
