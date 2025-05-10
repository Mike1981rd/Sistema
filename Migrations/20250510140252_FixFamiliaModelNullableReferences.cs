using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class FixFamiliaModelNullableReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Nota",
                table: "Familias",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(4968));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(4971));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(4973));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(4975));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(4976));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(4978));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(6732), new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(6733) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(6736), new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(6736) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 765, DateTimeKind.Utc).AddTicks(3548));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 765, DateTimeKind.Utc).AddTicks(3557));

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaAjustesId",
                table: "Familias",
                column: "CuentaAjustesId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaComprasInventariosId",
                table: "Familias",
                column: "CuentaComprasInventariosId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaCostoMateriaPrimaId",
                table: "Familias",
                column: "CuentaCostoMateriaPrimaId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaCostoVentasGastosId",
                table: "Familias",
                column: "CuentaCostoVentasGastosId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaDescuentosId",
                table: "Familias",
                column: "CuentaDescuentosId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaDevolucionesId",
                table: "Familias",
                column: "CuentaDevolucionesId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaVentasId",
                table: "Familias",
                column: "CuentaVentasId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.AlterColumn<string>(
                name: "Nota",
                table: "Familias",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(8320));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(8323));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(8325));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(8326));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(8328));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(8329));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(9823), new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(9824) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(9826), new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(9827) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 906, DateTimeKind.Utc).AddTicks(2873));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 906, DateTimeKind.Utc).AddTicks(2880));

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
    }
}
