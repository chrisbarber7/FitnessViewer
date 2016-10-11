namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFitbitUserTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FitbitUsers",
                c => new
                    {
                        FitbitUserId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        RefreshToken = c.String(),
                        Token = c.String(),
                        TokenType = c.String(),
                    })
                .PrimaryKey(t => t.FitbitUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FitbitUsers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.FitbitUsers", new[] { "UserId" });
            DropTable("dbo.FitbitUsers");
        }
    }
}
