namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DownloadQueueAddDownloadType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DownloadQueues", "RemoteSystem", c => c.Int(nullable: false));
            Sql("update dbo.DownloadQueues set RemoteSystem=1;");
        }
        
        public override void Down()
        {
            DropColumn("dbo.DownloadQueues", "RemoteSystem");
        }
    }
}
