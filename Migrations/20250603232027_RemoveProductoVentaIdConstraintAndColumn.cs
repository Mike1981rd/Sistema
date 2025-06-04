using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductoVentaIdConstraintAndColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(1105));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(1107));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(1109));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(1119));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(1120));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(1123));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(2820), new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(2820) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(2823), new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(2823) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 920, DateTimeKind.Utc).AddTicks(7254));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 920, DateTimeKind.Utc).AddTicks(7265));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 776, DateTimeKind.Utc).AddTicks(8435));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 776, DateTimeKind.Utc).AddTicks(8438));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 776, DateTimeKind.Utc).AddTicks(8440));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 776, DateTimeKind.Utc).AddTicks(8449));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 776, DateTimeKind.Utc).AddTicks(8451));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 776, DateTimeKind.Utc).AddTicks(8453));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 23, 18, 9, 777, DateTimeKind.Utc).AddTicks(313), new DateTime(2025, 6, 3, 23, 18, 9, 777, DateTimeKind.Utc).AddTicks(314) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 23, 18, 9, 777, DateTimeKind.Utc).AddTicks(317), new DateTime(2025, 6, 3, 23, 18, 9, 777, DateTimeKind.Utc).AddTicks(318) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 779, DateTimeKind.Utc).AddTicks(4156));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 779, DateTimeKind.Utc).AddTicks(4163));
        }
    }
}
