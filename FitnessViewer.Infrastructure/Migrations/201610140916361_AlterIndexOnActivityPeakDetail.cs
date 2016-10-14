namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterIndexOnActivityPeakDetail : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ActivityPeakDetails", "IX_ActivityIdAndDuration");
            CreateIndex("dbo.ActivityPeakDetails", "ActivityId");
            CreateIndex("dbo.ActivityPeakDetails", new[] { "Duration", "StreamType" }, name: "IX_ActivityPeakDetail_DurationAndStreamType");
            CreateIndex("dbo.ActivityPeakDetails", "Value", name: "IX_ActivityPeakDetail_Value");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ActivityPeakDetails", "IX_ActivityPeakDetail_Value");
            DropIndex("dbo.ActivityPeakDetails", "IX_ActivityPeakDetail_DurationAndStreamType");
            DropIndex("dbo.ActivityPeakDetails", new[] { "ActivityId" });
            CreateIndex("dbo.ActivityPeakDetails", new[] { "ActivityId", "StreamType", "Duration" }, unique: true, name: "IX_ActivityIdAndDuration");
        }
    }
}
