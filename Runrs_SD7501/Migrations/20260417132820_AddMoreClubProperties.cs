using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Runrs_SD7501.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreClubProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Clubs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Distance",
                table: "Clubs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Clubs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Difficulty", "Distance", "Type" },
                values: new object[] { 1, 1, 0 });

            migrationBuilder.UpdateData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Difficulty", "Distance", "Type" },
                values: new object[] { 2, 2, 3 });

            migrationBuilder.UpdateData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Difficulty", "Distance", "Type" },
                values: new object[] { 0, 1, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Clubs");
        }
    }
}
