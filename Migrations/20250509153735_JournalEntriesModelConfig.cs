using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class JournalEntriesModelConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 37, 34, 930, DateTimeKind.Utc).AddTicks(7749));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 37, 34, 930, DateTimeKind.Utc).AddTicks(7753));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 37, 34, 930, DateTimeKind.Utc).AddTicks(7754));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 37, 34, 930, DateTimeKind.Utc).AddTicks(7756));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 37, 34, 930, DateTimeKind.Utc).AddTicks(7758));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 37, 34, 930, DateTimeKind.Utc).AddTicks(7760));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 37, 34, 930, DateTimeKind.Utc).AddTicks(9322), new DateTime(2025, 5, 9, 15, 37, 34, 930, DateTimeKind.Utc).AddTicks(9324) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 37, 34, 930, DateTimeKind.Utc).AddTicks(9327), new DateTime(2025, 5, 9, 15, 37, 34, 930, DateTimeKind.Utc).AddTicks(9327) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 37, 34, 933, DateTimeKind.Utc).AddTicks(3010));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 37, 34, 933, DateTimeKind.Utc).AddTicks(3016));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 36, 56, 330, DateTimeKind.Utc).AddTicks(9653));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 36, 56, 330, DateTimeKind.Utc).AddTicks(9657));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 36, 56, 330, DateTimeKind.Utc).AddTicks(9659));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 36, 56, 330, DateTimeKind.Utc).AddTicks(9661));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 36, 56, 330, DateTimeKind.Utc).AddTicks(9663));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 36, 56, 330, DateTimeKind.Utc).AddTicks(9665));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 36, 56, 331, DateTimeKind.Utc).AddTicks(2415), new DateTime(2025, 5, 9, 15, 36, 56, 331, DateTimeKind.Utc).AddTicks(2418) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 36, 56, 331, DateTimeKind.Utc).AddTicks(2421), new DateTime(2025, 5, 9, 15, 36, 56, 331, DateTimeKind.Utc).AddTicks(2422) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 36, 56, 334, DateTimeKind.Utc).AddTicks(9973));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 36, 56, 334, DateTimeKind.Utc).AddTicks(9981));
        }
    }
}
