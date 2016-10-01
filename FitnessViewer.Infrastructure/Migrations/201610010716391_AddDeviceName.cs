namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeviceName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "DeviceName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "DeviceName");
        }
    }
}
