namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivitiesAddWeight : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "Weight", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "Weight");
        }
    }
}
