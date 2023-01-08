using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexus.WeightTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class mssqlmigration571 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientMetrics_Clients_ClientId",
                schema: "Weight",
                table: "ClientMetrics");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                schema: "Weight",
                table: "ClientMetrics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientMetrics_Clients_ClientId",
                schema: "Weight",
                table: "ClientMetrics",
                column: "ClientId",
                principalSchema: "Weight",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientMetrics_Clients_ClientId",
                schema: "Weight",
                table: "ClientMetrics");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                schema: "Weight",
                table: "ClientMetrics",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientMetrics_Clients_ClientId",
                schema: "Weight",
                table: "ClientMetrics",
                column: "ClientId",
                principalSchema: "Weight",
                principalTable: "Clients",
                principalColumn: "ClientId");
        }
    }
}
