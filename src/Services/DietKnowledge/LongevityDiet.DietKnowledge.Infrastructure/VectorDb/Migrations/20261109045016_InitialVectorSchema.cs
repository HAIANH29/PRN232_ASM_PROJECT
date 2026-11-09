using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace LongevityDiet.DietKnowledge.Infrastructure.VectorDb.Migrations
{
    /// <inheritdoc />
    public partial class InitialVectorSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.CreateTable(
                name: "EmbeddingIndexRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CompletedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ItemsIndexed = table.Column<int>(type: "integer", nullable: false),
                    ItemsFailed = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmbeddingIndexRuns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeEmbeddings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IndexRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Content = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Embedding = table.Column<Vector>(type: "vector(768)", nullable: false),
                    EmbeddingModel = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeEmbeddings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KnowledgeEmbeddings_EmbeddingIndexRuns_IndexRunId",
                        column: x => x.IndexRunId,
                        principalTable: "EmbeddingIndexRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmbeddingIndexRuns_StartedAtUtc",
                table: "EmbeddingIndexRuns",
                column: "StartedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeEmbeddings_Embedding",
                table: "KnowledgeEmbeddings",
                column: "Embedding")
                .Annotation("Npgsql:IndexMethod", "hnsw")
                .Annotation("Npgsql:IndexOperators", new[] { "vector_cosine_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeEmbeddings_IndexRunId",
                table: "KnowledgeEmbeddings",
                column: "IndexRunId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeEmbeddings_SourceType_SourceId",
                table: "KnowledgeEmbeddings",
                columns: new[] { "SourceType", "SourceId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KnowledgeEmbeddings");

            migrationBuilder.DropTable(
                name: "EmbeddingIndexRuns");
        }
    }
}
