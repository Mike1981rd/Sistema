using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class AddMultipleProductPricingSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductoVentaPrecios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoVentaId = table.Column<int>(type: "integer", nullable: false),
                    NombreNivel = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PrecioBase = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    PrecioTotal = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    EsPrincipal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ListaPrecioId = table.Column<int>(type: "integer", nullable: true),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoVentaPrecios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductoVentaPrecios_ListasPrecios_ListaPrecioId",
                        column: x => x.ListaPrecioId,
                        principalTable: "ListasPrecios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProductoVentaPrecios_ProductosVenta_ProductoVentaId",
                        column: x => x.ProductoVentaId,
                        principalTable: "ProductosVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductoVentaPrecioImpuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoVentaPrecioId = table.Column<int>(type: "integer", nullable: false),
                    ImpuestoId = table.Column<int>(type: "integer", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    PorcentajeOverride = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: true),
                    Notas = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoVentaPrecioImpuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductoVentaPrecioImpuestos_Impuestos_ImpuestoId",
                        column: x => x.ImpuestoId,
                        principalTable: "Impuestos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductoVentaPrecioImpuestos_ProductoVentaPrecios_ProductoV~",
                        column: x => x.ProductoVentaPrecioId,
                        principalTable: "ProductoVentaPrecios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 4, 2, 51, 5, 185, DateTimeKind.Utc).AddTicks(3123));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 4, 2, 51, 5, 185, DateTimeKind.Utc).AddTicks(3126));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 4, 2, 51, 5, 185, DateTimeKind.Utc).AddTicks(3128));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 4, 2, 51, 5, 185, DateTimeKind.Utc).AddTicks(3137));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 4, 2, 51, 5, 185, DateTimeKind.Utc).AddTicks(3139));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 4, 2, 51, 5, 185, DateTimeKind.Utc).AddTicks(3140));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 4, 2, 51, 5, 185, DateTimeKind.Utc).AddTicks(4992), new DateTime(2025, 6, 4, 2, 51, 5, 185, DateTimeKind.Utc).AddTicks(4993) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 4, 2, 51, 5, 185, DateTimeKind.Utc).AddTicks(4996), new DateTime(2025, 6, 4, 2, 51, 5, 185, DateTimeKind.Utc).AddTicks(4997) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 4, 2, 51, 5, 187, DateTimeKind.Utc).AddTicks(9538));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 4, 2, 51, 5, 187, DateTimeKind.Utc).AddTicks(9545));

            migrationBuilder.CreateIndex(
                name: "IX_ProductoVentaPrecioImpuestos_ImpuestoId",
                table: "ProductoVentaPrecioImpuestos",
                column: "ImpuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoVentaPrecioImpuestos_ProductoVentaPrecioId_Impuesto~",
                table: "ProductoVentaPrecioImpuestos",
                columns: new[] { "ProductoVentaPrecioId", "ImpuestoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductoVentaPrecios_ListaPrecioId",
                table: "ProductoVentaPrecios",
                column: "ListaPrecioId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoVentaPrecios_ProductoVentaId_EsPrincipal",
                table: "ProductoVentaPrecios",
                columns: new[] { "ProductoVentaId", "EsPrincipal" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductoVentaPrecios_ProductoVentaId_Orden",
                table: "ProductoVentaPrecios",
                columns: new[] { "ProductoVentaId", "Orden" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductoVentaPrecioImpuestos");

            migrationBuilder.DropTable(
                name: "ProductoVentaPrecios");

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(1105));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(1107));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(1109));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(1119));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(1120));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(1123));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(2820), new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(2820) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(2823), new DateTime(2025, 6, 3, 23, 20, 25, 917, DateTimeKind.Utc).AddTicks(2823) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 920, DateTimeKind.Utc).AddTicks(7254));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 6, 3, 23, 20, 25, 920, DateTimeKind.Utc).AddTicks(7265));
        }
    }
}
