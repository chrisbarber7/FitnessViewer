namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixActivityTypesRunDescription : DbMigration
    {
        public override void Up()
        {
            Sql("update dbo.ActivityTypes set description ='Run' where id='Run'");
        }
        
        public override void Down()
        {
        }
    }
}
