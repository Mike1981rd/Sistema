using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class JournalEntriesModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposEntradaDiario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposEntradaDiario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NumeracionesEntradaDiario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoEntradaDiarioId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Prefijo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    NumeroActual = table.Column<int>(type: "integer", nullable: false),
                    EsPreferida = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumeracionesEntradaDiario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NumeracionesEntradaDiario_TiposEntradaDiario_TipoEntradaDia~",
                        column: x => x.TipoEntradaDiarioId,
                        principalTable: "TiposEntradaDiario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntradasDiario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TipoEntradaId = table.Column<int>(type: "integer", nullable: false),
                    NumeracionId = table.Column<int>(type: "integer", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Estado = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradasDiario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntradasDiario_NumeracionesEntradaDiario_NumeracionId",
                        column: x => x.NumeracionId,
                        principalTable: "NumeracionesEntradaDiario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntradasDiario_TiposEntradaDiario_TipoEntradaId",
                        column: x => x.TipoEntradaId,
                        principalTable: "TiposEntradaDiario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosContables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntradaDiarioId = table.Column<int>(type: "integer", nullable: false),
                    CuentaContableId = table.Column<int>(type: "integer", nullable: false),
                    ContactoId = table.Column<int>(type: "integer", nullable: true),
                    TipoContacto = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    NumeroDocumento = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Debito = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Credito = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosContables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientosContables_Clientes_ContactoId",
                        column: x => x.ContactoId,
                        principalTable: "Clientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovimientosContables_CuentasContables_CuentaContableId",
                        column: x => x.CuentaContableId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovimientosContables_EntradasDiario_EntradaDiarioId",
                        column: x => x.EntradaDiarioId,
                        principalTable: "EntradasDiario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 33, 45, 524, DateTimeKind.Utc).AddTicks(9698));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 33, 45, 524, DateTimeKind.Utc).AddTicks(9702));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 33, 45, 524, DateTimeKind.Utc).AddTicks(9704));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 33, 45, 524, DateTimeKind.Utc).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 33, 45, 524, DateTimeKind.Utc).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 33, 45, 524, DateTimeKind.Utc).AddTicks(9711));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 33, 45, 525, DateTimeKind.Utc).AddTicks(2145), new DateTime(2025, 5, 9, 15, 33, 45, 525, DateTimeKind.Utc).AddTicks(2147) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 33, 45, 525, DateTimeKind.Utc).AddTicks(2152), new DateTime(2025, 5, 9, 15, 33, 45, 525, DateTimeKind.Utc).AddTicks(2153) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 33, 45, 528, DateTimeKind.Utc).AddTicks(8455));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 33, 45, 528, DateTimeKind.Utc).AddTicks(8468));

            migrationBuilder.CreateIndex(
                name: "IX_EntradasDiario_NumeracionId",
                table: "EntradasDiario",
                column: "NumeracionId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradasDiario_TipoEntradaId",
                table: "EntradasDiario",
                column: "TipoEntradaId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosContables_ContactoId",
                table: "MovimientosContables",
                column: "ContactoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosContables_CuentaContableId",
                table: "MovimientosContables",
                column: "CuentaContableId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosContables_EntradaDiarioId",
                table: "MovimientosContables",
                column: "EntradaDiarioId");

            migrationBuilder.CreateIndex(
                name: "IX_NumeracionesEntradaDiario_TipoEntradaDiarioId",
                table: "NumeracionesEntradaDiario",
                column: "TipoEntradaDiarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimientosContables");

            migrationBuilder.DropTable(
                name: "EntradasDiario");

            migrationBuilder.DropTable(
                name: "NumeracionesEntradaDiario");

            migrationBuilder.DropTable(
                name: "TiposEntradaDiario");

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 20, 56, 11, 909, DateTimeKind.Utc).AddTicks(8666));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 20, 56, 11, 909, DateTimeKind.Utc).AddTicks(8669));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 20, 56, 11, 909, DateTimeKind.Utc).AddTicks(8671));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 20, 56, 11, 909, DateTimeKind.Utc).AddTicks(8672));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 20, 56, 11, 909, DateTimeKind.Utc).AddTicks(8674));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 20, 56, 11, 909, DateTimeKind.Utc).AddTicks(8676));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 20, 56, 11, 910, DateTimeKind.Utc).AddTicks(630), new DateTime(2025, 5, 8, 20, 56, 11, 910, DateTimeKind.Utc).AddTicks(632) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 20, 56, 11, 910, DateTimeKind.Utc).AddTicks(634), new DateTime(2025, 5, 8, 20, 56, 11, 910, DateTimeKind.Utc).AddTicks(635) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 20, 56, 11, 916, DateTimeKind.Utc).AddTicks(5612));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 20, 56, 11, 916, DateTimeKind.Utc).AddTicks(5618));
        }
    }
}
