using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCuentasContablesAProductoVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CuentaVentasId",
                table: "ProductosVenta",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaComprasInventariosId",
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
                name: "CuentaAjustesId",
                table: "ProductosVenta",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaCostoMateriaPrimaId",
                table: "ProductosVenta",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_CuentaVentasId",
                table: "ProductosVenta",
                column: "CuentaVentasId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_CuentaComprasInventariosId",
                table: "ProductosVenta",
                column: "CuentaComprasInventariosId");

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
                name: "IX_ProductosVenta_CuentaAjustesId",
                table: "ProductosVenta",
                column: "CuentaAjustesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_CuentaCostoMateriaPrimaId",
                table: "ProductosVenta",
                column: "CuentaCostoMateriaPrimaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaVentasId",
                table: "ProductosVenta",
                column: "CuentaVentasId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaComprasInventariosId",
                table: "ProductosVenta",
                column: "CuentaComprasInventariosId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaCostoVentasGastosId",
                table: "ProductosVenta",
                column: "CuentaCostoVentasGastosId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaDescuentosId",
                table: "ProductosVenta",
                column: "CuentaDescuentosId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaDevolucionesId",
                table: "ProductosVenta",
                column: "CuentaDevolucionesId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaAjustesId",
                table: "ProductosVenta",
                column: "CuentaAjustesId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaCostoMateriaPrimaId",
                table: "ProductosVenta",
                column: "CuentaCostoMateriaPrimaId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaVentasId",
                table: "ProductosVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaComprasInventariosId",
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
                name: "FK_ProductosVenta_CuentasContables_CuentaAjustesId",
                table: "ProductosVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_CuentasContables_CuentaCostoMateriaPrimaId",
                table: "ProductosVenta");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_CuentaVentasId",
                table: "ProductosVenta");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_CuentaComprasInventariosId",
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
                name: "IX_ProductosVenta_CuentaAjustesId",
                table: "ProductosVenta");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_CuentaCostoMateriaPrimaId",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "CuentaVentasId",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "CuentaComprasInventariosId",
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
                name: "CuentaAjustesId",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "CuentaCostoMateriaPrimaId",
                table: "ProductosVenta");
        }
    }
}