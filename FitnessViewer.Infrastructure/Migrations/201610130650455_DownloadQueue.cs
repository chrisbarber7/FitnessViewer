namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DownloadQueue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DownloadQueues", "DownloadType", c => c.Int(nullable: false));
            DropColumn("dbo.DownloadQueues", "RemoteSystem");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DownloadQueues", "RemoteSystem", c => c.Int(nullable: false));
            DropColumn("dbo.DownloadQueues", "DownloadType");
        }
    }
}
