namespace FootballOracle.Models.Migrations.Pf
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompetitionRank : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompetitionV", "Rank", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompetitionV", "Rank");
        }
    }
}
