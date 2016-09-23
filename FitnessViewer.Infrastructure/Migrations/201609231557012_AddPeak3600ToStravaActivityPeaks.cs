namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPeak3600ToStravaActivityPeaks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StravaActivityPeaks", "Peak3600", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StravaActivityPeaks", "Peak3600");
        }
    }
}
