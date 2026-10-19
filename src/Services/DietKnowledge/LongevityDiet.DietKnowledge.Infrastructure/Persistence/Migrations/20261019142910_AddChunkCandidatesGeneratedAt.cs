using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LongevityDiet.DietKnowledge.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddChunkCandidatesGeneratedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CandidatesGeneratedAtUtc",
                table: "BookSourceChunks",
                type: "timestamp with time zone",
                nullable: true);

            // Chunks that already produced candidates before this column existed count as analyzed.
            migrationBuilder.Sql(
                "UPDATE \"BookSourceChunks\" SET \"CandidatesGeneratedAtUtc\" = NOW() " +
                "WHERE \"Id\" IN (SELECT \"ChunkId\" FROM \"KnowledgeCandidates\" WHERE \"ChunkId\" IS NOT NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CandidatesGeneratedAtUtc",
                table: "BookSourceChunks");
        }
    }
}
