using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adams.Services.Identity.Api.Data.Migrations.ConfigurationDb
{
    public partial class ClientsRequireSecretFalse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApiResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 12, 30, 13, 56, 40, 235, DateTimeKind.Utc).AddTicks(4518));

            migrationBuilder.UpdateData(
                table: "ApiResources",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 12, 30, 13, 56, 40, 235, DateTimeKind.Utc).AddTicks(6428));

            migrationBuilder.UpdateData(
                table: "ClientSecrets",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 12, 30, 13, 56, 40, 241, DateTimeKind.Utc).AddTicks(2917));

            migrationBuilder.UpdateData(
                table: "ClientSecrets",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 12, 30, 13, 56, 40, 249, DateTimeKind.Utc).AddTicks(2867));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "RequireClientSecret" },
                values: new object[] { new DateTime(2020, 12, 30, 13, 56, 40, 238, DateTimeKind.Utc).AddTicks(9177), false });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "RequireClientSecret" },
                values: new object[] { new DateTime(2020, 12, 30, 13, 56, 40, 239, DateTimeKind.Utc).AddTicks(1772), false });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created", "RequireClientSecret" },
                values: new object[] { new DateTime(2020, 12, 30, 13, 56, 40, 239, DateTimeKind.Utc).AddTicks(2104), false });

            migrationBuilder.UpdateData(
                table: "IdentityResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 12, 30, 13, 56, 40, 250, DateTimeKind.Utc).AddTicks(3747));

            migrationBuilder.UpdateData(
                table: "IdentityResources",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 12, 30, 13, 56, 40, 250, DateTimeKind.Utc).AddTicks(6599));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApiResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 12, 30, 12, 2, 55, 933, DateTimeKind.Utc).AddTicks(4730));

            migrationBuilder.UpdateData(
                table: "ApiResources",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 12, 30, 12, 2, 55, 933, DateTimeKind.Utc).AddTicks(6352));

            migrationBuilder.UpdateData(
                table: "ClientSecrets",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 12, 30, 12, 2, 55, 938, DateTimeKind.Utc).AddTicks(9313));

            migrationBuilder.UpdateData(
                table: "ClientSecrets",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 12, 30, 12, 2, 55, 942, DateTimeKind.Utc).AddTicks(2250));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "RequireClientSecret" },
                values: new object[] { new DateTime(2020, 12, 30, 12, 2, 55, 936, DateTimeKind.Utc).AddTicks(8355), true });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "RequireClientSecret" },
                values: new object[] { new DateTime(2020, 12, 30, 12, 2, 55, 937, DateTimeKind.Utc).AddTicks(277), true });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created", "RequireClientSecret" },
                values: new object[] { new DateTime(2020, 12, 30, 12, 2, 55, 937, DateTimeKind.Utc).AddTicks(601), true });

            migrationBuilder.UpdateData(
                table: "IdentityResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 12, 30, 12, 2, 55, 943, DateTimeKind.Utc).AddTicks(1363));

            migrationBuilder.UpdateData(
                table: "IdentityResources",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 12, 30, 12, 2, 55, 943, DateTimeKind.Utc).AddTicks(3774));
        }
    }
}
