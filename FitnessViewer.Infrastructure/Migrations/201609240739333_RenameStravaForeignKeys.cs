namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameStravaForeignKeys : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Activities", name: "StravaAthleteId", newName: "AthleteId");
            RenameColumn(table: "dbo.ActivityPeaks", name: "StravaActivityId", newName: "ActivityId");
            RenameColumn(table: "dbo.BestEfforts", name: "StravaActivityId", newName: "ActivityId");
            RenameColumn(table: "dbo.Streams", name: "StravaActivityId", newName: "ActivityId");
            RenameIndex(table: "dbo.Activities", name: "IX_StravaAthleteId", newName: "IX_AthleteId");
            RenameIndex(table: "dbo.ActivityPeaks", name: "IX_StravaActivityId", newName: "IX_ActivityId");
            RenameIndex(table: "dbo.BestEfforts", name: "IX_StravaActivityId", newName: "IX_ActivityId");
            RenameIndex(table: "dbo.Streams", name: "IX_StravaActivityId", newName: "IX_ActivityId");
            AddColumn("dbo.DownloadQueues", "ActivityId", c => c.Long());
            DropColumn("dbo.DownloadQueues", "StravaActivityId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DownloadQueues", "StravaActivityId", c => c.Long());
            DropColumn("dbo.DownloadQueues", "ActivityId");
            RenameIndex(table: "dbo.Streams", name: "IX_ActivityId", newName: "IX_StravaActivityId");
            RenameIndex(table: "dbo.BestEfforts", name: "IX_ActivityId", newName: "IX_StravaActivityId");
            RenameIndex(table: "dbo.ActivityPeaks", name: "IX_ActivityId", newName: "IX_StravaActivityId");
            RenameIndex(table: "dbo.Activities", name: "IX_AthleteId", newName: "IX_StravaAthleteId");
            RenameColumn(table: "dbo.Streams", name: "ActivityId", newName: "StravaActivityId");
            RenameColumn(table: "dbo.BestEfforts", name: "ActivityId", newName: "StravaActivityId");
            RenameColumn(table: "dbo.ActivityPeaks", name: "ActivityId", newName: "StravaActivityId");
            RenameColumn(table: "dbo.Activities", name: "AthleteId", newName: "StravaAthleteId");
        }
    }
}
