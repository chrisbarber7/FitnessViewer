namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameStreamTypeDurationToPeakStreamTypeDuration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PeakStreamTypeDurations",
                c => new
                    {
                        PeakStreamType = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PeakStreamType, t.Duration });
            
            DropTable("dbo.StreamTypeDurations");

            // Power = 1,
            // HeartRate = 2,
            // Cadence = 3,

            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(1, 5); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(1, 10); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(1, 30); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(1, 60); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(1, 120); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(1, 300); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(1, 360); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(1, 600); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(1, 720); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(1, 1200); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(1, 1800); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(1, 3600); ");

            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(2, 60); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(2, 120); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(2, 300); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(2, 360); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(2, 600); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(2, 720); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(2, 1200); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(2, 1800); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(2, 3600); ");

            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(3, 5); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(3, 10); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(3, 30); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(3, 60); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(3, 120); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(3, 300); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(3, 360); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(3, 600); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(3, 720); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(3, 1200); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(3, 1800); ");
            Sql("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(3, 3600); ");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StreamTypeDurations",
                c => new
                    {
                        StreamType = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StreamType, t.Duration });
            
            DropTable("dbo.PeakStreamTypeDurations");
        }
    }
}
