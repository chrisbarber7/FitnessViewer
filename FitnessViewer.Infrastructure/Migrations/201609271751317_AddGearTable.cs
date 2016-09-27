namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGearTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gears",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AthleteId = c.Long(nullable: false),
                        Brand = c.String(),
                        Model = c.String(),
                        Description = c.String(),
                        IsPrimary = c.Boolean(),
                        Name = c.String(),
                        Distance = c.Single(),
                        ResourceState = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Athletes", t => t.AthleteId, cascadeDelete: true)
                .Index(t => t.AthleteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Gears", "AthleteId", "dbo.Athletes");
            DropIndex("dbo.Gears", new[] { "AthleteId" });
            DropTable("dbo.Gears");
        }
    }
}
