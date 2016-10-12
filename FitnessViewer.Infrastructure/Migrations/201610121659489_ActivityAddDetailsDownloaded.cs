namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivityAddDetailsDownloaded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "DetailsDownloaded", c => c.Boolean(nullable: false));

            Sql("Update dbo.Activities set DetailsDownloaded=1 where DetailsDownloaded is null");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "DetailsDownloaded");
        }
    }
}
