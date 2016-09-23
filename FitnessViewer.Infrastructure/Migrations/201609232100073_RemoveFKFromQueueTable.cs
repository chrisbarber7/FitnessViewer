namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFKFromQueueTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StravaQueues", "StravaActivityId", "dbo.StravaActivities");
            DropIndex("dbo.StravaQueues", new[] { "StravaActivityId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.StravaQueues", "StravaActivityId");
            AddForeignKey("dbo.StravaQueues", "StravaActivityId", "dbo.StravaActivities", "Id");
        }
    }
}
