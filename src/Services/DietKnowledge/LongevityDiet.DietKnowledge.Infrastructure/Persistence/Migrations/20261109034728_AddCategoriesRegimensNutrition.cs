using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LongevityDiet.DietKnowledge.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriesRegimensNutrition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    CategoryType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            // Keep the old Foods.Category text: turn each distinct value into a Categories row and link it.
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Foods",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql(
                @"INSERT INTO ""Categories"" (""Id"", ""Name"", ""CategoryType"", ""Description"", ""IsActive"", ""CreatedAtUtc"")
                  SELECT gen_random_uuid(), c.""Category"", 'Food', '', TRUE, NOW()
                  FROM (SELECT DISTINCT ""Category"" FROM ""Foods"") c;");

            migrationBuilder.Sql(
                @"UPDATE ""Foods"" f SET ""CategoryId"" = c.""Id""
                  FROM ""Categories"" c
                  WHERE c.""CategoryType"" = 'Food' AND c.""Name"" = f.""Category"";");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Foods",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.DropIndex(
                name: "IX_Foods_Category",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Foods");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Recipes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "DietGuidelines",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DietRegimens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    DurationDays = table.Column<int>(type: "integer", nullable: false),
                    SourceTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SourceChapter = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SourcePage = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    ReviewStatus = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietRegimens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NutritionFacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FoodId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServingSize = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    EnergyKcal = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    ProteinG = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    CarbohydrateG = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    FatG = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    FiberG = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    SourceReference = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionFacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NutritionFacts_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    StepNumber = table.Column<int>(type: "integer", nullable: false),
                    Instruction = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeSteps_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DietRegimenGuidelines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DietRegimenId = table.Column<Guid>(type: "uuid", nullable: false),
                    GuidelineId = table.Column<Guid>(type: "uuid", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietRegimenGuidelines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DietRegimenGuidelines_DietGuidelines_GuidelineId",
                        column: x => x.GuidelineId,
                        principalTable: "DietGuidelines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DietRegimenGuidelines_DietRegimens_DietRegimenId",
                        column: x => x.DietRegimenId,
                        principalTable: "DietRegimens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DietRegimenMeals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DietRegimenId = table.Column<Guid>(type: "uuid", nullable: false),
                    DayNumber = table.Column<int>(type: "integer", nullable: false),
                    MealSlot = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietRegimenMeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DietRegimenMeals_DietRegimens_DietRegimenId",
                        column: x => x.DietRegimenId,
                        principalTable: "DietRegimens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DietRegimenMeals_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CategoryId",
                table: "Recipes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_CategoryId",
                table: "Foods",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DietGuidelines_CategoryId",
                table: "DietGuidelines",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryType_Name",
                table: "Categories",
                columns: new[] { "CategoryType", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DietRegimenGuidelines_DietRegimenId_GuidelineId",
                table: "DietRegimenGuidelines",
                columns: new[] { "DietRegimenId", "GuidelineId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DietRegimenGuidelines_GuidelineId",
                table: "DietRegimenGuidelines",
                column: "GuidelineId");

            migrationBuilder.CreateIndex(
                name: "IX_DietRegimenMeals_DietRegimenId_DayNumber_MealSlot",
                table: "DietRegimenMeals",
                columns: new[] { "DietRegimenId", "DayNumber", "MealSlot" });

            migrationBuilder.CreateIndex(
                name: "IX_DietRegimenMeals_RecipeId",
                table: "DietRegimenMeals",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_DietRegimens_Name",
                table: "DietRegimens",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DietRegimens_ReviewStatus",
                table: "DietRegimens",
                column: "ReviewStatus");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionFacts_FoodId",
                table: "NutritionFacts",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSteps_RecipeId_StepNumber",
                table: "RecipeSteps",
                columns: new[] { "RecipeId", "StepNumber" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DietGuidelines_Categories_CategoryId",
                table: "DietGuidelines",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Categories_CategoryId",
                table: "Foods",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Categories_CategoryId",
                table: "Recipes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DietGuidelines_Categories_CategoryId",
                table: "DietGuidelines");

            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Categories_CategoryId",
                table: "Foods");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Categories_CategoryId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "DietRegimenGuidelines");

            migrationBuilder.DropTable(
                name: "DietRegimenMeals");

            migrationBuilder.DropTable(
                name: "NutritionFacts");

            migrationBuilder.DropTable(
                name: "RecipeSteps");

            migrationBuilder.DropTable(
                name: "DietRegimens");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_CategoryId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Foods_CategoryId",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_DietGuidelines_CategoryId",
                table: "DietGuidelines");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "DietGuidelines");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Foods",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_Category",
                table: "Foods",
                column: "Category");
        }
    }
}
