namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtheleteSettingTableAmend : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AthleteSettings", "DashboardRange", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AthleteSettings", "DashboardRange");
        }
    }
}
