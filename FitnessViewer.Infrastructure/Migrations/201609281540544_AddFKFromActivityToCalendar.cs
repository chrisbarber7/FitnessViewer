namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKFromActivityToCalendar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "Start", c => c.DateTime(nullable: false));

            // need to have just the date part of date time to join to Calendar table.
            Sql("update dbo.activities set start=cast(startdatelocal as date);");
            
            CreateIndex("dbo.Activities", "Start");
            AddForeignKey("dbo.Activities", "Start", "dbo.Calendars", "Date", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "Start", "dbo.Calendars");
            DropIndex("dbo.Activities", new[] { "Start" });
            DropColumn("dbo.Activities", "Start");
        }
    }
}
