namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecreateAthleteSettingsWithoutIdentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AthleteSettings",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        DashboardStart = c.DateTime(nullable: false),
                        DashboardEnd = c.DateTime(nullable: false),
                        DashboardRange = c.String(maxLength: 32),
                        ShowRun = c.Boolean(nullable: false),
                        ShowRide = c.Boolean(nullable: false),
                        ShowSwim = c.Boolean(nullable: false),
                        ShowOther = c.Boolean(nullable: false),
                        ShowAll = c.Boolean(nullable: false),
                        RunDistanceUnit = c.Int(nullable: false),
                        RideDistanceUnit = c.Int(nullable: false),
                        SwimDistanceUnit = c.Int(nullable: false),
                        OtherDistanceUnit = c.Int(nullable: false),
                        AllDistanceUnit = c.Int(nullable: false),
                        RunPaceUnit = c.Int(nullable: false),
                        RidePaceUnit = c.Int(nullable: false),
                        SwimPaceUnit = c.Int(nullable: false),
                        OtherPaceUnit = c.Int(nullable: false),
                        AllPaceUnit = c.Int(nullable: false),
                        RunElevationUnit = c.Int(nullable: false),
                        RideElevationUnit = c.Int(nullable: false),
                        SwimElevationUnit = c.Int(nullable: false),
                        OtherElevationUnit = c.Int(nullable: false),
                        AllElevationUnit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Athletes", t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.UserId, unique: true, name: "IX_AthleteSetting_UserId");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AthleteSettings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AthleteSettings", "Id", "dbo.Athletes");
            DropIndex("dbo.AthleteSettings", "IX_AthleteSetting_UserId");
            DropIndex("dbo.AthleteSettings", new[] { "Id" });
            DropTable("dbo.AthleteSettings");
        }
    }
}
