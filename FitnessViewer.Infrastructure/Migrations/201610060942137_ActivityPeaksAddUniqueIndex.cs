namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivityPeaksAddUniqueIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ActivityPeaks", new[] { "ActivityId" });
            CreateIndex("dbo.ActivityPeaks", new[] { "ActivityId", "StreamType" }, unique: true, name: "IX_ActivityPeaks_ActivityIdAndStreamType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ActivityPeaks", "IX_ActivityPeaks_ActivityIdAndStreamType");
            CreateIndex("dbo.ActivityPeaks", "ActivityId");
        }
    }
}
