namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SufferScoreChangeTy : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Activities", "SufferScore", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Activities", "SufferScore", c => c.Int());
        }
    }
}
