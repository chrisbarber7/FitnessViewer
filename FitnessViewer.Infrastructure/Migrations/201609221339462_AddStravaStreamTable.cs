namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStravaStreamTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StravaStreams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StravaActivityId = c.Long(nullable: false),
                        Time = c.Int(nullable: false),
                        Latitude = c.Double(),
                        Longitude = c.Double(),
                        Distance = c.Double(),
                        Velocity = c.Double(),
                        HeartRate = c.Int(),
                        Cadence = c.Int(),
                        Watts = c.Int(),
                        Temperature = c.Int(),
                        Moving = c.Boolean(),
                        Gradient = c.Double(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StravaActivities", t => t.StravaActivityId, cascadeDelete: true)
                .Index(t => t.StravaActivityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StravaStreams", "StravaActivityId", "dbo.StravaActivities");
            DropIndex("dbo.StravaStreams", new[] { "StravaActivityId" });
            DropTable("dbo.StravaStreams");
        }
    }
}
