using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class AddCuentaCostoMateriaPrima : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CuentaCostoMateriaPrimaId",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 13, 14, 37, 22, 872, DateTimeKind.Utc).AddTicks(7187));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 13, 14, 37, 22, 872, DateTimeKind.Utc).AddTicks(7190));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 13, 14, 37, 22, 872, DateTimeKind.Utc).AddTicks(7192));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 13, 14, 37, 22, 872, DateTimeKind.Utc).AddTicks(7193));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 13, 14, 37, 22, 872, DateTimeKind.Utc).AddTicks(7195));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 13, 14, 37, 22, 872, DateTimeKind.Utc).AddTicks(7198));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 13, 14, 37, 22, 872, DateTimeKind.Utc).AddTicks(8774), new DateTime(2025, 5, 13, 14, 37, 22, 872, DateTimeKind.Utc).AddTicks(8775) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 13, 14, 37, 22, 872, DateTimeKind.Utc).AddTicks(8778), new DateTime(2025, 5, 13, 14, 37, 22, 872, DateTimeKind.Utc).AddTicks(8778) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 13, 14, 37, 22, 876, DateTimeKind.Utc).AddTicks(474));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 13, 14, 37, 22, 876, DateTimeKind.Utc).AddTicks(485));

            migrationBuilder.CreateIndex(
                name: "IX_Items_CuentaCostoMateriaPrimaId",
                table: "Items",
                column: "CuentaCostoMateriaPrimaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_CuentasContables_CuentaCostoMateriaPrimaId",
                table: "Items",
                column: "CuentaCostoMateriaPrimaId",
                principalTable: "CuentasContables",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_CuentasContables_CuentaCostoMateriaPrimaId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CuentaCostoMateriaPrimaId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CuentaCostoMateriaPrimaId",
                table: "Items");

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(4475));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(4478));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(4480));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(4482));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(4483));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(4485));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(5912), new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(5913) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(5918), new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(5918) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 372, DateTimeKind.Utc).AddTicks(1546));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 372, DateTimeKind.Utc).AddTicks(1555));
        }
    }
}
