namespace FootballOracle.Models.Migrations.Pf
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamNameMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamName",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        TeamVKey = c.Guid(nullable: false),
                        TeamNameType = c.Int(nullable: false),
                        LanguageType = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.TeamV", t => t.TeamVKey, cascadeDelete: true)
                .Index(t => t.TeamVKey);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamName", "TeamVKey", "dbo.TeamV");
            DropIndex("dbo.TeamName", new[] { "TeamVKey" });
            DropTable("dbo.TeamName");
        }
    }
}
