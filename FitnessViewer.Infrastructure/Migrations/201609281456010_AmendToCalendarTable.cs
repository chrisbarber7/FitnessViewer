namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AmendToCalendarTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Calendars", "DayName", c => c.String(nullable: false, maxLength: 9));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Calendars", "DayName", c => c.String(nullable: false, maxLength: 8));
        }
    }
}
