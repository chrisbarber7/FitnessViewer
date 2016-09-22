namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStravaStreamTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StravaStreams", "Altitude", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StravaStreams", "Altitude");
        }
    }
}
