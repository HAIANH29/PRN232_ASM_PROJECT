using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LongevityDiet.DietKnowledge.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCandidateIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IngredientsJson",
                table: "KnowledgeCandidates",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IngredientsJson",
                table: "KnowledgeCandidates");
        }
    }
}
