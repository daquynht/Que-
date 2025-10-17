using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Que.Migrations
{
    /// <inheritdoc />
    public partial class QueNy3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Quizes",
                columns: table => new
                {
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Difficulty = table.Column<string>(type: "TEXT", nullable: false),
                    TimeLimit = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizes", x => x.QuizId);
                    table.ForeignKey(
                        name: "FK_Quizes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false),
                    AllowMultipleAnswers = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_Quizes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    OptionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    IsCorrect = table.Column<bool>(type: "INTEGER", nullable: false),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.OptionId);
                    table.ForeignKey(
                        name: "FK_Options_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Quizes",
                columns: new[] { "QuizId", "Category", "Description", "Difficulty", "Name", "TimeLimit", "UserId" },
                values: new object[] { 1, "General", "Test your basic knowledge.", "Medium", "General Knowledge Basics", 10, null });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "QuestionId", "AllowMultipleAnswers", "QuizId", "Text" },
                values: new object[,]
                {
                    { 1, false, 1, "What is the capital of Norway?" },
                    { 2, false, 1, "What is the largest planet in our solar system?" }
                });

            migrationBuilder.InsertData(
                table: "Options",
                columns: new[] { "OptionId", "IsCorrect", "QuestionId", "Text" },
                values: new object[,]
                {
                    { 1, false, 1, "Bergen" },
                    { 2, true, 1, "Oslo" },
                    { 3, false, 1, "Trondheim" },
                    { 4, false, 1, "Stavanger" },
                    { 5, false, 2, "Saturn" },
                    { 6, true, 2, "Jupiter" },
                    { 7, false, 2, "Mars" },
                    { 8, false, 2, "Jorden" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Options_QuestionId",
                table: "Options",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizes_UserId",
                table: "Quizes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Quizes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
