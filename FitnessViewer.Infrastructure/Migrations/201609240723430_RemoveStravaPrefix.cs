namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveStravaPrefix : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.StravaQueues", newName: "DownloadQueues");
            RenameTable(name: "dbo.StravaActivities", newName: "Activities");
            RenameTable(name: "dbo.StravaAthletes", newName: "Athletes");
            RenameTable(name: "dbo.StravaActivityPeaks", newName: "ActivityPeaks");
            RenameTable(name: "dbo.StravaBestEfforts", newName: "BestEfforts");
            RenameTable(name: "dbo.StravaStreams", newName: "Streams");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Streams", newName: "StravaStreams");
            RenameTable(name: "dbo.BestEfforts", newName: "StravaBestEfforts");
            RenameTable(name: "dbo.ActivityPeaks", newName: "StravaActivityPeaks");
            RenameTable(name: "dbo.Athletes", newName: "StravaAthletes");
            RenameTable(name: "dbo.Activities", newName: "StravaActivities");
            RenameTable(name: "dbo.DownloadQueues", newName: "StravaQueues");
        }
    }
}
