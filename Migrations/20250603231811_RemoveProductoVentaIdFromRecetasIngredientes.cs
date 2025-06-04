using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductoVentaIdFromRecetasIngredientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                value: new DateTime(2025, 6, 3, 23, 18, 9, 776, DateTimeKind.Utc).AddTicks(8435));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 776, DateTimeKind.Utc).AddTicks(8438));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 776, DateTimeKind.Utc).AddTicks(8440));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 776, DateTimeKind.Utc).AddTicks(8449));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 776, DateTimeKind.Utc).AddTicks(8451));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 776, DateTimeKind.Utc).AddTicks(8453));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 23, 18, 9, 777, DateTimeKind.Utc).AddTicks(313), new DateTime(2025, 6, 3, 23, 18, 9, 777, DateTimeKind.Utc).AddTicks(314) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 23, 18, 9, 777, DateTimeKind.Utc).AddTicks(317), new DateTime(2025, 6, 3, 23, 18, 9, 777, DateTimeKind.Utc).AddTicks(318) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 779, DateTimeKind.Utc).AddTicks(4156));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 18, 9, 779, DateTimeKind.Utc).AddTicks(4163));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
