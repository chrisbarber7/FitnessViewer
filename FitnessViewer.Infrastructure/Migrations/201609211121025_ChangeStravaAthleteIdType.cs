namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStravaAthleteIdType : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.StravaAthletes");
            AlterColumn("dbo.StravaAthletes", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.StravaAthletes", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.StravaAthletes");
            AlterColumn("dbo.StravaAthletes", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.StravaAthletes", "Id");
        }
    }
}
