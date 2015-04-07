namespace FootballOracle.Models.Migrations.Pf
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MatchVPredictionMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MatchV", "AllFormPtsPc", c => c.Double());
            AddColumn("dbo.MatchV", "AllFormGoalDiff", c => c.Double());
            AddColumn("dbo.MatchV", "HomeVAwayFormPtsPc", c => c.Double());
            AddColumn("dbo.MatchV", "HomeVAwayFormGoalDiff", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MatchV", "HomeVAwayFormGoalDiff");
            DropColumn("dbo.MatchV", "HomeVAwayFormPtsPc");
            DropColumn("dbo.MatchV", "AllFormGoalDiff");
            DropColumn("dbo.MatchV", "AllFormPtsPc");
        }
    }
}
