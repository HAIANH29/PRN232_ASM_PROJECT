using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LongevityDiet.MealPlanning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMealPlanDietRegimenId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DietRegimenId",
                table: "MealPlans",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DietRegimenId",
                table: "MealPlans");
        }
    }
}
