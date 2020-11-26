using Microsoft.EntityFrameworkCore.Migrations;

namespace Adams.Services.Smoking.Migrations
{
    public partial class Initial : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "smoker");

            migrationBuilder.CreateSequence(
                name: "recipeseq",
                schema: "smoker",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "recipestepseq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "recipes",
                schema: "smoker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "recipeSteps",
                schema: "smoker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Step = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipeSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_recipeSteps_recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "smoker",
                        principalTable: "recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_recipeSteps_RecipeId",
                schema: "smoker",
                table: "recipeSteps",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "recipeSteps",
                schema: "smoker");

            migrationBuilder.DropTable(
                name: "recipes",
                schema: "smoker");

            migrationBuilder.DropSequence(
                name: "recipeseq",
                schema: "smoker");

            migrationBuilder.DropSequence(
                name: "recipestepseq");
        }
    }
}
