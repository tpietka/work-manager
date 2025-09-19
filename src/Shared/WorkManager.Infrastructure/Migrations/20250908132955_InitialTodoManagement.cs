using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialTodoManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false, comment: "The title/name of the todo item"),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true, comment: "Optional detailed description"),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false, comment: "Current status of the todo item"),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false, comment: "Priority value from 1 (Very Low) to 5 (Very High)"),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')", comment: "When the todo was created"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "When the todo was last modified"),
                    Deadline = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "Optional deadline for completion"),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "When the todo was marked as completed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TodoLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TodoItemId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "Reference to the todo item"),
                    Action = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "Type of action performed"),
                    OldValue = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, comment: "Previous value before change"),
                    NewValue = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, comment: "New value after change"),
                    ChangedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')", comment: "When the change occurred"),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false, comment: "Human-readable description of the change")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoLogs_TodoItems_TodoItemId",
                        column: x => x.TodoItemId,
                        principalTable: "TodoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TodoItems",
                columns: new[] { "Id", "CompletedAt", "CreatedAt", "Deadline", "Description", "Priority", "Status", "Title", "UpdatedAt" },
                values: new object[] { new Guid("12345678-1234-5678-9abc-123456789012"), null, new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), null, "This is your first todo item. Try editing it, changing its status, or creating new ones.", 3, "Todo", "Welcome to Work Management App! 🎉", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_CreatedAt",
                table: "TodoItems",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_Deadline",
                table: "TodoItems",
                column: "Deadline");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_Status",
                table: "TodoItems",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_Status_Priority",
                table: "TodoItems",
                columns: new[] { "Status", "Priority" });

            migrationBuilder.CreateIndex(
                name: "IX_TodoLogs_ChangedAt",
                table: "TodoLogs",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TodoLogs_TodoItemId",
                table: "TodoLogs",
                column: "TodoItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoLogs");

            migrationBuilder.DropTable(
                name: "TodoItems");
        }
    }
}
