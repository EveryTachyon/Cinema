using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnoCinema.Migrations
{
    /// <inheritdoc />
    public partial class room_linking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "Saal", table: "Showtimes");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Showtimes",
                type: "int",
                nullable: true,
                defaultValue: null);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Spent",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_RoomId",
                table: "Showtimes",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Showtimes_Rooms_RoomId",
                table: "Showtimes",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Showtimes_Rooms_RoomId",
                table: "Showtimes");

            migrationBuilder.DropIndex(
                name: "IX_Showtimes_RoomId",
                table: "Showtimes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Spent",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Showtimes",
                newName: "Saal");
        }
    }
}
