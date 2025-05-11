using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class AddActivoToComprobanteFiscal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "ComprobantesFiscales",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 314, DateTimeKind.Utc).AddTicks(9701));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 314, DateTimeKind.Utc).AddTicks(9703));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 314, DateTimeKind.Utc).AddTicks(9705));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 314, DateTimeKind.Utc).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 314, DateTimeKind.Utc).AddTicks(9708));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 314, DateTimeKind.Utc).AddTicks(9710));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 11, 1, 41, 9, 315, DateTimeKind.Utc).AddTicks(1342), new DateTime(2025, 5, 11, 1, 41, 9, 315, DateTimeKind.Utc).AddTicks(1342) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 11, 1, 41, 9, 315, DateTimeKind.Utc).AddTicks(1346), new DateTime(2025, 5, 11, 1, 41, 9, 315, DateTimeKind.Utc).AddTicks(1346) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 317, DateTimeKind.Utc).AddTicks(6862));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 317, DateTimeKind.Utc).AddTicks(6870));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "ComprobantesFiscales");

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 21, 43, 6, 20, DateTimeKind.Utc).AddTicks(9493));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 21, 43, 6, 20, DateTimeKind.Utc).AddTicks(9496));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 21, 43, 6, 20, DateTimeKind.Utc).AddTicks(9498));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 21, 43, 6, 20, DateTimeKind.Utc).AddTicks(9500));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 21, 43, 6, 20, DateTimeKind.Utc).AddTicks(9501));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 21, 43, 6, 20, DateTimeKind.Utc).AddTicks(9503));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 21, 43, 6, 21, DateTimeKind.Utc).AddTicks(959), new DateTime(2025, 5, 10, 21, 43, 6, 21, DateTimeKind.Utc).AddTicks(960) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 21, 43, 6, 21, DateTimeKind.Utc).AddTicks(963), new DateTime(2025, 5, 10, 21, 43, 6, 21, DateTimeKind.Utc).AddTicks(964) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 21, 43, 6, 24, DateTimeKind.Utc).AddTicks(1880));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 21, 43, 6, 24, DateTimeKind.Utc).AddTicks(1888));
        }
    }
}
