namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StreamAddUniqueIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Streams", new[] { "ActivityId" });
            CreateIndex("dbo.Streams", new[] { "ActivityId", "Time" }, unique: true, name: "IX_Stream_ActivityIdAndStream");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Streams", "IX_Stream_ActivityIdAndStream");
            CreateIndex("dbo.Streams", "ActivityId");
        }
    }
}
