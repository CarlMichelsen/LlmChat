using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "LlmChat");

            migrationBuilder.CreateTable(
                name: "Conversations",
                schema: "LlmChat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatorIdentifier = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prompts",
                schema: "LlmChat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProviderPromptIdentifier = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    InputTokens = table.Column<long>(type: "bigint", nullable: false),
                    OutputTokens = table.Column<long>(type: "bigint", nullable: false),
                    CurrentMillionInputTokenPrice = table.Column<long>(type: "bigint", nullable: false),
                    CurrentMillionOutputTokenPrice = table.Column<long>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prompts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "LlmChat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PromptId = table.Column<long>(type: "bigint", nullable: true),
                    PreviousMessageId = table.Column<long>(type: "bigint", nullable: true),
                    CompletedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConversationEntityId = table.Column<long>(type: "bigint", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_ConversationEntityId",
                        column: x => x.ConversationEntityId,
                        principalSchema: "LlmChat",
                        principalTable: "Conversations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_Messages_PreviousMessageId",
                        column: x => x.PreviousMessageId,
                        principalSchema: "LlmChat",
                        principalTable: "Messages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_Prompts_PromptId",
                        column: x => x.PromptId,
                        principalSchema: "LlmChat",
                        principalTable: "Prompts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContentEntity",
                schema: "LlmChat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentType = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    MessageEntityId = table.Column<long>(type: "bigint", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentEntity_Messages_MessageEntityId",
                        column: x => x.MessageEntityId,
                        principalSchema: "LlmChat",
                        principalTable: "Messages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentEntity_MessageEntityId",
                schema: "LlmChat",
                table: "ContentEntity",
                column: "MessageEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationEntityId",
                schema: "LlmChat",
                table: "Messages",
                column: "ConversationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_PreviousMessageId",
                schema: "LlmChat",
                table: "Messages",
                column: "PreviousMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_PromptId",
                schema: "LlmChat",
                table: "Messages",
                column: "PromptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentEntity",
                schema: "LlmChat");

            migrationBuilder.DropTable(
                name: "Messages",
                schema: "LlmChat");

            migrationBuilder.DropTable(
                name: "Conversations",
                schema: "LlmChat");

            migrationBuilder.DropTable(
                name: "Prompts",
                schema: "LlmChat");
        }
    }
}
