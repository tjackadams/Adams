using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adams.Services.Identity.Api.Data.Migrations.ConfigurationDb
{
    public partial class RemoveClientSecrets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ClientSecrets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ClientSecrets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "ApiResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 12, 30, 16, 1, 45, 65, DateTimeKind.Utc).AddTicks(2389));

            migrationBuilder.UpdateData(
                table: "ApiResources",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 12, 30, 16, 1, 45, 65, DateTimeKind.Utc).AddTicks(4476));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 12, 30, 16, 1, 45, 69, DateTimeKind.Utc).AddTicks(7920));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 12, 30, 16, 1, 45, 70, DateTimeKind.Utc).AddTicks(593));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3,
                column: "Created",
                value: new DateTime(2020, 12, 30, 16, 1, 45, 70, DateTimeKind.Utc).AddTicks(961));

            migrationBuilder.UpdateData(
                table: "IdentityResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 12, 30, 16, 1, 45, 73, DateTimeKind.Utc).AddTicks(5087));

            migrationBuilder.UpdateData(
                table: "IdentityResources",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 12, 30, 16, 1, 45, 73, DateTimeKind.Utc).AddTicks(8249));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "ClientSecrets",
                columns: new[] { "Id", "ClientId", "Created", "Description", "Expiration", "Type", "Value" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2020, 12, 30, 13, 56, 40, 241, DateTimeKind.Utc).AddTicks(2917), null, null, "SharedSecret", "K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=" },
                    { 2, 3, new DateTime(2020, 12, 30, 13, 56, 40, 249, DateTimeKind.Utc).AddTicks(2867), null, null, "SharedSecret", "K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=" }
                });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 12, 30, 13, 56, 40, 238, DateTimeKind.Utc).AddTicks(9177));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 12, 30, 13, 56, 40, 239, DateTimeKind.Utc).AddTicks(1772));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3,
                column: "Created",
                value: new DateTime(2020, 12, 30, 13, 56, 40, 239, DateTimeKind.Utc).AddTicks(2104));

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
    }
}
