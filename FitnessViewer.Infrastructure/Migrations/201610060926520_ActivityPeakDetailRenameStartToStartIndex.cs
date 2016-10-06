namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivityPeakDetailRenameStartToStartIndex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActivityPeakDetails", "StartIndex", c => c.Int());
            DropColumn("dbo.ActivityPeakDetails", "Start");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActivityPeakDetails", "Start", c => c.Int());
            DropColumn("dbo.ActivityPeakDetails", "StartIndex");
        }
    }
}
