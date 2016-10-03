namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActivityPeakDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityPeakDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Long(nullable: false),
                        Duration = c.Int(nullable: false),
                        Value = c.Int(),
                        Start = c.Int(),
                        StreamType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId, cascadeDelete: true)
                .Index(t => t.ActivityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityPeakDetails", "ActivityId", "dbo.Activities");
            DropIndex("dbo.ActivityPeakDetails", new[] { "ActivityId" });
            DropTable("dbo.ActivityPeakDetails");
        }
    }
}
