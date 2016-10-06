namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivityPeaksAddStreamTypeColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActivityPeaks", "StreamType", c => c.Int(nullable: false));
            Sql("update dbo.activityPeaks set streamtype = peaktype;");
        }
        
        public override void Down()
        {
            DropColumn("dbo.ActivityPeaks", "StreamType");
        }
    }
}
