using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class productopropina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 13, 19, 16, 859, DateTimeKind.Utc).AddTicks(6560));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 13, 19, 16, 859, DateTimeKind.Utc).AddTicks(6563));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 13, 19, 16, 859, DateTimeKind.Utc).AddTicks(6564));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 13, 19, 16, 859, DateTimeKind.Utc).AddTicks(6572));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 13, 19, 16, 859, DateTimeKind.Utc).AddTicks(6574));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 13, 19, 16, 859, DateTimeKind.Utc).AddTicks(6576));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 16, 13, 19, 16, 859, DateTimeKind.Utc).AddTicks(8015), new DateTime(2025, 5, 16, 13, 19, 16, 859, DateTimeKind.Utc).AddTicks(8015) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 16, 13, 19, 16, 859, DateTimeKind.Utc).AddTicks(8019), new DateTime(2025, 5, 16, 13, 19, 16, 859, DateTimeKind.Utc).AddTicks(8020) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 13, 19, 16, 862, DateTimeKind.Utc).AddTicks(882));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 13, 19, 16, 862, DateTimeKind.Utc).AddTicks(888));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 12, 37, 54, 646, DateTimeKind.Utc).AddTicks(5611));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 12, 37, 54, 646, DateTimeKind.Utc).AddTicks(5613));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 12, 37, 54, 646, DateTimeKind.Utc).AddTicks(5615));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 12, 37, 54, 646, DateTimeKind.Utc).AddTicks(5638));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 12, 37, 54, 646, DateTimeKind.Utc).AddTicks(5640));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 12, 37, 54, 646, DateTimeKind.Utc).AddTicks(5641));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 16, 12, 37, 54, 646, DateTimeKind.Utc).AddTicks(7606), new DateTime(2025, 5, 16, 12, 37, 54, 646, DateTimeKind.Utc).AddTicks(7607) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 16, 12, 37, 54, 646, DateTimeKind.Utc).AddTicks(7610), new DateTime(2025, 5, 16, 12, 37, 54, 646, DateTimeKind.Utc).AddTicks(7611) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 12, 37, 54, 649, DateTimeKind.Utc).AddTicks(2555));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 16, 12, 37, 54, 649, DateTimeKind.Utc).AddTicks(2561));
        }
    }
}
