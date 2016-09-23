namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStravaActivityPeaks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StravaActivityPeaks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StravaActivityId = c.Long(nullable: false),
                        PeakType = c.Byte(nullable: false),
                        Peak5 = c.Int(),
                        Peak10 = c.Int(),
                        Peak30 = c.Int(),
                        Peak60 = c.Int(),
                        Peak120 = c.Int(),
                        Peak300 = c.Int(),
                        Peak360 = c.Int(),
                        Peak600 = c.Int(),
                        Peak720 = c.Int(),
                        Peak1200 = c.Int(),
                        Peak1800 = c.Int(),
                        PeakDuration = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StravaActivities", t => t.StravaActivityId, cascadeDelete: true)
                .Index(t => t.StravaActivityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StravaActivityPeaks", "StravaActivityId", "dbo.StravaActivities");
            DropIndex("dbo.StravaActivityPeaks", new[] { "StravaActivityId" });
            DropTable("dbo.StravaActivityPeaks");
        }
    }
}
