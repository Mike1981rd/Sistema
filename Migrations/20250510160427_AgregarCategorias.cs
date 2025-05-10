using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Nota = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    FamiliaId = table.Column<int>(type: "integer", nullable: false),
                    CuentaVentasId = table.Column<int>(type: "integer", nullable: true),
                    CuentaComprasInventariosId = table.Column<int>(type: "integer", nullable: true),
                    CuentaCostoVentasGastosId = table.Column<int>(type: "integer", nullable: true),
                    CuentaDescuentosId = table.Column<int>(type: "integer", nullable: true),
                    CuentaDevolucionesId = table.Column<int>(type: "integer", nullable: true),
                    CuentaAjustesId = table.Column<int>(type: "integer", nullable: true),
                    CuentaCostoMateriaPrimaId = table.Column<int>(type: "integer", nullable: true),
                    Impuestos = table.Column<string>(type: "text", nullable: true),
                    Propina = table.Column<decimal>(type: "numeric", nullable: true),
                    CanalesImpresora = table.Column<string>(type: "text", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categorias_CuentasContables_CuentaAjustesId",
                        column: x => x.CuentaAjustesId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categorias_CuentasContables_CuentaComprasInventariosId",
                        column: x => x.CuentaComprasInventariosId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categorias_CuentasContables_CuentaCostoMateriaPrimaId",
                        column: x => x.CuentaCostoMateriaPrimaId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categorias_CuentasContables_CuentaCostoVentasGastosId",
                        column: x => x.CuentaCostoVentasGastosId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categorias_CuentasContables_CuentaDescuentosId",
                        column: x => x.CuentaDescuentosId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categorias_CuentasContables_CuentaDevolucionesId",
                        column: x => x.CuentaDevolucionesId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categorias_CuentasContables_CuentaVentasId",
                        column: x => x.CuentaVentasId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categorias_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Categorias_Familias_FamiliaId",
                        column: x => x.FamiliaId,
                        principalTable: "Familias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(895));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(899));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(901));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(904));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(906));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(909));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(5793), new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(5796) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(5800), new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(5800) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 739, DateTimeKind.Utc).AddTicks(509));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 739, DateTimeKind.Utc).AddTicks(519));

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CuentaAjustesId",
                table: "Categorias",
                column: "CuentaAjustesId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CuentaComprasInventariosId",
                table: "Categorias",
                column: "CuentaComprasInventariosId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CuentaCostoMateriaPrimaId",
                table: "Categorias",
                column: "CuentaCostoMateriaPrimaId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CuentaCostoVentasGastosId",
                table: "Categorias",
                column: "CuentaCostoVentasGastosId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CuentaDescuentosId",
                table: "Categorias",
                column: "CuentaDescuentosId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CuentaDevolucionesId",
                table: "Categorias",
                column: "CuentaDevolucionesId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CuentaVentasId",
                table: "Categorias",
                column: "CuentaVentasId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_EmpresaId",
                table: "Categorias",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_FamiliaId",
                table: "Categorias",
                column: "FamiliaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(4968));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(4971));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(4973));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(4975));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(4976));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(4978));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(6732), new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(6733) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(6736), new DateTime(2025, 5, 10, 14, 2, 50, 761, DateTimeKind.Utc).AddTicks(6736) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 765, DateTimeKind.Utc).AddTicks(3548));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 14, 2, 50, 765, DateTimeKind.Utc).AddTicks(3557));
        }
    }
}
