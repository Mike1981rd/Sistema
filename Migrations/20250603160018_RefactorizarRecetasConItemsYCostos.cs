using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class RefactorizarRecetasConItemsYCostos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecetasIngredientes_ProductosVenta_IngredienteProductoId",
                table: "RecetasIngredientes");

            migrationBuilder.DropColumn(
                name: "UnidadMedida",
                table: "RecetasIngredientes");

            migrationBuilder.RenameColumn(
                name: "IngredienteProductoId",
                table: "RecetasIngredientes",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_RecetasIngredientes_IngredienteProductoId",
                table: "RecetasIngredientes",
                newName: "IX_RecetasIngredientes_ItemId");

            migrationBuilder.AddColumn<decimal>(
                name: "CostoTotal",
                table: "RecetasIngredientes",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoUnitario",
                table: "RecetasIngredientes",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ItemContenedorId",
                table: "RecetasIngredientes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductoVentaId",
                table: "RecetasIngredientes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoTotalReceta",
                table: "ProductosVenta",
                type: "numeric(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MargenGananciaReceta",
                table: "ProductosVenta",
                type: "numeric(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotasReceta",
                table: "ProductosVenta",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 0, 16, 296, DateTimeKind.Utc).AddTicks(382));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 0, 16, 296, DateTimeKind.Utc).AddTicks(384));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 0, 16, 296, DateTimeKind.Utc).AddTicks(386));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 0, 16, 296, DateTimeKind.Utc).AddTicks(401));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 0, 16, 296, DateTimeKind.Utc).AddTicks(403));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 0, 16, 296, DateTimeKind.Utc).AddTicks(405));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 16, 0, 16, 296, DateTimeKind.Utc).AddTicks(2037), new DateTime(2025, 6, 3, 16, 0, 16, 296, DateTimeKind.Utc).AddTicks(2040) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 16, 0, 16, 296, DateTimeKind.Utc).AddTicks(2043), new DateTime(2025, 6, 3, 16, 0, 16, 296, DateTimeKind.Utc).AddTicks(2044) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 0, 16, 298, DateTimeKind.Utc).AddTicks(9208));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 0, 16, 298, DateTimeKind.Utc).AddTicks(9215));

            migrationBuilder.CreateIndex(
                name: "IX_RecetasIngredientes_ItemContenedorId",
                table: "RecetasIngredientes",
                column: "ItemContenedorId");

            migrationBuilder.CreateIndex(
                name: "IX_RecetasIngredientes_ProductoVentaId",
                table: "RecetasIngredientes",
                column: "ProductoVentaId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecetasIngredientes_ItemContenedores_ItemContenedorId",
                table: "RecetasIngredientes",
                column: "ItemContenedorId",
                principalTable: "ItemContenedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecetasIngredientes_Items_ItemId",
                table: "RecetasIngredientes",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecetasIngredientes_ProductosVenta_ProductoVentaId",
                table: "RecetasIngredientes",
                column: "ProductoVentaId",
                principalTable: "ProductosVenta",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecetasIngredientes_ItemContenedores_ItemContenedorId",
                table: "RecetasIngredientes");

            migrationBuilder.DropForeignKey(
                name: "FK_RecetasIngredientes_Items_ItemId",
                table: "RecetasIngredientes");

            migrationBuilder.DropForeignKey(
                name: "FK_RecetasIngredientes_ProductosVenta_ProductoVentaId",
                table: "RecetasIngredientes");

            migrationBuilder.DropIndex(
                name: "IX_RecetasIngredientes_ItemContenedorId",
                table: "RecetasIngredientes");

            migrationBuilder.DropIndex(
                name: "IX_RecetasIngredientes_ProductoVentaId",
                table: "RecetasIngredientes");

            migrationBuilder.DropColumn(
                name: "CostoTotal",
                table: "RecetasIngredientes");

            migrationBuilder.DropColumn(
                name: "CostoUnitario",
                table: "RecetasIngredientes");

            migrationBuilder.DropColumn(
                name: "ItemContenedorId",
                table: "RecetasIngredientes");

            migrationBuilder.DropColumn(
                name: "ProductoVentaId",
                table: "RecetasIngredientes");

            migrationBuilder.DropColumn(
                name: "CostoTotalReceta",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "MargenGananciaReceta",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "NotasReceta",
                table: "ProductosVenta");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "RecetasIngredientes",
                newName: "IngredienteProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_RecetasIngredientes_ItemId",
                table: "RecetasIngredientes",
                newName: "IX_RecetasIngredientes_IngredienteProductoId");

            migrationBuilder.AddColumn<string>(
                name: "UnidadMedida",
                table: "RecetasIngredientes",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 1, 15, 47, 45, 150, DateTimeKind.Utc).AddTicks(4887));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 1, 15, 47, 45, 150, DateTimeKind.Utc).AddTicks(4892));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 1, 15, 47, 45, 150, DateTimeKind.Utc).AddTicks(4896));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 1, 15, 47, 45, 150, DateTimeKind.Utc).AddTicks(4920));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 1, 15, 47, 45, 150, DateTimeKind.Utc).AddTicks(4922));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 1, 15, 47, 45, 150, DateTimeKind.Utc).AddTicks(4925));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 1, 15, 47, 45, 150, DateTimeKind.Utc).AddTicks(8114), new DateTime(2025, 6, 1, 15, 47, 45, 150, DateTimeKind.Utc).AddTicks(8116) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 1, 15, 47, 45, 150, DateTimeKind.Utc).AddTicks(8120), new DateTime(2025, 6, 1, 15, 47, 45, 150, DateTimeKind.Utc).AddTicks(8121) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 1, 15, 47, 45, 153, DateTimeKind.Utc).AddTicks(8229));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 1, 15, 47, 45, 153, DateTimeKind.Utc).AddTicks(8239));

            migrationBuilder.AddForeignKey(
                name: "FK_RecetasIngredientes_ProductosVenta_IngredienteProductoId",
                table: "RecetasIngredientes",
                column: "IngredienteProductoId",
                principalTable: "ProductosVenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
