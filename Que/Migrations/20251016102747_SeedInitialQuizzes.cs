using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Que.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialQuizzes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CorrectAswer",
                table: "Questions",
                newName: "CorrectAnswer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CorrectAnswer",
                table: "Questions",
                newName: "CorrectAswer");
        }
    }
}
