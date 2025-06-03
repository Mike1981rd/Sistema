using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarProductosRecetasYCamposOpcionales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_Items_ItemId",
                table: "ProductosVenta");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ProductosVenta",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 31, 37, 103, DateTimeKind.Utc).AddTicks(7518));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 31, 37, 103, DateTimeKind.Utc).AddTicks(7521));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 31, 37, 103, DateTimeKind.Utc).AddTicks(7522));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 31, 37, 103, DateTimeKind.Utc).AddTicks(7534));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 31, 37, 103, DateTimeKind.Utc).AddTicks(7536));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 31, 37, 103, DateTimeKind.Utc).AddTicks(7538));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 16, 31, 37, 103, DateTimeKind.Utc).AddTicks(9253), new DateTime(2025, 6, 3, 16, 31, 37, 103, DateTimeKind.Utc).AddTicks(9253) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 16, 31, 37, 103, DateTimeKind.Utc).AddTicks(9256), new DateTime(2025, 6, 3, 16, 31, 37, 103, DateTimeKind.Utc).AddTicks(9257) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 31, 37, 107, DateTimeKind.Utc).AddTicks(2219));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 16, 31, 37, 107, DateTimeKind.Utc).AddTicks(2226));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_Items_ItemId",
                table: "ProductosVenta",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_Items_ItemId",
                table: "ProductosVenta");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ProductosVenta",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_Items_ItemId",
                table: "ProductosVenta",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
