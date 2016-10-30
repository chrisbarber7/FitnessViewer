namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DownloadQueueAddDurationField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DownloadQueues", "Duration", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DownloadQueues", "Duration");
        }
    }
}
