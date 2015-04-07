namespace FootballOracle.Models.Migrations.Pf
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamVTeamNameNotRequiredMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TeamV", "TeamName", c => c.String(maxLength: 100, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TeamV", "TeamName", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
        }
    }
}
