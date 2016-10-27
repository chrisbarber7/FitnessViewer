namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateSPToRemoveStreamForActivity : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                  "dbo.ActivityStreamDelete",
                  p => new { ActivityId = p.Long() },
                  body:
                      @"BEGIN TRANSACTION

                    BEGIN TRY

	                DELETE FROM [dbo].[ActivityPeakDetails] WHERE ActivityId=@ActivityId
	                DELETE FROM [dbo].[ActivityPeaks] WHERE ActivityId=@ActivityId	              
                    DELETE FROM [dbo].[Streams] WHERE ActivityId=@ActivityId  
	
                    COMMIT
                END TRY
                BEGIN CATCH
                    ROLLBACK TRAN
                    RAISERROR('There was an error whilst deleteing stream.  The Transaction has been rolled back', 5, 1)
                END CATCH"
  );
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.ActivityStreamDelete");
        }
    }
}
