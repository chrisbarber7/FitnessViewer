namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AthleteAmend : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AthleteSettings");
            AlterColumn("dbo.AthleteSettings", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.AthleteSettings", "Id");
            CreateIndex("dbo.AthleteSettings", "Id");
            AddForeignKey("dbo.AthleteSettings", "Id", "dbo.Athletes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AthleteSettings", "Id", "dbo.Athletes");
            DropIndex("dbo.AthleteSettings", new[] { "Id" });
            DropPrimaryKey("dbo.AthleteSettings");
            AlterColumn("dbo.AthleteSettings", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.AthleteSettings", "Id");
        }
    }
}
