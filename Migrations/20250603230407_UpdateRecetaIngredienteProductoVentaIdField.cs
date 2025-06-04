using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecetaIngredienteProductoVentaIdField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecetasIngredientes_ProductosVenta_ProductoCompuestoId",
                table: "RecetasIngredientes");

            migrationBuilder.DropForeignKey(
                name: "FK_RecetasIngredientes_ProductosVenta_ProductoVentaId",
                table: "RecetasIngredientes");

            migrationBuilder.DropIndex(
                name: "IX_RecetasIngredientes_ProductoCompuestoId",
                table: "RecetasIngredientes");

            migrationBuilder.DropColumn(
                name: "ProductoCompuestoId",
                table: "RecetasIngredientes");

            migrationBuilder.AlterColumn<int>(
                name: "ProductoVentaId",
                table: "RecetasIngredientes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductoVentaId1",
                table: "RecetasIngredientes",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 4, 5, 456, DateTimeKind.Utc).AddTicks(9956));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 4, 5, 456, DateTimeKind.Utc).AddTicks(9961));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 4, 5, 456, DateTimeKind.Utc).AddTicks(9974));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 4, 5, 456, DateTimeKind.Utc).AddTicks(9978));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 4, 5, 456, DateTimeKind.Utc).AddTicks(9984));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 4, 5, 456, DateTimeKind.Utc).AddTicks(9987));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 23, 4, 5, 457, DateTimeKind.Utc).AddTicks(3826), new DateTime(2025, 6, 3, 23, 4, 5, 457, DateTimeKind.Utc).AddTicks(3827) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 23, 4, 5, 457, DateTimeKind.Utc).AddTicks(3830), new DateTime(2025, 6, 3, 23, 4, 5, 457, DateTimeKind.Utc).AddTicks(3831) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 4, 5, 460, DateTimeKind.Utc).AddTicks(2184));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 4, 5, 460, DateTimeKind.Utc).AddTicks(2194));

            migrationBuilder.CreateIndex(
                name: "IX_RecetasIngredientes_ProductoVentaId1",
                table: "RecetasIngredientes",
                column: "ProductoVentaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RecetasIngredientes_ProductosVenta_ProductoVentaId",
                table: "RecetasIngredientes",
                column: "ProductoVentaId",
                principalTable: "ProductosVenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecetasIngredientes_ProductosVenta_ProductoVentaId1",
                table: "RecetasIngredientes",
                column: "ProductoVentaId1",
                principalTable: "ProductosVenta",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecetasIngredientes_ProductosVenta_ProductoVentaId",
                table: "RecetasIngredientes");

            migrationBuilder.DropForeignKey(
                name: "FK_RecetasIngredientes_ProductosVenta_ProductoVentaId1",
                table: "RecetasIngredientes");

            migrationBuilder.DropIndex(
                name: "IX_RecetasIngredientes_ProductoVentaId1",
                table: "RecetasIngredientes");

            migrationBuilder.DropColumn(
                name: "ProductoVentaId1",
                table: "RecetasIngredientes");

            migrationBuilder.AlterColumn<int>(
                name: "ProductoVentaId",
                table: "RecetasIngredientes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "ProductoCompuestoId",
                table: "RecetasIngredientes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 22, 25, 50, 935, DateTimeKind.Utc).AddTicks(5561));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 22, 25, 50, 935, DateTimeKind.Utc).AddTicks(5564));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 22, 25, 50, 935, DateTimeKind.Utc).AddTicks(5566));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 22, 25, 50, 935, DateTimeKind.Utc).AddTicks(5582));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 22, 25, 50, 935, DateTimeKind.Utc).AddTicks(5584));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 22, 25, 50, 935, DateTimeKind.Utc).AddTicks(5597));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 22, 25, 50, 935, DateTimeKind.Utc).AddTicks(7289), new DateTime(2025, 6, 3, 22, 25, 50, 935, DateTimeKind.Utc).AddTicks(7290) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 22, 25, 50, 935, DateTimeKind.Utc).AddTicks(7293), new DateTime(2025, 6, 3, 22, 25, 50, 935, DateTimeKind.Utc).AddTicks(7293) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 22, 25, 50, 939, DateTimeKind.Utc).AddTicks(5713));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 22, 25, 50, 939, DateTimeKind.Utc).AddTicks(5722));

            migrationBuilder.CreateIndex(
                name: "IX_RecetasIngredientes_ProductoCompuestoId",
                table: "RecetasIngredientes",
                column: "ProductoCompuestoId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecetasIngredientes_ProductosVenta_ProductoCompuestoId",
                table: "RecetasIngredientes",
                column: "ProductoCompuestoId",
                principalTable: "ProductosVenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecetasIngredientes_ProductosVenta_ProductoVentaId",
                table: "RecetasIngredientes",
                column: "ProductoVentaId",
                principalTable: "ProductosVenta",
                principalColumn: "Id");
        }
    }
}
