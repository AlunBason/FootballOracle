namespace FootballOracle.Models.Migrations.Pf
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabaseCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaign",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        CompetitionKey = c.Guid(nullable: false),
                        StartDate = c.DateTime(nullable: false, precision: 0),
                        EndDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Competition", t => t.CompetitionKey, cascadeDelete: true)
                .Index(t => t.CompetitionKey);
            
            CreateTable(
                "dbo.Competition",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PrimaryKey);
            
            CreateTable(
                "dbo.CompetitionV",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        HeaderKey = c.Guid(nullable: false),
                        CompetitionName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OrganisationGuid = c.Guid(nullable: false),
                        CampaignPeriodValue = c.Int(nullable: false),
                        CampaignPeriodType = c.Int(nullable: false),
                        ResourceGuid = c.Guid(),
                        WebAddress = c.String(unicode: false),
                        EffectiveFrom = c.DateTime(nullable: false, precision: 0),
                        EffectiveTo = c.DateTime(nullable: false, precision: 0),
                        IsMarkedForDeletion = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        OwnerUserId = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false, precision: 0),
                        ModifiedUserId = c.Guid(nullable: false),
                        DateModified = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Competition", t => t.HeaderKey, cascadeDelete: true)
                .ForeignKey("dbo.Organisation", t => t.OrganisationGuid, cascadeDelete: true)
                .Index(t => t.HeaderKey)
                .Index(t => t.OrganisationGuid);
            
            CreateTable(
                "dbo.Organisation",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PrimaryKey);
            
            CreateTable(
                "dbo.CountryV",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        HeaderKey = c.Guid(nullable: false),
                        CountryName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        OrganisationGuid = c.Guid(nullable: false),
                        ResourceGuid = c.Guid(),
                        WebAddress = c.String(unicode: false),
                        EffectiveFrom = c.DateTime(nullable: false, precision: 0),
                        EffectiveTo = c.DateTime(nullable: false, precision: 0),
                        IsMarkedForDeletion = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        OwnerUserId = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false, precision: 0),
                        ModifiedUserId = c.Guid(nullable: false),
                        DateModified = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Country", t => t.HeaderKey, cascadeDelete: true)
                .ForeignKey("dbo.Organisation", t => t.OrganisationGuid, cascadeDelete: true)
                .ForeignKey("dbo.Resource", t => t.ResourceGuid)
                .Index(t => t.HeaderKey)
                .Index(t => t.OrganisationGuid)
                .Index(t => t.ResourceGuid);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PrimaryKey);
            
            CreateTable(
                "dbo.OrganisationV",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        HeaderKey = c.Guid(nullable: false),
                        OrganisationName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        OrganisationDescription = c.String(maxLength: 100, storeType: "nvarchar"),
                        ParentOrganisationGuid = c.Guid(),
                        CountryGuid = c.Guid(),
                        ResourceGuid = c.Guid(),
                        WebAddress = c.String(unicode: false),
                        EffectiveFrom = c.DateTime(nullable: false, precision: 0),
                        EffectiveTo = c.DateTime(nullable: false, precision: 0),
                        IsMarkedForDeletion = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        OwnerUserId = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false, precision: 0),
                        ModifiedUserId = c.Guid(nullable: false),
                        DateModified = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Country", t => t.CountryGuid)
                .ForeignKey("dbo.Organisation", t => t.HeaderKey, cascadeDelete: true)
                .ForeignKey("dbo.Organisation", t => t.ParentOrganisationGuid)
                .Index(t => t.HeaderKey)
                .Index(t => t.ParentOrganisationGuid)
                .Index(t => t.CountryGuid);
            
            CreateTable(
                "dbo.PersonV",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        HeaderKey = c.Guid(nullable: false),
                        Forenames = c.String(maxLength: 50, storeType: "nvarchar"),
                        Surname = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        SearchText = c.String(unicode: false),
                        DateOfBirth = c.DateTime(precision: 0),
                        DateOfDeath = c.DateTime(precision: 0),
                        CountryGuid = c.Guid(),
                        Height = c.Int(),
                        ResourceGuid = c.Guid(),
                        WebAddress = c.String(unicode: false),
                        EffectiveFrom = c.DateTime(nullable: false, precision: 0),
                        EffectiveTo = c.DateTime(nullable: false, precision: 0),
                        IsMarkedForDeletion = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        OwnerUserId = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false, precision: 0),
                        ModifiedUserId = c.Guid(nullable: false),
                        DateModified = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Country", t => t.CountryGuid)
                .ForeignKey("dbo.Person", t => t.HeaderKey, cascadeDelete: true)
                .Index(t => t.HeaderKey)
                .Index(t => t.CountryGuid);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PrimaryKey);
            
            CreateTable(
                "dbo.MatchEvent",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        MatchVPrimaryKey = c.Guid(nullable: false),
                        TeamPrimaryKey = c.Guid(nullable: false),
                        PersonPrimaryKey = c.Guid(nullable: false),
                        PositionType = c.Int(),
                        MatchEventType = c.Int(nullable: false),
                        Minute = c.Short(),
                        Extra = c.Short(),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.MatchV", t => t.MatchVPrimaryKey, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonPrimaryKey, cascadeDelete: true)
                .ForeignKey("dbo.Team", t => t.TeamPrimaryKey, cascadeDelete: true)
                .Index(t => t.MatchVPrimaryKey)
                .Index(t => t.TeamPrimaryKey)
                .Index(t => t.PersonPrimaryKey);
            
            CreateTable(
                "dbo.MatchV",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        HeaderKey = c.Guid(nullable: false),
                        MatchImportType = c.Int(nullable: false),
                        MatchDate = c.DateTime(nullable: false, precision: 0),
                        MatchTimeTicks = c.Long(),
                        CampaignGuid = c.Guid(),
                        VenueGuid = c.Guid(),
                        Attendance = c.Int(),
                        MatchType = c.Int(nullable: false),
                        Team1Guid = c.Guid(nullable: false),
                        Team1HT = c.Short(),
                        Team1FT = c.Short(),
                        Team2Guid = c.Guid(nullable: false),
                        Team2HT = c.Short(),
                        Team2FT = c.Short(),
                        ResourceGuid = c.Guid(),
                        WebAddress = c.String(unicode: false),
                        EffectiveFrom = c.DateTime(nullable: false, precision: 0),
                        EffectiveTo = c.DateTime(nullable: false, precision: 0),
                        IsMarkedForDeletion = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        OwnerUserId = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false, precision: 0),
                        ModifiedUserId = c.Guid(nullable: false),
                        DateModified = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Campaign", t => t.CampaignGuid)
                .ForeignKey("dbo.Match", t => t.HeaderKey, cascadeDelete: true)
                .ForeignKey("dbo.Team", t => t.Team1Guid, cascadeDelete: true)
                .ForeignKey("dbo.Team", t => t.Team2Guid)
                .ForeignKey("dbo.Venue", t => t.VenueGuid)
                .Index(t => t.HeaderKey)
                .Index(t => t.CampaignGuid)
                .Index(t => t.VenueGuid)
                .Index(t => t.Team1Guid)
                .Index(t => t.Team2Guid);
            
            CreateTable(
                "dbo.Match",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PrimaryKey);
            
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PrimaryKey);
            
            CreateTable(
                "dbo.TeamV",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        HeaderKey = c.Guid(nullable: false),
                        TeamName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        HomeVenueGuid = c.Guid(),
                        CountryGuid = c.Guid(),
                        ResourceGuid = c.Guid(),
                        WebAddress = c.String(unicode: false),
                        EffectiveFrom = c.DateTime(nullable: false, precision: 0),
                        EffectiveTo = c.DateTime(nullable: false, precision: 0),
                        IsMarkedForDeletion = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        OwnerUserId = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false, precision: 0),
                        ModifiedUserId = c.Guid(nullable: false),
                        DateModified = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Country", t => t.CountryGuid)
                .ForeignKey("dbo.Venue", t => t.HomeVenueGuid)
                .ForeignKey("dbo.Resource", t => t.ResourceGuid)
                .ForeignKey("dbo.Team", t => t.HeaderKey, cascadeDelete: true)
                .Index(t => t.HeaderKey)
                .Index(t => t.HomeVenueGuid)
                .Index(t => t.CountryGuid)
                .Index(t => t.ResourceGuid);
            
            CreateTable(
                "dbo.Venue",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PrimaryKey);
            
            CreateTable(
                "dbo.VenueV",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        HeaderKey = c.Guid(nullable: false),
                        VenueName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Capacity = c.Int(),
                        Address1 = c.String(maxLength: 100, storeType: "nvarchar"),
                        Address2 = c.String(maxLength: 100, storeType: "nvarchar"),
                        Address3 = c.String(maxLength: 100, storeType: "nvarchar"),
                        Address4 = c.String(maxLength: 100, storeType: "nvarchar"),
                        PostCode = c.String(maxLength: 100, storeType: "nvarchar"),
                        CountryGuid = c.Guid(nullable: false),
                        ResourceGuid = c.Guid(),
                        WebAddress = c.String(unicode: false),
                        EffectiveFrom = c.DateTime(nullable: false, precision: 0),
                        EffectiveTo = c.DateTime(nullable: false, precision: 0),
                        IsMarkedForDeletion = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        OwnerUserId = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false, precision: 0),
                        ModifiedUserId = c.Guid(nullable: false),
                        DateModified = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Country", t => t.CountryGuid, cascadeDelete: true)
                .ForeignKey("dbo.Venue", t => t.HeaderKey, cascadeDelete: true)
                .Index(t => t.HeaderKey)
                .Index(t => t.CountryGuid);
            
            CreateTable(
                "dbo.Resource",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        ResourceBytes = c.Binary(),
                    })
                .PrimaryKey(t => t.PrimaryKey);
            
            CreateTable(
                "dbo.LookupCompetition",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        ImportSite = c.Int(nullable: false),
                        CompetitionGuid = c.Guid(nullable: false),
                        LookupId = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Competition", t => t.CompetitionGuid, cascadeDelete: true)
                .Index(t => t.CompetitionGuid);
            
            CreateTable(
                "dbo.LookupMatch",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        ImportSite = c.Int(nullable: false),
                        MatchGuid = c.Guid(nullable: false),
                        LookupId = c.String(maxLength: 100, storeType: "nvarchar"),
                        HasMatchDetails = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Match", t => t.MatchGuid, cascadeDelete: true)
                .Index(t => t.MatchGuid);
            
            CreateTable(
                "dbo.LookupPerson",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        ImportSite = c.Int(nullable: false),
                        PersonGuid = c.Guid(nullable: false),
                        LookupId = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Person", t => t.PersonGuid, cascadeDelete: true)
                .Index(t => t.PersonGuid);
            
            CreateTable(
                "dbo.LookupTeam",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        ImportSite = c.Int(nullable: false),
                        TeamGuid = c.Guid(nullable: false),
                        LookupId = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Team", t => t.TeamGuid, cascadeDelete: true)
                .Index(t => t.TeamGuid);
            
            CreateTable(
                "dbo.LookupVenue",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        ImportSite = c.Int(nullable: false),
                        VenueGuid = c.Guid(nullable: false),
                        LookupId = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Venue", t => t.VenueGuid, cascadeDelete: true)
                .Index(t => t.VenueGuid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LookupVenue", "VenueGuid", "dbo.Venue");
            DropForeignKey("dbo.LookupTeam", "TeamGuid", "dbo.Team");
            DropForeignKey("dbo.LookupPerson", "PersonGuid", "dbo.Person");
            DropForeignKey("dbo.LookupMatch", "MatchGuid", "dbo.Match");
            DropForeignKey("dbo.LookupCompetition", "CompetitionGuid", "dbo.Competition");
            DropForeignKey("dbo.Campaign", "CompetitionKey", "dbo.Competition");
            DropForeignKey("dbo.CompetitionV", "OrganisationGuid", "dbo.Organisation");
            DropForeignKey("dbo.CountryV", "ResourceGuid", "dbo.Resource");
            DropForeignKey("dbo.CountryV", "OrganisationGuid", "dbo.Organisation");
            DropForeignKey("dbo.CountryV", "HeaderKey", "dbo.Country");
            DropForeignKey("dbo.PersonV", "HeaderKey", "dbo.Person");
            DropForeignKey("dbo.MatchEvent", "TeamPrimaryKey", "dbo.Team");
            DropForeignKey("dbo.MatchEvent", "PersonPrimaryKey", "dbo.Person");
            DropForeignKey("dbo.MatchEvent", "MatchVPrimaryKey", "dbo.MatchV");
            DropForeignKey("dbo.MatchV", "VenueGuid", "dbo.Venue");
            DropForeignKey("dbo.MatchV", "Team2Guid", "dbo.Team");
            DropForeignKey("dbo.MatchV", "Team1Guid", "dbo.Team");
            DropForeignKey("dbo.TeamV", "HeaderKey", "dbo.Team");
            DropForeignKey("dbo.TeamV", "ResourceGuid", "dbo.Resource");
            DropForeignKey("dbo.TeamV", "HomeVenueGuid", "dbo.Venue");
            DropForeignKey("dbo.VenueV", "HeaderKey", "dbo.Venue");
            DropForeignKey("dbo.VenueV", "CountryGuid", "dbo.Country");
            DropForeignKey("dbo.TeamV", "CountryGuid", "dbo.Country");
            DropForeignKey("dbo.MatchV", "HeaderKey", "dbo.Match");
            DropForeignKey("dbo.MatchV", "CampaignGuid", "dbo.Campaign");
            DropForeignKey("dbo.PersonV", "CountryGuid", "dbo.Country");
            DropForeignKey("dbo.OrganisationV", "ParentOrganisationGuid", "dbo.Organisation");
            DropForeignKey("dbo.OrganisationV", "HeaderKey", "dbo.Organisation");
            DropForeignKey("dbo.OrganisationV", "CountryGuid", "dbo.Country");
            DropForeignKey("dbo.CompetitionV", "HeaderKey", "dbo.Competition");
            DropIndex("dbo.LookupVenue", new[] { "VenueGuid" });
            DropIndex("dbo.LookupTeam", new[] { "TeamGuid" });
            DropIndex("dbo.LookupPerson", new[] { "PersonGuid" });
            DropIndex("dbo.LookupMatch", new[] { "MatchGuid" });
            DropIndex("dbo.LookupCompetition", new[] { "CompetitionGuid" });
            DropIndex("dbo.VenueV", new[] { "CountryGuid" });
            DropIndex("dbo.VenueV", new[] { "HeaderKey" });
            DropIndex("dbo.TeamV", new[] { "ResourceGuid" });
            DropIndex("dbo.TeamV", new[] { "CountryGuid" });
            DropIndex("dbo.TeamV", new[] { "HomeVenueGuid" });
            DropIndex("dbo.TeamV", new[] { "HeaderKey" });
            DropIndex("dbo.MatchV", new[] { "Team2Guid" });
            DropIndex("dbo.MatchV", new[] { "Team1Guid" });
            DropIndex("dbo.MatchV", new[] { "VenueGuid" });
            DropIndex("dbo.MatchV", new[] { "CampaignGuid" });
            DropIndex("dbo.MatchV", new[] { "HeaderKey" });
            DropIndex("dbo.MatchEvent", new[] { "PersonPrimaryKey" });
            DropIndex("dbo.MatchEvent", new[] { "TeamPrimaryKey" });
            DropIndex("dbo.MatchEvent", new[] { "MatchVPrimaryKey" });
            DropIndex("dbo.PersonV", new[] { "CountryGuid" });
            DropIndex("dbo.PersonV", new[] { "HeaderKey" });
            DropIndex("dbo.OrganisationV", new[] { "CountryGuid" });
            DropIndex("dbo.OrganisationV", new[] { "ParentOrganisationGuid" });
            DropIndex("dbo.OrganisationV", new[] { "HeaderKey" });
            DropIndex("dbo.CountryV", new[] { "ResourceGuid" });
            DropIndex("dbo.CountryV", new[] { "OrganisationGuid" });
            DropIndex("dbo.CountryV", new[] { "HeaderKey" });
            DropIndex("dbo.CompetitionV", new[] { "OrganisationGuid" });
            DropIndex("dbo.CompetitionV", new[] { "HeaderKey" });
            DropIndex("dbo.Campaign", new[] { "CompetitionKey" });
            DropTable("dbo.LookupVenue");
            DropTable("dbo.LookupTeam");
            DropTable("dbo.LookupPerson");
            DropTable("dbo.LookupMatch");
            DropTable("dbo.LookupCompetition");
            DropTable("dbo.Resource");
            DropTable("dbo.VenueV");
            DropTable("dbo.Venue");
            DropTable("dbo.TeamV");
            DropTable("dbo.Team");
            DropTable("dbo.Match");
            DropTable("dbo.MatchV");
            DropTable("dbo.MatchEvent");
            DropTable("dbo.Person");
            DropTable("dbo.PersonV");
            DropTable("dbo.OrganisationV");
            DropTable("dbo.Country");
            DropTable("dbo.CountryV");
            DropTable("dbo.Organisation");
            DropTable("dbo.CompetitionV");
            DropTable("dbo.Competition");
            DropTable("dbo.Campaign");
        }
    }
}
