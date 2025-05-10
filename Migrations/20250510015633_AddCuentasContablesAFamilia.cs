using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class AddCuentasContablesAFamilia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CuentaAjustesId",
                table: "Familias",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaComprasInventariosId",
                table: "Familias",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaCostoMateriaPrimaId",
                table: "Familias",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaCostoVentasGastosId",
                table: "Familias",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaDescuentosId",
                table: "Familias",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaDevolucionesId",
                table: "Familias",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaVentasId",
                table: "Familias",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 1, 56, 32, 514, DateTimeKind.Utc).AddTicks(9766));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 1, 56, 32, 514, DateTimeKind.Utc).AddTicks(9768));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 1, 56, 32, 514, DateTimeKind.Utc).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 1, 56, 32, 514, DateTimeKind.Utc).AddTicks(9771));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 1, 56, 32, 514, DateTimeKind.Utc).AddTicks(9773));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 1, 56, 32, 514, DateTimeKind.Utc).AddTicks(9775));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 1, 56, 32, 515, DateTimeKind.Utc).AddTicks(1407), new DateTime(2025, 5, 10, 1, 56, 32, 515, DateTimeKind.Utc).AddTicks(1408) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 1, 56, 32, 515, DateTimeKind.Utc).AddTicks(1411), new DateTime(2025, 5, 10, 1, 56, 32, 515, DateTimeKind.Utc).AddTicks(1411) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 1, 56, 32, 518, DateTimeKind.Utc).AddTicks(6561));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 1, 56, 32, 518, DateTimeKind.Utc).AddTicks(6573));

            migrationBuilder.CreateIndex(
                name: "IX_Familias_CuentaAjustesId",
                table: "Familias",
                column: "CuentaAjustesId");

            migrationBuilder.CreateIndex(
                name: "IX_Familias_CuentaComprasInventariosId",
                table: "Familias",
                column: "CuentaComprasInventariosId");

            migrationBuilder.CreateIndex(
                name: "IX_Familias_CuentaCostoMateriaPrimaId",
                table: "Familias",
                column: "CuentaCostoMateriaPrimaId");

            migrationBuilder.CreateIndex(
                name: "IX_Familias_CuentaCostoVentasGastosId",
                table: "Familias",
                column: "CuentaCostoVentasGastosId");

            migrationBuilder.CreateIndex(
                name: "IX_Familias_CuentaDescuentosId",
                table: "Familias",
                column: "CuentaDescuentosId");

            migrationBuilder.CreateIndex(
                name: "IX_Familias_CuentaDevolucionesId",
                table: "Familias",
                column: "CuentaDevolucionesId");

            migrationBuilder.CreateIndex(
                name: "IX_Familias_CuentaVentasId",
                table: "Familias",
                column: "CuentaVentasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaAjustesId",
                table: "Familias",
                column: "CuentaAjustesId",
                principalTable: "CuentasContables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaComprasInventariosId",
                table: "Familias",
                column: "CuentaComprasInventariosId",
                principalTable: "CuentasContables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaCostoMateriaPrimaId",
                table: "Familias",
                column: "CuentaCostoMateriaPrimaId",
                principalTable: "CuentasContables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaCostoVentasGastosId",
                table: "Familias",
                column: "CuentaCostoVentasGastosId",
                principalTable: "CuentasContables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaDescuentosId",
                table: "Familias",
                column: "CuentaDescuentosId",
                principalTable: "CuentasContables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaDevolucionesId",
                table: "Familias",
                column: "CuentaDevolucionesId",
                principalTable: "CuentasContables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaVentasId",
                table: "Familias",
                column: "CuentaVentasId",
                principalTable: "CuentasContables",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Familias_CuentasContables_CuentaAjustesId",
                table: "Familias");

            migrationBuilder.DropForeignKey(
                name: "FK_Familias_CuentasContables_CuentaComprasInventariosId",
                table: "Familias");

            migrationBuilder.DropForeignKey(
                name: "FK_Familias_CuentasContables_CuentaCostoMateriaPrimaId",
                table: "Familias");

            migrationBuilder.DropForeignKey(
                name: "FK_Familias_CuentasContables_CuentaCostoVentasGastosId",
                table: "Familias");

            migrationBuilder.DropForeignKey(
                name: "FK_Familias_CuentasContables_CuentaDescuentosId",
                table: "Familias");

            migrationBuilder.DropForeignKey(
                name: "FK_Familias_CuentasContables_CuentaDevolucionesId",
                table: "Familias");

            migrationBuilder.DropForeignKey(
                name: "FK_Familias_CuentasContables_CuentaVentasId",
                table: "Familias");

            migrationBuilder.DropIndex(
                name: "IX_Familias_CuentaAjustesId",
                table: "Familias");

            migrationBuilder.DropIndex(
                name: "IX_Familias_CuentaComprasInventariosId",
                table: "Familias");

            migrationBuilder.DropIndex(
                name: "IX_Familias_CuentaCostoMateriaPrimaId",
                table: "Familias");

            migrationBuilder.DropIndex(
                name: "IX_Familias_CuentaCostoVentasGastosId",
                table: "Familias");

            migrationBuilder.DropIndex(
                name: "IX_Familias_CuentaDescuentosId",
                table: "Familias");

            migrationBuilder.DropIndex(
                name: "IX_Familias_CuentaDevolucionesId",
                table: "Familias");

            migrationBuilder.DropIndex(
                name: "IX_Familias_CuentaVentasId",
                table: "Familias");

            migrationBuilder.DropColumn(
                name: "CuentaAjustesId",
                table: "Familias");

            migrationBuilder.DropColumn(
                name: "CuentaComprasInventariosId",
                table: "Familias");

            migrationBuilder.DropColumn(
                name: "CuentaCostoMateriaPrimaId",
                table: "Familias");

            migrationBuilder.DropColumn(
                name: "CuentaCostoVentasGastosId",
                table: "Familias");

            migrationBuilder.DropColumn(
                name: "CuentaDescuentosId",
                table: "Familias");

            migrationBuilder.DropColumn(
                name: "CuentaDevolucionesId",
                table: "Familias");

            migrationBuilder.DropColumn(
                name: "CuentaVentasId",
                table: "Familias");

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 277, DateTimeKind.Utc).AddTicks(9253));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 277, DateTimeKind.Utc).AddTicks(9257));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 277, DateTimeKind.Utc).AddTicks(9259));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 277, DateTimeKind.Utc).AddTicks(9261));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 277, DateTimeKind.Utc).AddTicks(9262));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 277, DateTimeKind.Utc).AddTicks(9264));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 0, 41, 7, 278, DateTimeKind.Utc).AddTicks(942), new DateTime(2025, 5, 10, 0, 41, 7, 278, DateTimeKind.Utc).AddTicks(943) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 0, 41, 7, 278, DateTimeKind.Utc).AddTicks(946), new DateTime(2025, 5, 10, 0, 41, 7, 278, DateTimeKind.Utc).AddTicks(947) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 280, DateTimeKind.Utc).AddTicks(6796));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 280, DateTimeKind.Utc).AddTicks(6803));
        }
    }
}
