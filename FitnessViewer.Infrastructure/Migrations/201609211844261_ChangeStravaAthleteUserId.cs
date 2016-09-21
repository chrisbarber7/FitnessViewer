namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStravaAthleteUserId : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.StravaAthletes", "UserId");
            AddForeignKey("dbo.StravaAthletes", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StravaAthletes", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.StravaAthletes", new[] { "UserId" });
        }
    }
}
