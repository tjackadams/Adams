using Microsoft.EntityFrameworkCore.Migrations;

namespace Adams.Services.Smoking.Migrations
{
    public partial class AddProbeTemperature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Temperature",
                schema: "smoker",
                table: "recipeSteps",
                newName: "GrillTemperature");

            migrationBuilder.AddColumn<double>(
                name: "ProbeTemperature",
                schema: "smoker",
                table: "recipeSteps",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProbeTemperature",
                schema: "smoker",
                table: "recipeSteps");

            migrationBuilder.RenameColumn(
                name: "GrillTemperature",
                schema: "smoker",
                table: "recipeSteps",
                newName: "Temperature");
        }
    }
}
