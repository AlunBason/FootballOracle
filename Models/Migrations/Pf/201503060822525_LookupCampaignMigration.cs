namespace FootballOracle.Models.Migrations.Pf
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LookupCampaignMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("LookupCampaignStage", "CampaignKey", "Campaign");
            DropIndex("LookupCampaignStage", new[] { "CampaignKey" });
            DropColumn("LookupCampaignStage", "CampaignKey");
        }
        
        public override void Down()
        {
            AddColumn("LookupCampaignStage", "CampaignKey", c => c.Guid(nullable: false));
            CreateIndex("LookupCampaignStage", "CampaignKey");
            AddForeignKey("LookupCampaignStage", "CampaignKey", "Campaign", "PrimaryKey", cascadeDelete: true);
        }
    }
}
