namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivityPeakDetailaddUniqueIndexAndEndIndexColumn : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ActivityPeakDetails", new[] { "ActivityId" });
            AddColumn("dbo.ActivityPeakDetails", "EndIndex", c => c.Int());
            CreateIndex("dbo.ActivityPeakDetails", new[] { "ActivityId", "StreamType", "Duration" }, unique: true, name: "IX_ActivityIdAndDuration");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ActivityPeakDetails", "IX_ActivityIdAndDuration");
            DropColumn("dbo.ActivityPeakDetails", "EndIndex");
            CreateIndex("dbo.ActivityPeakDetails", "ActivityId");
        }
    }
}
