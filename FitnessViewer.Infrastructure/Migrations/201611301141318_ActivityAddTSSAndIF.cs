namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivityAddTSSAndIF : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "TSS", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Activities", "IntensityFactor", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "IntensityFactor");
            DropColumn("dbo.Activities", "TSS");
        }
    }
}
