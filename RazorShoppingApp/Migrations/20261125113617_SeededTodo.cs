using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RazorShoppingApp.Migrations
{
    /// <inheritdoc />
    public partial class SeededTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishedDate",
                table: "Todos",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Completed", "CreatedDate", "Description", "FinishedDate", "Title", "UserId" },
                values: new object[] { 1, false, new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "A todo list to keep track of goals and such for this application", null, "Add a Todo List!", "50909e80-48e2-45b1-a418-7a3a43ceb3e3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishedDate",
                table: "Todos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
