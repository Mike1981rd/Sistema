using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class productscuentascontables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductosModificadoresGrupos_GruposModificadores_GrupoModif~",
                table: "ProductosModificadoresGrupos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosModificadoresGrupos_ProductosVenta_ProductoId",
                table: "ProductosModificadoresGrupos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_Impuestos_ImpuestoId",
                table: "ProductosVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_RutasImpresora_RutaImpresoraId",
                table: "ProductosVenta");

            migrationBuilder.DropIndex(
                name: "IX_VariantesProducto_PLUVariante",
                table: "VariantesProducto");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_PLU",
                table: "ProductosVenta");

            migrationBuilder.AlterColumn<string>(
                name: "AjustePrecioTipo",
                table: "VariantesProducto",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "CuentaAjustesId",
                table: "ProductosVenta",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaComprasInventariosId",
                table: "ProductosVenta",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaCostoMateriaPrimaId",
                table: "ProductosVenta",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaCostoVentasGastosId",
                table: "ProductosVenta",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaDescuentosId",
                table: "ProductosVenta",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaDevolucionesId",
                table: "ProductosVenta",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaVentasId",
                table: "ProductosVenta",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TipoVisualizacionTPV",
                table: "GruposModificadores",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

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

            migrationBuilder.CreateIndex(
                name: "IX_VariantesProducto_PLUVariante",
                table: "VariantesProducto",
                column: "PLUVariante",
                unique: true,
                filter: "\"PLUVariante\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_CuentaAjustesId",
                table: "ProductosVenta",
                column: "CuentaAjustesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_CuentaComprasInventariosId",
                table: "ProductosVenta",
                column: "CuentaComprasInventariosId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_CuentaCostoMateriaPrimaId",
                table: "ProductosVenta",
                column: "CuentaCostoMateriaPrimaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_CuentaCostoVentasGastosId",
                table: "ProductosVenta",
                column: "CuentaCostoVentasGastosId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_CuentaDescuentosId",
                table: "ProductosVenta",
                column: "CuentaDescuentosId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_CuentaDevolucionesId",
                table: "ProductosVenta",
                column: "CuentaDevolucionesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_CuentaVentasId",
                table: "ProductosVenta",
                column: "CuentaVentasId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_PLU",
                table: "ProductosVenta",
                column: "PLU",
                unique: true,
                filter: "\"PLU\" IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosModificadoresGrupos_GruposModificadores_GrupoModif~",
                table: "ProductosModificadoresGrupos",
                column: "GrupoModificadoresId",
                principalTable: "GruposModificadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosModificadoresGrupos_ProductosVenta_ProductoId",
                table: "ProductosModificadoresGrupos",
                column: "ProductoId",
                principalTable: "ProductosVenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaAjustesId",
                table: "ProductosVenta",
                column: "CuentaAjustesId",
                principalTable: "CuentasContables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaComprasInventariosId",
                table: "ProductosVenta",
                column: "CuentaComprasInventariosId",
                principalTable: "CuentasContables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaCostoMateriaPrimaId",
                table: "ProductosVenta",
                column: "CuentaCostoMateriaPrimaId",
                principalTable: "CuentasContables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaCostoVentasGastosId",
                table: "ProductosVenta",
                column: "CuentaCostoVentasGastosId",
                principalTable: "CuentasContables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaDescuentosId",
                table: "ProductosVenta",
                column: "CuentaDescuentosId",
                principalTable: "CuentasContables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaDevolucionesId",
                table: "ProductosVenta",
                column: "CuentaDevolucionesId",
                principalTable: "CuentasContables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaVentasId",
                table: "ProductosVenta",
                column: "CuentaVentasId",
                principalTable: "CuentasContables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_Impuestos_ImpuestoId",
                table: "ProductosVenta",
                column: "ImpuestoId",
                principalTable: "Impuestos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_RutasImpresora_RutaImpresoraId",
                table: "ProductosVenta",
                column: "RutaImpresoraId",
                principalTable: "RutasImpresora",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductosModificadoresGrupos_GruposModificadores_GrupoModif~",
                table: "ProductosModificadoresGrupos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosModificadoresGrupos_ProductosVenta_ProductoId",
                table: "ProductosModificadoresGrupos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaAjustesId",
                table: "ProductosVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaComprasInventariosId",
                table: "ProductosVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaCostoMateriaPrimaId",
                table: "ProductosVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaCostoVentasGastosId",
                table: "ProductosVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaDescuentosId",
                table: "ProductosVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaDevolucionesId",
                table: "ProductosVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaVentasId",
                table: "ProductosVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_Impuestos_ImpuestoId",
                table: "ProductosVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_RutasImpresora_RutaImpresoraId",
                table: "ProductosVenta");

            migrationBuilder.DropIndex(
                name: "IX_VariantesProducto_PLUVariante",
                table: "VariantesProducto");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_CuentaAjustesId",
                table: "ProductosVenta");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_CuentaComprasInventariosId",
                table: "ProductosVenta");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_CuentaCostoMateriaPrimaId",
                table: "ProductosVenta");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_CuentaCostoVentasGastosId",
                table: "ProductosVenta");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_CuentaDescuentosId",
                table: "ProductosVenta");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_CuentaDevolucionesId",
                table: "ProductosVenta");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_CuentaVentasId",
                table: "ProductosVenta");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_PLU",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "CuentaAjustesId",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "CuentaComprasInventariosId",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "CuentaCostoMateriaPrimaId",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "CuentaCostoVentasGastosId",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "CuentaDescuentosId",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "CuentaDevolucionesId",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "CuentaVentasId",
                table: "ProductosVenta");

            migrationBuilder.AlterColumn<int>(
                name: "AjustePrecioTipo",
                table: "VariantesProducto",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "TipoVisualizacionTPV",
                table: "GruposModificadores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(5703));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(5706));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(5708));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(5717));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(5719));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(5721));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(7175), new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(7176) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(7179), new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(7179) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 357, DateTimeKind.Utc).AddTicks(219));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 357, DateTimeKind.Utc).AddTicks(228));

            migrationBuilder.CreateIndex(
                name: "IX_VariantesProducto_PLUVariante",
                table: "VariantesProducto",
                column: "PLUVariante",
                unique: true,
                filter: "PLUVariante IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_PLU",
                table: "ProductosVenta",
                column: "PLU",
                unique: true,
                filter: "PLU IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosModificadoresGrupos_GruposModificadores_GrupoModif~",
                table: "ProductosModificadoresGrupos",
                column: "GrupoModificadoresId",
                principalTable: "GruposModificadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosModificadoresGrupos_ProductosVenta_ProductoId",
                table: "ProductosModificadoresGrupos",
                column: "ProductoId",
                principalTable: "ProductosVenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_Impuestos_ImpuestoId",
                table: "ProductosVenta",
                column: "ImpuestoId",
                principalTable: "Impuestos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_RutasImpresora_RutaImpresoraId",
                table: "ProductosVenta",
                column: "RutaImpresoraId",
                principalTable: "RutasImpresora",
                principalColumn: "Id");
        }
    }
}
