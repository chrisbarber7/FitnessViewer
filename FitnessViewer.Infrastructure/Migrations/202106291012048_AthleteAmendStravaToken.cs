namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AthleteAmendStravaToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Athletes", "RefreshToken", c => c.String());
            AddColumn("dbo.Athletes", "ExpiresAt", c => c.Int(nullable: false));
            AddColumn("dbo.Athletes", "ExpiresIn", c => c.Int(nullable: false));
            DropColumn("dbo.Athletes", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Athletes", "Email", c => c.String());
            DropColumn("dbo.Athletes", "ExpiresIn");
            DropColumn("dbo.Athletes", "ExpiresAt");
            DropColumn("dbo.Athletes", "RefreshToken");
        }
    }
}
