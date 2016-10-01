namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActivityMap : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "MapId", c => c.String());
            AddColumn("dbo.Activities", "MapPolyline", c => c.String());
            AddColumn("dbo.Activities", "MapPolylineSummary", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "MapPolylineSummary");
            DropColumn("dbo.Activities", "MapPolyline");
            DropColumn("dbo.Activities", "MapId");
        }
    }
}
