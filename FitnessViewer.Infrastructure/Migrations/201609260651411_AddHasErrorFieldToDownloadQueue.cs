namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHasErrorFieldToDownloadQueue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DownloadQueues", "HasError", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DownloadQueues", "HasError");
        }
    }
}
