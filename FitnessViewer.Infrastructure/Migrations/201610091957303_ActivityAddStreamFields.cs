namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivityAddStreamFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "StreamSize", c => c.Int());
            AddColumn("dbo.Activities", "StreamStep", c => c.Int());

            Sql("update activities set streamsize = (select max(time) from streams where activityid = activities.id)");
            Sql("update activities set streamstep = 30 where streamsize > 20000");
            Sql("update activities set streamstep = 20 where streamsize <= 20000");
            Sql("update activities set streamstep = 10 where streamsize <= 10000");
            Sql("update activities set streamstep = 5 where streamsize <= 5000");
            Sql("update activities set streamstep = 2 where streamsize <= 2000");
            Sql("update activities set streamstep = 1 where streamsize <= 500");
        }

        public override void Down()
        {
            DropColumn("dbo.Activities", "StreamStep");
            DropColumn("dbo.Activities", "StreamSize");
        }
    }
}
