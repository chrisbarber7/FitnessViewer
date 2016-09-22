namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStravaBestEffort : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StravaBestEfforts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StravaActivityId = c.Long(nullable: false),
                        ResourceState = c.Int(nullable: false),
                        Name = c.String(),
                        ElapsedTime = c.Time(nullable: false, precision: 7),
                        MovingTime = c.Time(nullable: false, precision: 7),
                        StartDate = c.DateTime(nullable: false),
                        StartDateLocal = c.DateTime(nullable: false),
                        Distance = c.Single(nullable: false),
                        StartIndex = c.Int(nullable: false),
                        EndIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StravaActivities", t => t.StravaActivityId, cascadeDelete: true)
                .Index(t => t.StravaActivityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StravaBestEfforts", "StravaActivityId", "dbo.StravaActivities");
            DropIndex("dbo.StravaBestEfforts", new[] { "StravaActivityId" });
            DropTable("dbo.StravaBestEfforts");
        }
    }
}
