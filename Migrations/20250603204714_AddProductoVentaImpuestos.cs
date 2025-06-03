using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class AddProductoVentaImpuestos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductoVentaImpuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoVentaId = table.Column<int>(type: "integer", nullable: false),
                    ImpuestoId = table.Column<int>(type: "integer", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoVentaImpuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductoVentaImpuestos_Impuestos_ImpuestoId",
                        column: x => x.ImpuestoId,
                        principalTable: "Impuestos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductoVentaImpuestos_ProductosVenta_ProductoVentaId",
                        column: x => x.ProductoVentaId,
                        principalTable: "ProductosVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 20, 47, 12, 451, DateTimeKind.Utc).AddTicks(2127));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 20, 47, 12, 451, DateTimeKind.Utc).AddTicks(2129));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 20, 47, 12, 451, DateTimeKind.Utc).AddTicks(2131));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 20, 47, 12, 451, DateTimeKind.Utc).AddTicks(2172));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 20, 47, 12, 451, DateTimeKind.Utc).AddTicks(2174));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 20, 47, 12, 451, DateTimeKind.Utc).AddTicks(2176));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 20, 47, 12, 451, DateTimeKind.Utc).AddTicks(4002), new DateTime(2025, 6, 3, 20, 47, 12, 451, DateTimeKind.Utc).AddTicks(4003) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 20, 47, 12, 451, DateTimeKind.Utc).AddTicks(4006), new DateTime(2025, 6, 3, 20, 47, 12, 451, DateTimeKind.Utc).AddTicks(4007) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 20, 47, 12, 453, DateTimeKind.Utc).AddTicks(7022));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 20, 47, 12, 453, DateTimeKind.Utc).AddTicks(7029));

            migrationBuilder.CreateIndex(
                name: "IX_ProductoVentaImpuestos_ImpuestoId",
                table: "ProductoVentaImpuestos",
                column: "ImpuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoVentaImpuestos_ProductoVentaId_ImpuestoId",
                table: "ProductoVentaImpuestos",
                columns: new[] { "ProductoVentaId", "ImpuestoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductoVentaImpuestos");

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
        }
    }
}
