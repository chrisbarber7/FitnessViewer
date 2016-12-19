namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AthleteAmend2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AthleteSettings", "DashboardRange", c => c.String(maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AthleteSettings", "DashboardRange", c => c.String());
        }
    }
}
