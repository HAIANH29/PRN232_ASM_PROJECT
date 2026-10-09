using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LongevityDiet.DietKnowledge.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBookKnowledgeReviewMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewStatus",
                table: "Recipes",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "NeedsReview");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReviewedAtUtc",
                table: "Recipes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewedBy",
                table: "Recipes",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceChapter",
                table: "Recipes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourcePage",
                table: "Recipes",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceReference",
                table: "Recipes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "Legacy content requires source verification.");

            migrationBuilder.AddColumn<string>(
                name: "SourceTitle",
                table: "Recipes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "The Longevity Diet");

            migrationBuilder.AddColumn<string>(
                name: "ReviewStatus",
                table: "Foods",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "NeedsReview");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReviewedAtUtc",
                table: "Foods",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewedBy",
                table: "Foods",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceChapter",
                table: "Foods",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourcePage",
                table: "Foods",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceReference",
                table: "Foods",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "Legacy content requires source verification.");

            migrationBuilder.AddColumn<string>(
                name: "SourceTitle",
                table: "Foods",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "The Longevity Diet");

            migrationBuilder.AddColumn<string>(
                name: "ReviewStatus",
                table: "DietGuidelines",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "NeedsReview");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReviewedAtUtc",
                table: "DietGuidelines",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewedBy",
                table: "DietGuidelines",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceChapter",
                table: "DietGuidelines",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourcePage",
                table: "DietGuidelines",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceReference",
                table: "DietGuidelines",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "Legacy content requires source verification.");

            migrationBuilder.AddColumn<string>(
                name: "SourceTitle",
                table: "DietGuidelines",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "The Longevity Diet");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ReviewStatus",
                table: "Recipes",
                column: "ReviewStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_ReviewStatus",
                table: "Foods",
                column: "ReviewStatus");

            migrationBuilder.CreateIndex(
                name: "IX_DietGuidelines_ReviewStatus",
                table: "DietGuidelines",
                column: "ReviewStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recipes_ReviewStatus",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Foods_ReviewStatus",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_DietGuidelines_ReviewStatus",
                table: "DietGuidelines");

            migrationBuilder.DropColumn(
                name: "ReviewStatus",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ReviewedAtUtc",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ReviewedBy",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "SourceChapter",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "SourcePage",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "SourceReference",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "SourceTitle",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ReviewStatus",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "ReviewedAtUtc",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "ReviewedBy",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "SourceChapter",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "SourcePage",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "SourceReference",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "SourceTitle",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "ReviewStatus",
                table: "DietGuidelines");

            migrationBuilder.DropColumn(
                name: "ReviewedAtUtc",
                table: "DietGuidelines");

            migrationBuilder.DropColumn(
                name: "ReviewedBy",
                table: "DietGuidelines");

            migrationBuilder.DropColumn(
                name: "SourceChapter",
                table: "DietGuidelines");

            migrationBuilder.DropColumn(
                name: "SourcePage",
                table: "DietGuidelines");

            migrationBuilder.DropColumn(
                name: "SourceReference",
                table: "DietGuidelines");

            migrationBuilder.DropColumn(
                name: "SourceTitle",
                table: "DietGuidelines");
        }
    }
}
