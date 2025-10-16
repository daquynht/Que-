using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Que.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrectAnswerToQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorrectAswer",
                table: "Questions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAswer",
                table: "Questions");
        }
    }
}
