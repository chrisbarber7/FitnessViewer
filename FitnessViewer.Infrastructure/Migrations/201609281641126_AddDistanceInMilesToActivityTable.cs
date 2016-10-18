namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDistanceInMilesToActivityTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "DistanceInMiles", c => c.Single(nullable: true));

            Sql("update dbo.activities set distanceinmiles=distance*0.00062137119");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "DistanceInMiles");
        }
    }
}
