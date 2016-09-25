namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActivityTypeTable : DbMigration
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
                        Activity_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.Activity_Id)
                .Index(t => t.Activity_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityTypes", "Activity_Id", "dbo.Activities");
            DropIndex("dbo.ActivityTypes", new[] { "Activity_Id" });
            DropTable("dbo.ActivityTypes");
        }
    }
}
