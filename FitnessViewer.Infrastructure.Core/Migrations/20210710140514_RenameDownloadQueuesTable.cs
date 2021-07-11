using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessViewer.Infrastructure.Core.Migrations
{
    public partial class RenameDownloadQueuesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queue_AspNetUsers_UserId",
                table: "Queue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Queue",
                table: "Queue");

            migrationBuilder.RenameTable(
                name: "Queue",
                newName: "DownloadQueues");

            migrationBuilder.RenameIndex(
                name: "IX_Queue_UserId",
                table: "DownloadQueues",
                newName: "IX_DownloadQueues_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DownloadQueues",
                table: "DownloadQueues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DownloadQueues_AspNetUsers_UserId",
                table: "DownloadQueues",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DownloadQueues_AspNetUsers_UserId",
                table: "DownloadQueues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DownloadQueues",
                table: "DownloadQueues");

            migrationBuilder.RenameTable(
                name: "DownloadQueues",
                newName: "Queue");

            migrationBuilder.RenameIndex(
                name: "IX_DownloadQueues_UserId",
                table: "Queue",
                newName: "IX_Queue_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Queue",
                table: "Queue",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Queue_AspNetUsers_UserId",
                table: "Queue",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
