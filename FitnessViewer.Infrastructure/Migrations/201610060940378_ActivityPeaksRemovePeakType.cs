namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivityPeaksRemovePeakType : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ActivityPeaks", "PeakType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActivityPeaks", "PeakType", c => c.Byte(nullable: false));
        }
    }
}
