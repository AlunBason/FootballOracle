namespace FootballOracle.Models.Migrations.Pf
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampaignStageMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignStage",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        CampaignKey = c.Guid(nullable: false),
                        StageOrder = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        IsLeague = c.Boolean(nullable: false),
                        LegCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Campaign", t => t.CampaignKey, cascadeDelete: true)
                .Index(t => t.CampaignKey);
            
            CreateTable(
                "dbo.LookupCampaignStage",
                c => new
                    {
                        PrimaryKey = c.Guid(nullable: false),
                        ImportSite = c.Int(nullable: false),
                        CampaignStageKey = c.Guid(nullable: false),
                        LookupId = c.String(maxLength: 100, storeType: "nvarchar"),
                        CampaignKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PrimaryKey)
                .ForeignKey("dbo.Campaign", t => t.CampaignKey, cascadeDelete: true)
                .ForeignKey("dbo.CampaignStage", t => t.CampaignStageKey, cascadeDelete: true)
                .Index(t => t.CampaignStageKey)
                .Index(t => t.CampaignKey);
            
            AddColumn("dbo.MatchV", "CampaignStageKey", c => c.Guid());
            CreateIndex("dbo.MatchV", "CampaignStageKey");
            AddForeignKey("dbo.MatchV", "CampaignStageKey", "dbo.CampaignStage", "PrimaryKey");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LookupCampaignStage", "CampaignStageKey", "dbo.CampaignStage");
            DropForeignKey("dbo.LookupCampaignStage", "CampaignKey", "dbo.Campaign");
            DropForeignKey("dbo.MatchV", "CampaignStageKey", "dbo.CampaignStage");
            DropForeignKey("dbo.CampaignStage", "CampaignKey", "dbo.Campaign");
            DropIndex("dbo.LookupCampaignStage", new[] { "CampaignKey" });
            DropIndex("dbo.LookupCampaignStage", new[] { "CampaignStageKey" });
            DropIndex("dbo.MatchV", new[] { "CampaignStageKey" });
            DropIndex("dbo.CampaignStage", new[] { "CampaignKey" });
            DropColumn("dbo.MatchV", "CampaignStageKey");
            DropTable("dbo.LookupCampaignStage");
            DropTable("dbo.CampaignStage");
        }
    }
}
