namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MetricTableAddIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Metrics", new[] { "UserId" });
            CreateIndex("dbo.Metrics", new[] { "UserId", "Recorded", "MetricType" }, unique: true, name: "IX_Metric_UserIdRecorded");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Metrics", "IX_Metric_UserIdRecorded");
            CreateIndex("dbo.Metrics", "UserId");
        }
    }
}
