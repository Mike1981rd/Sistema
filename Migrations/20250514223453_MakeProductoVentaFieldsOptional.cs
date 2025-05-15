using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class MakeProductoVentaFieldsOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_UnidadesMedida_UnidadMedidaInventarioId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_ItemContenedores_ItemContenedorId",
                table: "ProductosVenta");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "ProductosVenta",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "ItemContenedorId",
                table: "ProductosVenta",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "UnidadMedidaInventarioId",
                table: "Items",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(2530));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(2533));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(2535));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(2545));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(2547));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(2549));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(5403), new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(5405) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(5410), new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(5411) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 385, DateTimeKind.Utc).AddTicks(8799));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 385, DateTimeKind.Utc).AddTicks(8809));

            migrationBuilder.AddForeignKey(
                name: "FK_Items_UnidadesMedida_UnidadMedidaInventarioId",
                table: "Items",
                column: "UnidadMedidaInventarioId",
                principalTable: "UnidadesMedida",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_ItemContenedores_ItemContenedorId",
                table: "ProductosVenta",
                column: "ItemContenedorId",
                principalTable: "ItemContenedores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_UnidadesMedida_UnidadMedidaInventarioId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_ItemContenedores_ItemContenedorId",
                table: "ProductosVenta");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "ProductosVenta",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ItemContenedorId",
                table: "ProductosVenta",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnidadMedidaInventarioId",
                table: "Items",
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
                value: new DateTime(2025, 5, 14, 2, 35, 30, 187, DateTimeKind.Utc).AddTicks(9207));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 2, 35, 30, 187, DateTimeKind.Utc).AddTicks(9210));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 2, 35, 30, 187, DateTimeKind.Utc).AddTicks(9212));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 2, 35, 30, 187, DateTimeKind.Utc).AddTicks(9220));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 2, 35, 30, 187, DateTimeKind.Utc).AddTicks(9221));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 2, 35, 30, 187, DateTimeKind.Utc).AddTicks(9224));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 14, 2, 35, 30, 188, DateTimeKind.Utc).AddTicks(611), new DateTime(2025, 5, 14, 2, 35, 30, 188, DateTimeKind.Utc).AddTicks(612) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 14, 2, 35, 30, 188, DateTimeKind.Utc).AddTicks(615), new DateTime(2025, 5, 14, 2, 35, 30, 188, DateTimeKind.Utc).AddTicks(615) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 2, 35, 30, 190, DateTimeKind.Utc).AddTicks(1827));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 2, 35, 30, 190, DateTimeKind.Utc).AddTicks(1834));

            migrationBuilder.AddForeignKey(
                name: "FK_Items_UnidadesMedida_UnidadMedidaInventarioId",
                table: "Items",
                column: "UnidadMedidaInventarioId",
                principalTable: "UnidadesMedida",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_ItemContenedores_ItemContenedorId",
                table: "ProductosVenta",
                column: "ItemContenedorId",
                principalTable: "ItemContenedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
