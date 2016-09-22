namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQueue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StravaQueues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Added = c.DateTime(nullable: false),
                        Processed = c.Boolean(nullable: false),
                        ProcessedAt = c.DateTime(),
                        StravaActivityId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StravaActivities", t => t.StravaActivityId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.StravaActivityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StravaQueues", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StravaQueues", "StravaActivityId", "dbo.StravaActivities");
            DropIndex("dbo.StravaQueues", new[] { "StravaActivityId" });
            DropIndex("dbo.StravaQueues", new[] { "UserId" });
            DropTable("dbo.StravaQueues");
        }
    }
}
