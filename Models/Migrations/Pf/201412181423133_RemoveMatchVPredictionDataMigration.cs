namespace FootballOracle.Models.Migrations.Pf
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveMatchVPredictionDataMigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MatchV", "AllFormPtsPc");
            DropColumn("dbo.MatchV", "AllFormGoalDiff");
            DropColumn("dbo.MatchV", "HomeVAwayFormPtsPc");
            DropColumn("dbo.MatchV", "HomeVAwayFormGoalDiff");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MatchV", "HomeVAwayFormGoalDiff", c => c.Double());
            AddColumn("dbo.MatchV", "HomeVAwayFormPtsPc", c => c.Double());
            AddColumn("dbo.MatchV", "AllFormGoalDiff", c => c.Double());
            AddColumn("dbo.MatchV", "AllFormPtsPc", c => c.Double());
        }
    }
}
