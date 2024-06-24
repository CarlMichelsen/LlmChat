using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                name: "Profiles",
                schema: "LlmChat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DefaultSystemMessage = table.Column<string>(type: "text", nullable: false),
                    SelectedModel = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prompts",
                schema: "LlmChat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ModelName = table.Column<string>(type: "text", nullable: false),
                    ModelId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProviderPromptIdentifier = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    InputTokens = table.Column<long>(type: "bigint", nullable: false),
                    OutputTokens = table.Column<long>(type: "bigint", nullable: false),
                    CurrentMillionInputTokenPrice = table.Column<long>(type: "bigint", nullable: false),
                    CurrentMillionOutputTokenPrice = table.Column<long>(type: "bigint", nullable: false),
                    StopReason = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prompts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                schema: "LlmChat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    SystemMessage = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastAppendedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversations_Profiles_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "LlmChat",
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemMessages",
                schema: "LlmChat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    LastAppendedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemMessages_Profiles_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "LlmChat",
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DialogSlices",
                schema: "LlmChat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SelectedMessageGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConversationEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialogSlices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DialogSlices_Conversations_ConversationEntityId",
                        column: x => x.ConversationEntityId,
                        principalSchema: "LlmChat",
                        principalTable: "Conversations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "LlmChat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsUserMessage = table.Column<bool>(type: "boolean", nullable: false),
                    PromptId = table.Column<Guid>(type: "uuid", nullable: true),
                    PreviousMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompletedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DialogSliceEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_DialogSlices_DialogSliceEntityId",
                        column: x => x.DialogSliceEntityId,
                        principalSchema: "LlmChat",
                        principalTable: "DialogSlices",
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
                name: "Content",
                schema: "LlmChat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentType = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    MessageEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Content_Messages_MessageEntityId",
                        column: x => x.MessageEntityId,
                        principalSchema: "LlmChat",
                        principalTable: "Messages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Content_MessageEntityId",
                schema: "LlmChat",
                table: "Content",
                column: "MessageEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_CreatorId",
                schema: "LlmChat",
                table: "Conversations",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_DeletedAtUtc",
                schema: "LlmChat",
                table: "Conversations",
                column: "DeletedAtUtc",
                filter: "\"Conversations\".\"DeletedAtUtc\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DialogSlices_ConversationEntityId",
                schema: "LlmChat",
                table: "DialogSlices",
                column: "ConversationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DialogSliceEntityId",
                schema: "LlmChat",
                table: "Messages",
                column: "DialogSliceEntityId");

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

            migrationBuilder.CreateIndex(
                name: "IX_SystemMessages_CreatorId",
                schema: "LlmChat",
                table: "SystemMessages",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemMessages_DeletedAtUtc",
                schema: "LlmChat",
                table: "SystemMessages",
                column: "DeletedAtUtc",
                filter: "\"SystemMessages\".\"DeletedAtUtc\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Content",
                schema: "LlmChat");

            migrationBuilder.DropTable(
                name: "SystemMessages",
                schema: "LlmChat");

            migrationBuilder.DropTable(
                name: "Messages",
                schema: "LlmChat");

            migrationBuilder.DropTable(
                name: "DialogSlices",
                schema: "LlmChat");

            migrationBuilder.DropTable(
                name: "Prompts",
                schema: "LlmChat");

            migrationBuilder.DropTable(
                name: "Conversations",
                schema: "LlmChat");

            migrationBuilder.DropTable(
                name: "Profiles",
                schema: "LlmChat");
        }
    }
}
