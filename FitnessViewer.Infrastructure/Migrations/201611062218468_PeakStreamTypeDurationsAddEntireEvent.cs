namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PeakStreamTypeDurationsAddEntireEvent : DbMigration
    {
        public override void Up()
        {
            Sql(string.Format("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(1, {0}); ", int.MaxValue.ToString()));
            Sql(string.Format("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(2, {0}); ", int.MaxValue.ToString()));
            Sql(string.Format("insert into dbo.PeakStreamTypeDurations(PeakStreamType, Duration) values(3, {0}); ", int.MaxValue.ToString()));
        }
        
        public override void Down()
        {
        }
    }
}
