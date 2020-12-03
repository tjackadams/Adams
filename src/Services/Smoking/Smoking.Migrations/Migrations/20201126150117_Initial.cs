using Microsoft.EntityFrameworkCore.Migrations;

namespace Adams.Services.Smoking.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                "smoker");

            migrationBuilder.CreateSequence(
                "recipeseq",
                "smoker",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                "recipestepseq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                "recipes",
                schema: "smoker",
                columns: table => new
                {
                    Id = table.Column<int>("int", nullable: false),
                    Description = table.Column<string>("nvarchar(2000)", maxLength: 2000, nullable: false),
                    DisplayName = table.Column<string>("nvarchar(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>("nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_recipes", x => x.Id); });

            migrationBuilder.CreateTable(
                "recipeSteps",
                schema: "smoker",
                columns: table => new
                {
                    Id = table.Column<int>("int", nullable: false),
                    RecipeId = table.Column<int>("int", nullable: false),
                    Description = table.Column<string>("nvarchar(2000)", maxLength: 2000, nullable: false),
                    Step = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipeSteps", x => x.Id);
                    table.ForeignKey(
                        "FK_recipeSteps_recipes_RecipeId",
                        x => x.RecipeId,
                        principalSchema: "smoker",
                        principalTable: "recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_recipeSteps_RecipeId",
                schema: "smoker",
                table: "recipeSteps",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "recipeSteps",
                "smoker");

            migrationBuilder.DropTable(
                "recipes",
                "smoker");

            migrationBuilder.DropSequence(
                "recipeseq",
                "smoker");

            migrationBuilder.DropSequence(
                "recipestepseq");
        }
    }
}