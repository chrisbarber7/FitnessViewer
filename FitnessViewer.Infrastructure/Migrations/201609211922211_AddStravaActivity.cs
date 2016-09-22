namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStravaActivity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StravaActivities",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        StravaAthleteId = c.Long(nullable: false),
                        Name = c.String(),
                        ExternalId = c.String(),
                        Type = c.String(),
                        SufferScore = c.Int(),
                        EmbedToken = c.String(),
                        Distance = c.Single(nullable: false),
                        TotalPhotoCount = c.Int(nullable: false),
                        ElevationGain = c.Single(nullable: false),
                        HasKudoed = c.Boolean(nullable: false),
                        AverageHeartrate = c.Single(nullable: false),
                        MaxHeartrate = c.Single(nullable: false),
                        Truncated = c.Int(),
                        GearId = c.String(),
                        AverageSpeed = c.Single(nullable: false),
                        MaxSpeed = c.Single(nullable: false),
                        AverageCadence = c.Single(nullable: false),
                        AverageTemperature = c.Single(nullable: false),
                        AveragePower = c.Single(nullable: false),
                        Kilojoules = c.Single(nullable: false),
                        IsTrainer = c.Boolean(nullable: false),
                        IsCommute = c.Boolean(nullable: false),
                        IsManual = c.Boolean(nullable: false),
                        IsPrivate = c.Boolean(nullable: false),
                        IsFlagged = c.Boolean(nullable: false),
                        AchievementCount = c.Int(nullable: false),
                        KudosCount = c.Int(nullable: false),
                        CommentCount = c.Int(nullable: false),
                        AthleteCount = c.Int(nullable: false),
                        PhotoCount = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        StartDateLocal = c.DateTime(nullable: false),
                        MovingTime = c.Time(precision: 7),
                        ElapsedTime = c.Time(precision: 7),
                        TimeZone = c.String(),
                        StartLatitude = c.Double(),
                        StartLongitude = c.Double(),
                        WeightedAverageWatts = c.Int(nullable: false),
                        EndLatitude = c.Double(),
                        EndLongitude = c.Double(),
                        HasPowerMeter = c.Boolean(nullable: false),
                        Calories = c.Single(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StravaAthletes", t => t.StravaAthleteId, cascadeDelete: true)
                .Index(t => t.StravaAthleteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StravaActivities", "StravaAthleteId", "dbo.StravaAthletes");
            DropIndex("dbo.StravaActivities", new[] { "StravaAthleteId" });
            DropTable("dbo.StravaActivities");
        }
    }
}
