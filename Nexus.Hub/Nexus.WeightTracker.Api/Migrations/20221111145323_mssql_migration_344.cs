using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexus.WeightTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class mssqlmigration344 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientMetric_Clients_ClientId",
                schema: "Weight",
                table: "ClientMetric");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientMetric",
                schema: "Weight",
                table: "ClientMetric");

            migrationBuilder.RenameTable(
                name: "ClientMetric",
                schema: "Weight",
                newName: "ClientMetrics",
                newSchema: "Weight");

            migrationBuilder.RenameIndex(
                name: "IX_ClientMetric_ClientId",
                schema: "Weight",
                table: "ClientMetrics",
                newName: "IX_ClientMetrics_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientMetrics",
                schema: "Weight",
                table: "ClientMetrics",
                column: "ClientMetricId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientMetrics_Clients_ClientId",
                schema: "Weight",
                table: "ClientMetrics",
                column: "ClientId",
                principalSchema: "Weight",
                principalTable: "Clients",
                principalColumn: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientMetrics_Clients_ClientId",
                schema: "Weight",
                table: "ClientMetrics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientMetrics",
                schema: "Weight",
                table: "ClientMetrics");

            migrationBuilder.RenameTable(
                name: "ClientMetrics",
                schema: "Weight",
                newName: "ClientMetric",
                newSchema: "Weight");

            migrationBuilder.RenameIndex(
                name: "IX_ClientMetrics_ClientId",
                schema: "Weight",
                table: "ClientMetric",
                newName: "IX_ClientMetric_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientMetric",
                schema: "Weight",
                table: "ClientMetric",
                column: "ClientMetricId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientMetric_Clients_ClientId",
                schema: "Weight",
                table: "ClientMetric",
                column: "ClientId",
                principalSchema: "Weight",
                principalTable: "Clients",
                principalColumn: "ClientId");
        }
    }
}
