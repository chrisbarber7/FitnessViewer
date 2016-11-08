namespace FitnessViewer.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ActivityDeleteDetailsAddNotifications : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure(
          "dbo.ActivityDeleteDetails",
          p => new { ActivityId = p.Long() },
          body:
              @"BEGIN TRANSACTION

                    BEGIN TRY

	                    DELETE FROM [dbo].[UserNotifications] where NotificationId in (Select id from [dbo].[Notifications] where activityId=@ActivityId)
	                    DELETE FROM [dbo].[Notifications] where activityId=@ActivityId

    	                DELETE FROM [dbo].[ActivityPeakDetails] WHERE ActivityId=@ActivityId
    	                DELETE FROM [dbo].[ActivityPeaks] WHERE ActivityId=@ActivityId
    	                DELETE FROM [dbo].[BestEfforts] WHERE ActivityId=@ActivityId
    	                DELETE FROM [dbo].[Laps] WHERE ActivityId=@ActivityId
						DELETE FROM [dbo].[Streams] WHERE ActivityId=@ActivityId  
	
                    COMMIT
                END TRY
                BEGIN CATCH
                    ROLLBACK TRAN
                    RAISERROR('There was an error whilst deleteing activity.  The Transaction has been rolled back', 5, 1)
                END CATCH"
);
        }


        public override void Down()
        {
            DropStoredProcedure("dbo.ActivityDeleteDetails");
        }
    }
}