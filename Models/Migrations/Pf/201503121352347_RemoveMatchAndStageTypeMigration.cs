namespace FootballOracle.Models.Migrations.Pf
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveMatchAndStageTypeMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("MatchV", "CampaignGuid", "Campaign");
            DropForeignKey("MatchV", "CampaignStageKey", "CampaignStage");
            DropIndex("MatchV", new[] { "CampaignGuid" });
            DropIndex("MatchV", new[] { "CampaignStageKey" });
            AlterColumn("MatchV", "CampaignStageKey", c => c.Guid(nullable: false));
            CreateIndex("MatchV", "CampaignStageKey");
            AddForeignKey("MatchV", "CampaignStageKey", "CampaignStage", "PrimaryKey", cascadeDelete: true);
            DropColumn("MatchV", "CampaignGuid");
            DropColumn("MatchV", "MatchType");
            DropColumn("MatchV", "StageType");
        }
        
        public override void Down()
        {
            AddColumn("MatchV", "StageType", c => c.Int());
            AddColumn("MatchV", "MatchType", c => c.Int(nullable: false));
            AddColumn("MatchV", "CampaignGuid", c => c.Guid());
            DropForeignKey("MatchV", "CampaignStageKey", "CampaignStage");
            DropIndex("MatchV", new[] { "CampaignStageKey" });
            AlterColumn("MatchV", "CampaignStageKey", c => c.Guid());
            CreateIndex("MatchV", "CampaignStageKey");
            CreateIndex("MatchV", "CampaignGuid");
            AddForeignKey("MatchV", "CampaignStageKey", "CampaignStage", "PrimaryKey");
            AddForeignKey("MatchV", "CampaignGuid", "Campaign", "PrimaryKey");
        }
    }
}
