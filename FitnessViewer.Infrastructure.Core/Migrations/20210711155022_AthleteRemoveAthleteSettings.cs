using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessViewer.Infrastructure.Core.Migrations
{
    public partial class AthleteRemoveAthleteSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Athletes_AthleteSettings_AthleteSettingId",
                table: "Athletes");

            migrationBuilder.DropIndex(
                name: "IX_Athletes_AthleteSettingId",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "AthleteSettingId",
                table: "Athletes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AthleteSettingId",
                table: "Athletes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Athletes_AthleteSettingId",
                table: "Athletes",
                column: "AthleteSettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Athletes_AthleteSettings_AthleteSettingId",
                table: "Athletes",
                column: "AthleteSettingId",
                principalTable: "AthleteSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
