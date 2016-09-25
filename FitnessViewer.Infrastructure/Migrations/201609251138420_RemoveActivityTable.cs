namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveActivityTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActivityTypes", "Activity_Id", "dbo.Activities");
            DropIndex("dbo.ActivityTypes", new[] { "Activity_Id" });
            DropTable("dbo.ActivityTypes");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ActivityTypes", "Activity_Id");
            AddForeignKey("dbo.ActivityTypes", "Activity_Id", "dbo.Activities", "Id");
        }
    }
}
