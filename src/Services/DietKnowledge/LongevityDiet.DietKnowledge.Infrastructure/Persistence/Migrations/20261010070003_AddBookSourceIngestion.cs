using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LongevityDiet.DietKnowledge.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBookSourceIngestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookSourceDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalFileName = table.Column<string>(type: "character varying(260)", maxLength: 260, nullable: false),
                    StoredFileName = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    StoragePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    UploadedBy = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    UploadedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ProcessedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ChunkCount = table.Column<int>(type: "integer", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookSourceDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookSourceChunks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChunkIndex = table.Column<int>(type: "integer", nullable: false),
                    PageNumber = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                    CharacterCount = table.Column<int>(type: "integer", nullable: false),
                    TokenEstimate = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookSourceChunks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookSourceChunks_BookSourceDocuments_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "BookSourceDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeCandidates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChunkId = table.Column<Guid>(type: "uuid", nullable: true),
                    CandidateType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Summary = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    SourceTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SourceChapter = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SourcePage = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    SourceReference = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    GeneratedBy = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReviewedBy = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    ReviewedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ApprovedKnowledgeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeCandidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KnowledgeCandidates_BookSourceChunks_ChunkId",
                        column: x => x.ChunkId,
                        principalTable: "BookSourceChunks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_KnowledgeCandidates_BookSourceDocuments_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "BookSourceDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookSourceChunks_DocumentId_ChunkIndex",
                table: "BookSourceChunks",
                columns: new[] { "DocumentId", "ChunkIndex" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookSourceChunks_PageNumber",
                table: "BookSourceChunks",
                column: "PageNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BookSourceDocuments_Status",
                table: "BookSourceDocuments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BookSourceDocuments_UploadedAtUtc",
                table: "BookSourceDocuments",
                column: "UploadedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeCandidates_CandidateType",
                table: "KnowledgeCandidates",
                column: "CandidateType");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeCandidates_ChunkId",
                table: "KnowledgeCandidates",
                column: "ChunkId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeCandidates_DocumentId",
                table: "KnowledgeCandidates",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeCandidates_Status",
                table: "KnowledgeCandidates",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KnowledgeCandidates");

            migrationBuilder.DropTable(
                name: "BookSourceChunks");

            migrationBuilder.DropTable(
                name: "BookSourceDocuments");
        }
    }
}
