using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexus.WeightTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class mssqlmigration758 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "RecordedValue",
                schema: "Weight",
                table: "ClientMetrics",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "RecordedValue",
                schema: "Weight",
                table: "ClientMetrics",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
