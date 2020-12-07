using Microsoft.EntityFrameworkCore.Migrations;

namespace Adams.Services.Smoking.Migrations
{
    public partial class AddProteins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProteinId",
                schema: "smoker",
                table: "recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "protein",
                schema: "smoker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_protein", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "smoker",
                table: "protein",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "none" },
                    { 2, "pork" },
                    { 3, "beef" },
                    { 4, "poultry" },
                    { 5, "lamb" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_recipes_ProteinId",
                schema: "smoker",
                table: "recipes",
                column: "ProteinId");

            migrationBuilder.AddForeignKey(
                name: "FK_recipes_protein_ProteinId",
                schema: "smoker",
                table: "recipes",
                column: "ProteinId",
                principalSchema: "smoker",
                principalTable: "protein",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipes_protein_ProteinId",
                schema: "smoker",
                table: "recipes");

            migrationBuilder.DropTable(
                name: "protein",
                schema: "smoker");

            migrationBuilder.DropIndex(
                name: "IX_recipes_ProteinId",
                schema: "smoker",
                table: "recipes");

            migrationBuilder.DropColumn(
                name: "ProteinId",
                schema: "smoker",
                table: "recipes");
        }
    }
}
