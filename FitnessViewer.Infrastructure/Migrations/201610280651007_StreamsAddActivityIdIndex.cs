namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StreamsAddActivityIdIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Streams", "ActivityId", name: "IX_Stream_ActivityId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Streams", "IX_Stream_ActivityId");
        }
    }
}
