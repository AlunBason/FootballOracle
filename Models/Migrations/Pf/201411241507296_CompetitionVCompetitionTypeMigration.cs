namespace FootballOracle.Models.Migrations.Pf
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompetitionVCompetitionTypeMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompetitionV", "CompetitionType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompetitionV", "CompetitionType");
        }
    }
}
