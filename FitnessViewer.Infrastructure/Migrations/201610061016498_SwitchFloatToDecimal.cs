namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SwitchFloatToDecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Activities", "Distance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Activities", "DistanceInMiles", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Activities", "ElevationGain", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Activities", "AverageHeartrate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Activities", "MaxHeartrate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Activities", "AverageSpeed", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Activities", "MaxSpeed", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Activities", "AverageCadence", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Activities", "AverageTemperature", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Activities", "AveragePower", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Activities", "Kilojoules", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Activities", "Calories", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Athletes", "Weight", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.BestEfforts", "Distance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Gears", "Distance", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Laps", "Distance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Laps", "TotalElevationGain", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Laps", "AverageSpeed", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Laps", "MaxSpeed", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Laps", "AverageCadence", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Laps", "AveragePower", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Laps", "AverageHeartrate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Laps", "MaxHeartrate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Laps", "MaxHeartrate", c => c.Single(nullable: false));
            AlterColumn("dbo.Laps", "AverageHeartrate", c => c.Single(nullable: false));
            AlterColumn("dbo.Laps", "AveragePower", c => c.Single(nullable: false));
            AlterColumn("dbo.Laps", "AverageCadence", c => c.Single(nullable: false));
            AlterColumn("dbo.Laps", "MaxSpeed", c => c.Single(nullable: false));
            AlterColumn("dbo.Laps", "AverageSpeed", c => c.Single(nullable: false));
            AlterColumn("dbo.Laps", "TotalElevationGain", c => c.Single(nullable: false));
            AlterColumn("dbo.Laps", "Distance", c => c.Single(nullable: false));
            AlterColumn("dbo.Gears", "Distance", c => c.Single());
            AlterColumn("dbo.BestEfforts", "Distance", c => c.Single(nullable: false));
            AlterColumn("dbo.Athletes", "Weight", c => c.Single());
            AlterColumn("dbo.Activities", "Calories", c => c.Single(nullable: false));
            AlterColumn("dbo.Activities", "Kilojoules", c => c.Single(nullable: false));
            AlterColumn("dbo.Activities", "AveragePower", c => c.Single(nullable: false));
            AlterColumn("dbo.Activities", "AverageTemperature", c => c.Single(nullable: false));
            AlterColumn("dbo.Activities", "AverageCadence", c => c.Single(nullable: false));
            AlterColumn("dbo.Activities", "MaxSpeed", c => c.Single(nullable: false));
            AlterColumn("dbo.Activities", "AverageSpeed", c => c.Single(nullable: false));
            AlterColumn("dbo.Activities", "MaxHeartrate", c => c.Single(nullable: false));
            AlterColumn("dbo.Activities", "AverageHeartrate", c => c.Single(nullable: false));
            AlterColumn("dbo.Activities", "ElevationGain", c => c.Single(nullable: false));
            AlterColumn("dbo.Activities", "DistanceInMiles", c => c.Single(nullable: false));
            AlterColumn("dbo.Activities", "Distance", c => c.Single(nullable: false));
        }
    }
}
