using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexus.WeightTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class mssqlmigration375 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                schema: "Weight",
                table: "Clients",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                schema: "Weight",
                table: "Clients");
        }
    }
}
