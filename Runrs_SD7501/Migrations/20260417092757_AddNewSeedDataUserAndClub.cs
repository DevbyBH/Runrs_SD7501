using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Runrs_SD7501.Migrations
{
    /// <inheritdoc />
    public partial class AddNewSeedDataUserAndClub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "FirstName", "JoinedAt", "LastName", "PasswordHash", "Username" },
                values: new object[,]
                {
                    { 2, new DateTime(1999, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test2@gmail.com", "Test2", new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "User2", "Test123", "testuser2" },
                    { 3, new DateTime(1999, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test@gmail.com", "Test3", new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "User3", "Test123", "testuser3" }
                });

            migrationBuilder.InsertData(
                table: "Clubs",
                columns: new[] { "Id", "ClubDescription", "ClubLocation", "ClubName", "CreatedAt", "ImageUrl", "IsPrivate", "OwnerId" },
                values: new object[,]
                {
                    { 2, "Wanting a challenge? Join our run club that regularly does the famous 'Bays Route', a 30km scenic route along some of the most beautiful bays Wellington has to offer.", "Wellington CBD, Wellington", "Bay Runners", new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 2 },
                    { 3, "Join our social run club based in Porirua which is open to all levels of fitness. We meet every Saturday at the Porirua pools to complete a 5km run and socialise over coffee after. ", "Porirua, Wellington", "Social Runners WLG", new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "", false, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
