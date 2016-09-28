namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCalendarTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Calendars",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        YearMonth = c.String(nullable: false, maxLength: 6),
                        YearWeek = c.String(nullable: false, maxLength: 6),
                        MonthName = c.String(nullable: false, maxLength: 9),
                        DayName = c.String(nullable: false, maxLength: 8),
                    })
                .PrimaryKey(t => t.Date)
                .Index(t => t.YearMonth)
                .Index(t => t.YearWeek);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Calendars", new[] { "YearWeek" });
            DropIndex("dbo.Calendars", new[] { "YearMonth" });
            DropTable("dbo.Calendars");
        }
    }
}
