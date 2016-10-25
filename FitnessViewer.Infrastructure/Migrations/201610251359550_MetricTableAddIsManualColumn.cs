namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MetricTableAddIsManualColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Metrics", "IsManual", c => c.Boolean(nullable: false));

            Sql("update dbo.Metrics set IsManual=0;");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Metrics", "IsManual");
        }
    }
}
