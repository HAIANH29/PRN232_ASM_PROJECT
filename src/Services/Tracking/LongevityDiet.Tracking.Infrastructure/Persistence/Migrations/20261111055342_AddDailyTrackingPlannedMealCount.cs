using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LongevityDiet.Tracking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyTrackingPlannedMealCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlannedMealCount",
                table: "DailyTrackings",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlannedMealCount",
                table: "DailyTrackings");
        }
    }
}
