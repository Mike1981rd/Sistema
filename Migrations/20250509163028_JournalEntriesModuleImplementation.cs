using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class JournalEntriesModuleImplementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 16, 30, 27, 221, DateTimeKind.Utc).AddTicks(5562));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 16, 30, 27, 221, DateTimeKind.Utc).AddTicks(5565));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 16, 30, 27, 221, DateTimeKind.Utc).AddTicks(5567));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 16, 30, 27, 221, DateTimeKind.Utc).AddTicks(5568));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 16, 30, 27, 221, DateTimeKind.Utc).AddTicks(5570));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 16, 30, 27, 221, DateTimeKind.Utc).AddTicks(5572));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 16, 30, 27, 221, DateTimeKind.Utc).AddTicks(7144), new DateTime(2025, 5, 9, 16, 30, 27, 221, DateTimeKind.Utc).AddTicks(7145) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 16, 30, 27, 221, DateTimeKind.Utc).AddTicks(7148), new DateTime(2025, 5, 9, 16, 30, 27, 221, DateTimeKind.Utc).AddTicks(7148) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 16, 30, 27, 224, DateTimeKind.Utc).AddTicks(2336));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 16, 30, 27, 224, DateTimeKind.Utc).AddTicks(2342));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 841, DateTimeKind.Utc).AddTicks(9385));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 841, DateTimeKind.Utc).AddTicks(9389));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 841, DateTimeKind.Utc).AddTicks(9391));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 841, DateTimeKind.Utc).AddTicks(9393));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 841, DateTimeKind.Utc).AddTicks(9394));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 841, DateTimeKind.Utc).AddTicks(9396));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 41, 8, 842, DateTimeKind.Utc).AddTicks(959), new DateTime(2025, 5, 9, 15, 41, 8, 842, DateTimeKind.Utc).AddTicks(960) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 41, 8, 842, DateTimeKind.Utc).AddTicks(963), new DateTime(2025, 5, 9, 15, 41, 8, 842, DateTimeKind.Utc).AddTicks(963) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 844, DateTimeKind.Utc).AddTicks(9825));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 844, DateTimeKind.Utc).AddTicks(9832));
        }
    }
}
