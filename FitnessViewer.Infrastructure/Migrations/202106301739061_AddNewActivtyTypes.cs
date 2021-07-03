namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewActivtyTypes : DbMigration
    {
        public override void Up()
        {                
            Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Golf','Golf',0,0,0,1);");
            Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Handcycle','Handcycle',0,0,0,1);");
            Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Sail','Sail',0,0,0,1);");
            Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Skateboard','Skateboard',0,0,0,1);");
            Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Soccer','Soccer',0,0,0,1);");
            Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Velomobile','Velomobile',0,0,0,1);");
            Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('Wheelchair','Wheelchair',0,0,0,1);");
        }
        
        public override void Down()
        {            
            Sql("delete from dbo.ActivityTypes where Id='Golf';");
            Sql("delete from dbo.ActivityTypes where Id='Handcycle';");
            Sql("delete from dbo.ActivityTypes where Id='Sail';");            
            Sql("delete from dbo.ActivityTypes where Id='Skateboard';");
            Sql("delete from dbo.ActivityTypes where Id='Soccer';");
            Sql("delete from dbo.ActivityTypes where Id='Velomobile';");
            Sql("delete from dbo.ActivityTypes where Id='Wheelchair';");

        }
    }
}
