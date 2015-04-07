namespace FootballOracle.Models.Migrations.Pf
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MatchVStageTypeMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MatchV", "StageType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MatchV", "StageType");
        }
    }
}
