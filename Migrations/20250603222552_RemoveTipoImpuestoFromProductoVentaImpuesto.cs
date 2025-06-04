using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTipoImpuestoFromProductoVentaImpuesto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductoVentaImpuestos_ProductoVentaId_ImpuestoId_TipoImpue~",
                table: "ProductoVentaImpuestos");

            migrationBuilder.DropColumn(
                name: "TipoImpuesto",
                table: "ProductoVentaImpuestos");

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
                name: "IX_ProductoVentaImpuestos_ProductoVentaId_ImpuestoId",
                table: "ProductoVentaImpuestos",
                columns: new[] { "ProductoVentaId", "ImpuestoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductoVentaImpuestos_ProductoVentaId_ImpuestoId",
                table: "ProductoVentaImpuestos");

            migrationBuilder.AddColumn<int>(
                name: "TipoImpuesto",
                table: "ProductoVentaImpuestos",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 21, 58, 14, 867, DateTimeKind.Utc).AddTicks(731));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 21, 58, 14, 867, DateTimeKind.Utc).AddTicks(733));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 21, 58, 14, 867, DateTimeKind.Utc).AddTicks(748));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 21, 58, 14, 867, DateTimeKind.Utc).AddTicks(751));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 21, 58, 14, 867, DateTimeKind.Utc).AddTicks(753));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 21, 58, 14, 867, DateTimeKind.Utc).AddTicks(756));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 21, 58, 14, 867, DateTimeKind.Utc).AddTicks(2637), new DateTime(2025, 6, 3, 21, 58, 14, 867, DateTimeKind.Utc).AddTicks(2638) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 21, 58, 14, 867, DateTimeKind.Utc).AddTicks(2641), new DateTime(2025, 6, 3, 21, 58, 14, 867, DateTimeKind.Utc).AddTicks(2642) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 21, 58, 14, 869, DateTimeKind.Utc).AddTicks(9980));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 21, 58, 14, 869, DateTimeKind.Utc).AddTicks(9991));

            migrationBuilder.CreateIndex(
                name: "IX_ProductoVentaImpuestos_ProductoVentaId_ImpuestoId_TipoImpue~",
                table: "ProductoVentaImpuestos",
                columns: new[] { "ProductoVentaId", "ImpuestoId", "TipoImpuesto" },
                unique: true);
        }
    }
}
