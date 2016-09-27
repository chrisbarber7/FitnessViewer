namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGearTypeToGear : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gears", "GearType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Gears", "GearType");
        }
    }
}
