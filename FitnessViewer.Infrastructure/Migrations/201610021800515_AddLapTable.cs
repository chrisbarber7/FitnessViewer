namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLapTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Laps",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ActivityId = c.Long(nullable: false),
                        AthleteId = c.Long(nullable: false),
                        ResourceState = c.Int(nullable: false),
                        Name = c.String(),
                        MovingTime = c.Time(nullable: false, precision: 7),
                        ElapsedTime = c.Time(nullable: false, precision: 7),
                        Start = c.DateTime(nullable: false),
                        StartLocal = c.DateTime(nullable: false),
                        Distance = c.Single(nullable: false),
                        StartIndex = c.Int(nullable: false),
                        EndIndex = c.Int(nullable: false),
                        TotalElevationGain = c.Single(nullable: false),
                        AverageSpeed = c.Single(nullable: false),
                        MaxSpeed = c.Single(nullable: false),
                        AverageCadence = c.Single(nullable: false),
                        AveragePower = c.Single(nullable: false),
                        AverageHeartrate = c.Single(nullable: false),
                        MaxHeartrate = c.Single(nullable: false),
                        LapIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId, cascadeDelete: true)
                .ForeignKey("dbo.Athletes", t => t.AthleteId)
                .Index(t => t.ActivityId)
                .Index(t => t.AthleteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Laps", "AthleteId", "dbo.Athletes");
            DropForeignKey("dbo.Laps", "ActivityId", "dbo.Activities");
            DropIndex("dbo.Laps", new[] { "AthleteId" });
            DropIndex("dbo.Laps", new[] { "ActivityId" });
            DropTable("dbo.Laps");
        }
    }
}
