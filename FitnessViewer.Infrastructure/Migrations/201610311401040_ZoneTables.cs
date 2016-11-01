namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZoneTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Zones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ZoneType = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ZoneRanges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ZoneType = c.Int(nullable: false),
                        ZoneName = c.String(maxLength: 32),
                        ZoneStart = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZoneRanges", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Zones", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ZoneRanges", new[] { "UserId" });
            DropIndex("dbo.Zones", new[] { "UserId" });
            DropTable("dbo.ZoneRanges");
            DropTable("dbo.Zones");
        }
    }
}
