using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexus.WeightTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class mssqlmigration702 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Weight");

            migrationBuilder.CreateTable(
                name: "Clients",
                schema: "Weight",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "ClientMetric",
                schema: "Weight",
                columns: table => new
                {
                    ClientMetricId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordedValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RecordedDate = table.Column<DateTime>(type: "date", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientMetric", x => x.ClientMetricId);
                    table.ForeignKey(
                        name: "FK_ClientMetric_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "Weight",
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientMetric_ClientId",
                schema: "Weight",
                table: "ClientMetric",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientMetric",
                schema: "Weight");

            migrationBuilder.DropTable(
                name: "Clients",
                schema: "Weight");
        }
    }
}
