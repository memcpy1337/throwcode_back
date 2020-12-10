using Microsoft.EntityFrameworkCore.Migrations;

namespace throwcode_back.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Problems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Solved = table.Column<int>(type: "int", nullable: false),
                    Trying = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Problems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProblemsDescription",
                columns: table => new
                {
                    ProblemId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExamplesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cheat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitialCodeJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestCasesJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemsDescription", x => x.ProblemId);
                    table.ForeignKey(
                        name: "FK_ProblemsDescription_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProblemUser",
                columns: table => new
                {
                    Solved_ById = table.Column<int>(type: "int", nullable: false),
                    Solved_ProblemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemUser", x => new { x.Solved_ById, x.Solved_ProblemsId });
                    table.ForeignKey(
                        name: "FK_ProblemUser_Problems_Solved_ProblemsId",
                        column: x => x.Solved_ProblemsId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProblemUser_Users_Solved_ById",
                        column: x => x.Solved_ById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Problems",
                columns: new[] { "Id", "Solved", "Title", "Trying", "Type" },
                values: new object[,]
                {
                    { 1, 0, "Найти сумму двух чисел", 0, "Easy" },
                    { 2, 0, "Найти разницу двух чисел", 0, "Easy" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Login", "Password" },
                values: new object[,]
                {
                    { 1, "admin@admin.ru", "admin", "admin" },
                    { 2, "slave@slave.ru", "slave", "slave" }
                });

            migrationBuilder.InsertData(
                table: "ProblemsDescription",
                columns: new[] { "ProblemId", "Cheat", "Description", "ExamplesJson", "Id", "InitialCodeJson", "TestCasesJson" },
                values: new object[] { 1, "К этой задаче нет подсказок", "В этой задаче тербуется найти сумму двух чисел", "[{\"input\":\"a = 1, b = 2\",\"output\":\"3\"},{\"input\":\"a = 3, b = 3\",\"output\":\"6\"}]", 1, "{\"Javascript\":\"JS Code Here!\",\"Csharp\":\"C# Code Here!\",\"CPlus\":\"C++ Code Here!\"}", "[{\"input\":[\"2\",\"1\"],\"output\":\"3\"},{\"input\":[\"2\",\"2\"],\"output\":\"4\"}]" });

            migrationBuilder.InsertData(
                table: "ProblemsDescription",
                columns: new[] { "ProblemId", "Cheat", "Description", "ExamplesJson", "Id", "InitialCodeJson", "TestCasesJson" },
                values: new object[] { 2, "К этой задаче нет подсказок", "В этой задаче тербуется найти разность двух чисел", "[{\"input\":\"a = 2, b = 1\",\"output\":\"1\"},{\"input\":\"a = 3, b = 3\",\"output\":\"0\"}]", 2, "{\"Javascript\":\"JS Code Here!\",\"Csharp\":\"C# Code Here!\",\"CPlus\":\"C++ Code Here!\"}", "[{\"input\":[\"2\",\"1\"],\"output\":\"1\"},{\"input\":[\"2\",\"2\"],\"output\":\"0\"}]" });

            migrationBuilder.CreateIndex(
                name: "IX_ProblemUser_Solved_ProblemsId",
                table: "ProblemUser",
                column: "Solved_ProblemsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProblemsDescription");

            migrationBuilder.DropTable(
                name: "ProblemUser");

            migrationBuilder.DropTable(
                name: "Problems");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
