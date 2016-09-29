namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CalendarAddWeekLabel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Calendars", "WeekLabel", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Calendars", "WeekLabel");
        }
    }
}
