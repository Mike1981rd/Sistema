using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class AgregarModuloImpuestos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Impuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Porcentaje = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EsAcreditable = table.Column<bool>(type: "boolean", nullable: false),
                    CuentaContableVentasId = table.Column<int>(type: "integer", nullable: false),
                    CuentaContableComprasId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EstaEnUso = table.Column<bool>(type: "boolean", nullable: false),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Impuestos_CuentasContables_CuentaContableComprasId",
                        column: x => x.CuentaContableComprasId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Impuestos_CuentasContables_CuentaContableVentasId",
                        column: x => x.CuentaContableVentasId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Impuestos_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Impuestos_CuentaContableComprasId",
                table: "Impuestos",
                column: "CuentaContableComprasId");

            migrationBuilder.CreateIndex(
                name: "IX_Impuestos_CuentaContableVentasId",
                table: "Impuestos",
                column: "CuentaContableVentasId");

            migrationBuilder.CreateIndex(
                name: "IX_Impuestos_EmpresaId",
                table: "Impuestos",
                column: "EmpresaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Impuestos");
        }
    }
}
