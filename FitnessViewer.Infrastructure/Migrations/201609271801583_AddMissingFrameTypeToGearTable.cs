namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingFrameTypeToGearTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gears", "FrameType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Gears", "FrameType");
        }
    }
}
