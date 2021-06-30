namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVirtualRunActivityType : DbMigration
    {
        public override void Up()
        {
            Sql("insert into dbo.ActivityTypes(Id, Description, IsRide, IsRun, IsSwim, IsOther) values('VirtualRun','VirtualRun',0,1,0,0);");
        }
        
        public override void Down()
        {
        }
    }
}
