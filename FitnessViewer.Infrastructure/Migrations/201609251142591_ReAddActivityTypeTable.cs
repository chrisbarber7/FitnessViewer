namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReAddActivityTypeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        IsRide = c.Boolean(nullable: false),
                        IsRun = c.Boolean(nullable: false),
                        IsSwim = c.Boolean(nullable: false),
                        IsOther = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Activities", "ActivityTypeId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Activities", "ActivityTypeId");
            AddForeignKey("dbo.Activities", "ActivityTypeId", "dbo.ActivityTypes", "Id");
            DropColumn("dbo.Activities", "Type");

          

       Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Ride','Ride',1,0,0,0);");
       Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Run',' Run',0,1,0,0);");
       Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Swim', 'Swim',0,0,1,0);");
        Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Hike','Hike',0,0,0,1);");
        Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Walk','Walk',0,0,0,1);");
        Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('AlpineSki', 'AlpineSki',0,0,0,1);");
        Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('BackcountrySki', 'BackcountrySki',0,0,0,1);");
        Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Canoeing', 'Canoeing',0,0,0,1);");
        Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('CrossCountrySkiing',       'CrossCountrySkiing',0,0,0,1);");
        Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Crossfit','Crossfit',0,0,0,1);");
        Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('EBikeRide','EBikeRide',0,0,0,1);");
        Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Elliptical',   'Elliptical',0,0,0,1);");
        Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Iceskate', 'Iceskate',0,0,0,1);");; 
        Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('InlineSkate','InlineSkate',0,0,0,1);");
       Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Kayaking','Kayaking',0,0,0,1);");
        Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Kitesurf','Kitesurf',0,0,0,1);");
       Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('NordicSki','NordicSki',0,0,0,1);");
        Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('RockClimbing', 'RockClimbing',0,0,0,1);");
        Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('RollerSki','RollerSki',0,0,0,1);");
       Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Rowing', 'Rowing',0,0,0,1);");
       Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Snowboard', 'Snowboard',0,0,0,1);");
      Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Snowshoe','Snowshoe',0,0,0,1);");
       Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('StairStepper','StairStepper',0,0,0,1);");
       Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('StandUpPaddling','StandUpPaddling',0,0,0,1);");
       Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Surfing','Surfing',0,0,0,1);");
      Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('VirtualRide','VirtualRide',1,0,0,0);");
       Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('WeightTraining','WeightTraining',0,0,0,1);");
       Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Windsurf','Windsurf',0,0,0,1);");
       Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Workout','Workout',0,0,0,1);");
       Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Yoga','Yoga',0,0,0,1);");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Activities", "Type", c => c.String());
            DropForeignKey("dbo.Activities", "ActivityTypeId", "dbo.ActivityTypes");
            DropIndex("dbo.Activities", new[] { "ActivityTypeId" });
            DropColumn("dbo.Activities", "ActivityTypeId");
            DropTable("dbo.ActivityTypes");
        }
    }
}
