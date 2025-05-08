using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class RetencionesUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 23, 21, 260, DateTimeKind.Utc).AddTicks(3860));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 23, 21, 260, DateTimeKind.Utc).AddTicks(3863));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 23, 21, 260, DateTimeKind.Utc).AddTicks(3865));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 23, 21, 260, DateTimeKind.Utc).AddTicks(3868));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 23, 21, 260, DateTimeKind.Utc).AddTicks(3869));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 23, 21, 260, DateTimeKind.Utc).AddTicks(3871));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 14, 23, 21, 260, DateTimeKind.Utc).AddTicks(5122), new DateTime(2025, 5, 8, 14, 23, 21, 260, DateTimeKind.Utc).AddTicks(5123) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 14, 23, 21, 260, DateTimeKind.Utc).AddTicks(5126), new DateTime(2025, 5, 8, 14, 23, 21, 260, DateTimeKind.Utc).AddTicks(5127) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 14, 37, 321, DateTimeKind.Utc).AddTicks(8999));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 14, 37, 321, DateTimeKind.Utc).AddTicks(9002));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 14, 37, 321, DateTimeKind.Utc).AddTicks(9004));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 14, 37, 321, DateTimeKind.Utc).AddTicks(9006));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 14, 37, 321, DateTimeKind.Utc).AddTicks(9072));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 14, 37, 321, DateTimeKind.Utc).AddTicks(9074));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 14, 14, 37, 322, DateTimeKind.Utc).AddTicks(1963), new DateTime(2025, 5, 8, 14, 14, 37, 322, DateTimeKind.Utc).AddTicks(1965) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 14, 14, 37, 322, DateTimeKind.Utc).AddTicks(1969), new DateTime(2025, 5, 8, 14, 14, 37, 322, DateTimeKind.Utc).AddTicks(1970) });
        }
    }
}
