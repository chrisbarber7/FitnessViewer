using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessViewer.Infrastructure.Core.Migrations
{
    public partial class RenameTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_ActivityType_ActivityTypeId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Athlete_AthleteId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Calendar_Start",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityPeak_Activity_ActivityId",
                table: "ActivityPeak");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityPeakDetail_Activity_ActivityId",
                table: "ActivityPeakDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Athlete_AspNetUsers_UserId",
                table: "Athlete");

            migrationBuilder.DropForeignKey(
                name: "FK_Athlete_AthleteSetting_AthleteSettingId",
                table: "Athlete");

            migrationBuilder.DropForeignKey(
                name: "FK_AthleteSetting_AspNetUsers_UserId",
                table: "AthleteSetting");

            migrationBuilder.DropForeignKey(
                name: "FK_BestEffort_Activity_ActivityId",
                table: "BestEffort");

            migrationBuilder.DropForeignKey(
                name: "FK_Gear_Athlete_AthleteId",
                table: "Gear");

            migrationBuilder.DropForeignKey(
                name: "FK_Lap_Activity_ActivityId",
                table: "Lap");

            migrationBuilder.DropForeignKey(
                name: "FK_Metric_AspNetUsers_UserId",
                table: "Metric");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Activity_ActivityId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Stream_Activity_ActivityId",
                table: "Stream");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotification_AspNetUsers_UserId",
                table: "UserNotification");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotification_Notification_NotificationId",
                table: "UserNotification");

            migrationBuilder.DropForeignKey(
                name: "FK_Zone_AspNetUsers_UserId",
                table: "Zone");

            migrationBuilder.DropForeignKey(
                name: "FK_ZoneRange_AspNetUsers_UserId",
                table: "ZoneRange");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ZoneRange",
                table: "ZoneRange");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Zone",
                table: "Zone");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNotification",
                table: "UserNotification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stream",
                table: "Stream");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeakStreamTypeDuration",
                table: "PeakStreamTypeDuration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Metric",
                table: "Metric");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lap",
                table: "Lap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gear",
                table: "Gear");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Calendar",
                table: "Calendar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BestEffort",
                table: "BestEffort");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AthleteSetting",
                table: "AthleteSetting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Athlete",
                table: "Athlete");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityType",
                table: "ActivityType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityPeakDetail",
                table: "ActivityPeakDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityPeak",
                table: "ActivityPeak");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity",
                table: "Activity");

            migrationBuilder.RenameTable(
                name: "ZoneRange",
                newName: "ZoneRanges");

            migrationBuilder.RenameTable(
                name: "Zone",
                newName: "Zones");

            migrationBuilder.RenameTable(
                name: "UserNotification",
                newName: "UserNotifications");

            migrationBuilder.RenameTable(
                name: "Stream",
                newName: "Streams");

            migrationBuilder.RenameTable(
                name: "PeakStreamTypeDuration",
                newName: "PeakStreamTypeDurations");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "Metric",
                newName: "Metrics");

            migrationBuilder.RenameTable(
                name: "Lap",
                newName: "Laps");

            migrationBuilder.RenameTable(
                name: "Gear",
                newName: "Gears");

            migrationBuilder.RenameTable(
                name: "Calendar",
                newName: "Calendars");

            migrationBuilder.RenameTable(
                name: "BestEffort",
                newName: "BestEfforts");

            migrationBuilder.RenameTable(
                name: "AthleteSetting",
                newName: "AthleteSettings");

            migrationBuilder.RenameTable(
                name: "Athlete",
                newName: "Athletes");

            migrationBuilder.RenameTable(
                name: "ActivityType",
                newName: "ActivityTypes");

            migrationBuilder.RenameTable(
                name: "ActivityPeakDetail",
                newName: "ActivityPeakDetails");

            migrationBuilder.RenameTable(
                name: "ActivityPeak",
                newName: "ActivityPeaks");

            migrationBuilder.RenameTable(
                name: "Activity",
                newName: "Activities");

            migrationBuilder.RenameIndex(
                name: "IX_ZoneRange_UserId",
                table: "ZoneRanges",
                newName: "IX_ZoneRanges_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Zone_UserId",
                table: "Zones",
                newName: "IX_Zones_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotification_UserId",
                table: "UserNotifications",
                newName: "IX_UserNotifications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotification_NotificationId",
                table: "UserNotifications",
                newName: "IX_UserNotifications_NotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_ActivityId",
                table: "Notifications",
                newName: "IX_Notifications_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Lap_ActivityId",
                table: "Laps",
                newName: "IX_Laps_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Gear_AthleteId",
                table: "Gears",
                newName: "IX_Gears_AthleteId");

            migrationBuilder.RenameIndex(
                name: "IX_BestEffort_ActivityId",
                table: "BestEfforts",
                newName: "IX_BestEfforts_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Athlete_UserId",
                table: "Athletes",
                newName: "IX_Athletes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Athlete_AthleteSettingId",
                table: "Athletes",
                newName: "IX_Athletes_AthleteSettingId");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityPeakDetail_ActivityId",
                table: "ActivityPeakDetails",
                newName: "IX_ActivityPeakDetails_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityPeak_ActivityId",
                table: "ActivityPeaks",
                newName: "IX_ActivityPeaks_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_Start",
                table: "Activities",
                newName: "IX_Activities_Start");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_AthleteId",
                table: "Activities",
                newName: "IX_Activities_AthleteId");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_ActivityTypeId",
                table: "Activities",
                newName: "IX_Activities_ActivityTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ZoneRanges",
                table: "ZoneRanges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Zones",
                table: "Zones",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNotifications",
                table: "UserNotifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Streams",
                table: "Streams",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeakStreamTypeDurations",
                table: "PeakStreamTypeDurations",
                columns: new[] { "PeakStreamType", "Duration" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Metrics",
                table: "Metrics",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Laps",
                table: "Laps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gears",
                table: "Gears",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Calendars",
                table: "Calendars",
                column: "Date");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BestEfforts",
                table: "BestEfforts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AthleteSettings",
                table: "AthleteSettings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Athletes",
                table: "Athletes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityTypes",
                table: "ActivityTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityPeakDetails",
                table: "ActivityPeakDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityPeaks",
                table: "ActivityPeaks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_ActivityTypes_ActivityTypeId",
                table: "Activities",
                column: "ActivityTypeId",
                principalTable: "ActivityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Athletes_AthleteId",
                table: "Activities",
                column: "AthleteId",
                principalTable: "Athletes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Calendars_Start",
                table: "Activities",
                column: "Start",
                principalTable: "Calendars",
                principalColumn: "Date",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityPeakDetails_Activities_ActivityId",
                table: "ActivityPeakDetails",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityPeaks_Activities_ActivityId",
                table: "ActivityPeaks",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Athletes_AspNetUsers_UserId",
                table: "Athletes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Athletes_AthleteSettings_AthleteSettingId",
                table: "Athletes",
                column: "AthleteSettingId",
                principalTable: "AthleteSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AthleteSettings_AspNetUsers_UserId",
                table: "AthleteSettings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BestEfforts_Activities_ActivityId",
                table: "BestEfforts",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gears_Athletes_AthleteId",
                table: "Gears",
                column: "AthleteId",
                principalTable: "Athletes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Laps_Activities_ActivityId",
                table: "Laps",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Metrics_AspNetUsers_UserId",
                table: "Metrics",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Activities_ActivityId",
                table: "Notifications",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Streams_Activities_ActivityId",
                table: "Streams",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_AspNetUsers_UserId",
                table: "UserNotifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_Notifications_NotificationId",
                table: "UserNotifications",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZoneRanges_AspNetUsers_UserId",
                table: "ZoneRanges",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_AspNetUsers_UserId",
                table: "Zones",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_ActivityTypes_ActivityTypeId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Athletes_AthleteId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Calendars_Start",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityPeakDetails_Activities_ActivityId",
                table: "ActivityPeakDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityPeaks_Activities_ActivityId",
                table: "ActivityPeaks");

            migrationBuilder.DropForeignKey(
                name: "FK_Athletes_AspNetUsers_UserId",
                table: "Athletes");

            migrationBuilder.DropForeignKey(
                name: "FK_Athletes_AthleteSettings_AthleteSettingId",
                table: "Athletes");

            migrationBuilder.DropForeignKey(
                name: "FK_AthleteSettings_AspNetUsers_UserId",
                table: "AthleteSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_BestEfforts_Activities_ActivityId",
                table: "BestEfforts");

            migrationBuilder.DropForeignKey(
                name: "FK_Gears_Athletes_AthleteId",
                table: "Gears");

            migrationBuilder.DropForeignKey(
                name: "FK_Laps_Activities_ActivityId",
                table: "Laps");

            migrationBuilder.DropForeignKey(
                name: "FK_Metrics_AspNetUsers_UserId",
                table: "Metrics");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Activities_ActivityId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Streams_Activities_ActivityId",
                table: "Streams");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_AspNetUsers_UserId",
                table: "UserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_Notifications_NotificationId",
                table: "UserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ZoneRanges_AspNetUsers_UserId",
                table: "ZoneRanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_AspNetUsers_UserId",
                table: "Zones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Zones",
                table: "Zones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ZoneRanges",
                table: "ZoneRanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNotifications",
                table: "UserNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Streams",
                table: "Streams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeakStreamTypeDurations",
                table: "PeakStreamTypeDurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Metrics",
                table: "Metrics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Laps",
                table: "Laps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gears",
                table: "Gears");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Calendars",
                table: "Calendars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BestEfforts",
                table: "BestEfforts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AthleteSettings",
                table: "AthleteSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Athletes",
                table: "Athletes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityTypes",
                table: "ActivityTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityPeaks",
                table: "ActivityPeaks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityPeakDetails",
                table: "ActivityPeakDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.RenameTable(
                name: "Zones",
                newName: "Zone");

            migrationBuilder.RenameTable(
                name: "ZoneRanges",
                newName: "ZoneRange");

            migrationBuilder.RenameTable(
                name: "UserNotifications",
                newName: "UserNotification");

            migrationBuilder.RenameTable(
                name: "Streams",
                newName: "Stream");

            migrationBuilder.RenameTable(
                name: "PeakStreamTypeDurations",
                newName: "PeakStreamTypeDuration");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "Metrics",
                newName: "Metric");

            migrationBuilder.RenameTable(
                name: "Laps",
                newName: "Lap");

            migrationBuilder.RenameTable(
                name: "Gears",
                newName: "Gear");

            migrationBuilder.RenameTable(
                name: "Calendars",
                newName: "Calendar");

            migrationBuilder.RenameTable(
                name: "BestEfforts",
                newName: "BestEffort");

            migrationBuilder.RenameTable(
                name: "AthleteSettings",
                newName: "AthleteSetting");

            migrationBuilder.RenameTable(
                name: "Athletes",
                newName: "Athlete");

            migrationBuilder.RenameTable(
                name: "ActivityTypes",
                newName: "ActivityType");

            migrationBuilder.RenameTable(
                name: "ActivityPeaks",
                newName: "ActivityPeak");

            migrationBuilder.RenameTable(
                name: "ActivityPeakDetails",
                newName: "ActivityPeakDetail");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Activity");

            migrationBuilder.RenameIndex(
                name: "IX_Zones_UserId",
                table: "Zone",
                newName: "IX_Zone_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ZoneRanges_UserId",
                table: "ZoneRange",
                newName: "IX_ZoneRange_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotifications_UserId",
                table: "UserNotification",
                newName: "IX_UserNotification_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotifications_NotificationId",
                table: "UserNotification",
                newName: "IX_UserNotification_NotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_ActivityId",
                table: "Notification",
                newName: "IX_Notification_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Laps_ActivityId",
                table: "Lap",
                newName: "IX_Lap_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Gears_AthleteId",
                table: "Gear",
                newName: "IX_Gear_AthleteId");

            migrationBuilder.RenameIndex(
                name: "IX_BestEfforts_ActivityId",
                table: "BestEffort",
                newName: "IX_BestEffort_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Athletes_UserId",
                table: "Athlete",
                newName: "IX_Athlete_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Athletes_AthleteSettingId",
                table: "Athlete",
                newName: "IX_Athlete_AthleteSettingId");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityPeaks_ActivityId",
                table: "ActivityPeak",
                newName: "IX_ActivityPeak_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityPeakDetails_ActivityId",
                table: "ActivityPeakDetail",
                newName: "IX_ActivityPeakDetail_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_Start",
                table: "Activity",
                newName: "IX_Activity_Start");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_AthleteId",
                table: "Activity",
                newName: "IX_Activity_AthleteId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_ActivityTypeId",
                table: "Activity",
                newName: "IX_Activity_ActivityTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Zone",
                table: "Zone",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ZoneRange",
                table: "ZoneRange",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNotification",
                table: "UserNotification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stream",
                table: "Stream",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeakStreamTypeDuration",
                table: "PeakStreamTypeDuration",
                columns: new[] { "PeakStreamType", "Duration" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Metric",
                table: "Metric",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lap",
                table: "Lap",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gear",
                table: "Gear",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Calendar",
                table: "Calendar",
                column: "Date");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BestEffort",
                table: "BestEffort",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AthleteSetting",
                table: "AthleteSetting",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Athlete",
                table: "Athlete",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityType",
                table: "ActivityType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityPeak",
                table: "ActivityPeak",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityPeakDetail",
                table: "ActivityPeakDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity",
                table: "Activity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_ActivityType_ActivityTypeId",
                table: "Activity",
                column: "ActivityTypeId",
                principalTable: "ActivityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Athlete_AthleteId",
                table: "Activity",
                column: "AthleteId",
                principalTable: "Athlete",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Calendar_Start",
                table: "Activity",
                column: "Start",
                principalTable: "Calendar",
                principalColumn: "Date",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityPeak_Activity_ActivityId",
                table: "ActivityPeak",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityPeakDetail_Activity_ActivityId",
                table: "ActivityPeakDetail",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Athlete_AspNetUsers_UserId",
                table: "Athlete",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Athlete_AthleteSetting_AthleteSettingId",
                table: "Athlete",
                column: "AthleteSettingId",
                principalTable: "AthleteSetting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AthleteSetting_AspNetUsers_UserId",
                table: "AthleteSetting",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BestEffort_Activity_ActivityId",
                table: "BestEffort",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gear_Athlete_AthleteId",
                table: "Gear",
                column: "AthleteId",
                principalTable: "Athlete",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lap_Activity_ActivityId",
                table: "Lap",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Metric_AspNetUsers_UserId",
                table: "Metric",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Activity_ActivityId",
                table: "Notification",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stream_Activity_ActivityId",
                table: "Stream",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotification_AspNetUsers_UserId",
                table: "UserNotification",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotification_Notification_NotificationId",
                table: "UserNotification",
                column: "NotificationId",
                principalTable: "Notification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zone_AspNetUsers_UserId",
                table: "Zone",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZoneRange_AspNetUsers_UserId",
                table: "ZoneRange",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
