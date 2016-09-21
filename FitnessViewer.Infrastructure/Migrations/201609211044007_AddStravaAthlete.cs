namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStravaAthlete : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StravaAthletes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ProfileMedium = c.String(),
                        Profile = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        Sex = c.String(),
                        Friend = c.String(),
                        Follower = c.String(),
                        IsPremium = c.Boolean(nullable: false),
                        CreatedAt = c.String(),
                        UpdatedAt = c.String(),
                        ApproveFollowers = c.Boolean(nullable: false),
                        AthleteType = c.Int(nullable: false),
                        DatePreference = c.String(),
                        MeasurementPreference = c.String(),
                        Email = c.String(),
                        FTP = c.Int(),
                        Weight = c.Single(),
                        Token = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StravaAthletes");
        }
    }
}
