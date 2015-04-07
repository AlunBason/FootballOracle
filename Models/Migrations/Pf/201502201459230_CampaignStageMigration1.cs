namespace FootballOracle.Models.Migrations.Pf
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampaignStageMigration1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignStage", "IsDefault", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignStage", "IsDefault");
        }
    }
}
