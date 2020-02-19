using Microsoft.EntityFrameworkCore.Migrations;

namespace bFit.Web.Migrations
{
    public partial class _20200215 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Athletes_Gyms_GymLocalId",
                table: "Athletes");

            migrationBuilder.DropIndex(
                name: "IX_Athletes_GymLocalId",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Franchises");

            migrationBuilder.DropColumn(
                name: "GymLocalId",
                table: "Athletes");

            migrationBuilder.AddColumn<int>(
                name: "LocalGymId",
                table: "Athletes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Athletes_LocalGymId",
                table: "Athletes",
                column: "LocalGymId");

            migrationBuilder.AddForeignKey(
                name: "FK_Athletes_Gyms_LocalGymId",
                table: "Athletes",
                column: "LocalGymId",
                principalTable: "Gyms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Athletes_Gyms_LocalGymId",
                table: "Athletes");

            migrationBuilder.DropIndex(
                name: "IX_Athletes_LocalGymId",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "LocalGymId",
                table: "Athletes");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Franchises",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GymLocalId",
                table: "Athletes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Athletes_GymLocalId",
                table: "Athletes",
                column: "GymLocalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Athletes_Gyms_GymLocalId",
                table: "Athletes",
                column: "GymLocalId",
                principalTable: "Gyms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
