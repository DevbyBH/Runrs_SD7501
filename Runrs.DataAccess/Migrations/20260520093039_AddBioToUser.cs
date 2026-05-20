using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Runrs.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBioToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClubDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ClubLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    Distance = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clubs_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequesterId = table.Column<int>(type: "int", nullable: false),
                    AddresseeId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friendships_Users_AddresseeId",
                        column: x => x.AddresseeId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Friendships_Users_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memberships_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Memberships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RunEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    MaxParticipants = table.Column<int>(type: "int", nullable: false),
                    EntryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RunEvents_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    RunEventId = table.Column<int>(type: "int", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_RunEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "RunEvents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Bio", "DateOfBirth", "Email", "FirstName", "JoinedAt", "LastName", "PasswordHash", "Username" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1999, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test@gmail.com", "Test", new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Test123", "testuser" },
                    { 2, null, new DateTime(1999, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test2@gmail.com", "Test2", new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "User2", "Test123", "testuser2" },
                    { 3, null, new DateTime(1999, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test@gmail.com", "Test3", new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "User3", "Test123", "testuser3" }
                });

            migrationBuilder.InsertData(
                table: "Clubs",
                columns: new[] { "Id", "ClubDescription", "ClubLocation", "ClubName", "CreatedAt", "Difficulty", "Distance", "ImageUrl", "IsPrivate", "OwnerId", "Type" },
                values: new object[,]
                {
                    { 1, "Join us every Wednesday & Saturday for a 10km run along Petone Esplanade/Beach", "Petone, Wellington", "Hutt Valley Run Club", new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "https://wordpress.nzrunning.co.nz/wp-content/uploads/2025/04/445cover.jpg", false, 1, 0 },
                    { 2, "Wanting a challenge? Join our run club that regularly does the famous 'Bays Route', a 30km scenic route along some of the most beautiful bays Wellington has to offer.", "Wellington CBD, Wellington", "Bay Runners", new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, "https://www.changefitness.co.nz/wp-content/uploads/feb78d35-680f-4f15-81c1-1e7a0183f311.jpg", false, 2, 3 },
                    { 3, "Join our social run club based in Porirua which is open to all levels of fitness. We meet every Saturday at the Porirua pools to complete a 5km run and socialise over coffee after. ", "Porirua, Wellington", "Social Runners WLG", new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 1, "https://cdn.eventfinda.co.nz/uploads/events/transformed/1771695-766708-34.jpg", false, 3, 2 }
                });

            migrationBuilder.InsertData(
                table: "RunEvents",
                columns: new[] { "Id", "ClubId", "CreatedAt", "Distance", "EntryFee", "EventDate", "EventDescription", "EventLocation", "EventTitle", "MaxParticipants" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.0, 0m, new DateTime(2026, 11, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), "Join us for a run along Petone Esplanade. This event will be an introductory event for any new members wanting to come and feel out the club!", "Petone Esplanade, Wellington", "Petone Run For Fun", 20 },
                    { 2, 2, new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 30.0, 20m, new DateTime(2026, 11, 22, 8, 0, 0, 0, DateTimeKind.Unspecified), "The annual BAYS ROUTE MARATHON is coming up. Join us to raise money for local charities in Wellington.", "Wellington Waterfront, Wellington", "Bays Route Marathon", 50 },
                    { 3, 3, new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 5.0, 0m, new DateTime(2026, 6, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), "Our weekly Saturday 5km social run followed by coffee.", "Porirua Pools, Porirua", "Porirua 5km Social Run", 50 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_OwnerId",
                table: "Clubs",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_EventId",
                table: "EventRegistrations",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_UserId",
                table: "EventRegistrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_AddresseeId",
                table: "Friendships",
                column: "AddresseeId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_RequesterId",
                table: "Friendships",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_ClubId",
                table: "Memberships",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_UserId",
                table: "Memberships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RunEvents_ClubId",
                table: "RunEvents",
                column: "ClubId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventRegistrations");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "RunEvents");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
