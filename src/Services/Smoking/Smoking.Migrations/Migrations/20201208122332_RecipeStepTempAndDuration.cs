using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adams.Services.Smoking.Migrations
{
    public partial class RecipeStepTempAndDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                schema: "smoker",
                table: "recipeSteps",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Temperature",
                schema: "smoker",
                table: "recipeSteps",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                schema: "smoker",
                table: "recipeSteps");

            migrationBuilder.DropColumn(
                name: "Temperature",
                schema: "smoker",
                table: "recipeSteps");
        }
    }
}
