using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Que.Migrations
{
    /// <inheritdoc />
    public partial class CategoryandDifficulty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Quizes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Difficulty",
                table: "Quizes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TimeLimit",
                table: "Quizes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Quizes");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Quizes");

            migrationBuilder.DropColumn(
                name: "TimeLimit",
                table: "Quizes");
        }
    }
}
