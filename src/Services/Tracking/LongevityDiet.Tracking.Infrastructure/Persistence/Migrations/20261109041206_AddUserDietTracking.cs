using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LongevityDiet.Tracking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDietTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserDietTrackingId",
                table: "ProgressSummaries",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserDietTrackingId",
                table: "DailyTrackings",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserDietTrackings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DietRegimenId = table.Column<Guid>(type: "uuid", nullable: true),
                    MealPlanId = table.Column<Guid>(type: "uuid", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDietTrackings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgressSummaries_UserDietTrackingId",
                table: "ProgressSummaries",
                column: "UserDietTrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTrackings_UserDietTrackingId",
                table: "DailyTrackings",
                column: "UserDietTrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDietTrackings_DietRegimenId",
                table: "UserDietTrackings",
                column: "DietRegimenId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDietTrackings_MealPlanId",
                table: "UserDietTrackings",
                column: "MealPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDietTrackings_UserId_Status",
                table: "UserDietTrackings",
                columns: new[] { "UserId", "Status" });

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTrackings_UserDietTrackings_UserDietTrackingId",
                table: "DailyTrackings",
                column: "UserDietTrackingId",
                principalTable: "UserDietTrackings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProgressSummaries_UserDietTrackings_UserDietTrackingId",
                table: "ProgressSummaries",
                column: "UserDietTrackingId",
                principalTable: "UserDietTrackings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyTrackings_UserDietTrackings_UserDietTrackingId",
                table: "DailyTrackings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgressSummaries_UserDietTrackings_UserDietTrackingId",
                table: "ProgressSummaries");

            migrationBuilder.DropTable(
                name: "UserDietTrackings");

            migrationBuilder.DropIndex(
                name: "IX_ProgressSummaries_UserDietTrackingId",
                table: "ProgressSummaries");

            migrationBuilder.DropIndex(
                name: "IX_DailyTrackings_UserDietTrackingId",
                table: "DailyTrackings");

            migrationBuilder.DropColumn(
                name: "UserDietTrackingId",
                table: "ProgressSummaries");

            migrationBuilder.DropColumn(
                name: "UserDietTrackingId",
                table: "DailyTrackings");
        }
    }
}
