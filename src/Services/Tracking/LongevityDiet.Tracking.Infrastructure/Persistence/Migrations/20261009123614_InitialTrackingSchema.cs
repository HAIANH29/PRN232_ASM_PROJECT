using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LongevityDiet.Tracking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialTrackingSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyTrackings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TrackingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyTrackings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgressSummaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PeriodStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PeriodEndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PlannedMeals = table.Column<int>(type: "integer", nullable: false),
                    CompletedMeals = table.Column<int>(type: "integer", nullable: false),
                    CalculatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastNotificationId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastNotificationSentAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressSummaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealTrackings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DailyTrackingId = table.Column<Guid>(type: "uuid", nullable: false),
                    MealPlanItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealTrackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealTrackings_DailyTrackings_DailyTrackingId",
                        column: x => x.DailyTrackingId,
                        principalTable: "DailyTrackings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyTrackings_UserId_TrackingDate",
                table: "DailyTrackings",
                columns: new[] { "UserId", "TrackingDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealTrackings_DailyTrackingId_MealPlanItemId",
                table: "MealTrackings",
                columns: new[] { "DailyTrackingId", "MealPlanItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealTrackings_MealPlanItemId",
                table: "MealTrackings",
                column: "MealPlanItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressSummaries_UserId_PeriodStartDate_PeriodEndDate",
                table: "ProgressSummaries",
                columns: new[] { "UserId", "PeriodStartDate", "PeriodEndDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealTrackings");

            migrationBuilder.DropTable(
                name: "ProgressSummaries");

            migrationBuilder.DropTable(
                name: "DailyTrackings");
        }
    }
}
