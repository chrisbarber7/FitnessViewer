namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BestEffortAddIndex : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BestEfforts", "Name", c => c.String(maxLength: 20));
            CreateIndex("dbo.BestEfforts", "Name", name: "IX_BestEffort_Name");
            CreateIndex("dbo.BestEfforts", "ElapsedTime", name: "IX_BestEffort_ElapsedTime");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BestEfforts", "IX_BestEffort_ElapsedTime");
            DropIndex("dbo.BestEfforts", "IX_BestEffort_Name");
            AlterColumn("dbo.BestEfforts", "Name", c => c.String());
        }
    }
}
