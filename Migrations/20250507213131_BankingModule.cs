using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class BankingModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Contactos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AsientosContables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Concepto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Contabilizado = table.Column<bool>(type: "boolean", nullable: false),
                    FechaContabilizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    MontoTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OrigenDocumento = table.Column<string>(type: "text", nullable: true),
                    OrigenId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioCreacion = table.Column<string>(type: "text", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsientosContables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsientosContables_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bancos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NumeroCuenta = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TipoCuenta = table.Column<int>(type: "integer", nullable: false),
                    EntidadBancaria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Moneda = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SaldoInicial = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SaldoActual = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SaldoConciliado = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    FechaApertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    CuentaContableId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioCreacion = table.Column<string>(type: "text", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bancos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bancos_CuentasContables_CuentaContableId",
                        column: x => x.CuentaContableId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bancos_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesAsientoContable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AsientoContableId = table.Column<int>(type: "integer", nullable: false),
                    CuentaContableId = table.Column<int>(type: "integer", nullable: false),
                    Debe = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Haber = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Concepto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ContactoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesAsientoContable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesAsientoContable_AsientosContables_AsientoContableId",
                        column: x => x.AsientoContableId,
                        principalTable: "AsientosContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesAsientoContable_Contactos_ContactoId",
                        column: x => x.ContactoId,
                        principalTable: "Contactos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DetallesAsientoContable_CuentasContables_CuentaContableId",
                        column: x => x.CuentaContableId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConciliacionesBancarias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BancoId = table.Column<int>(type: "integer", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaConciliacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SaldoSegunLibro = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SaldoSegunBanco = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DiferenciaConciliacion = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Notas = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Estado = table.Column<int>(type: "integer", nullable: false),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioCreacion = table.Column<string>(type: "text", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConciliacionesBancarias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConciliacionesBancarias_Bancos_BancoId",
                        column: x => x.BancoId,
                        principalTable: "Bancos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConciliacionesBancarias_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AjustesConciliacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConciliacionId = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Concepto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Aplicado = table.Column<bool>(type: "boolean", nullable: false),
                    AsientoContableId = table.Column<int>(type: "integer", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AjustesConciliacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AjustesConciliacion_AsientosContables_AsientoContableId",
                        column: x => x.AsientoContableId,
                        principalTable: "AsientosContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AjustesConciliacion_ConciliacionesBancarias_ConciliacionId",
                        column: x => x.ConciliacionId,
                        principalTable: "ConciliacionesBancarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransaccionesBanco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BancoId = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Concepto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Referencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Conciliado = table.Column<bool>(type: "boolean", nullable: false),
                    ConciliacionId = table.Column<int>(type: "integer", nullable: true),
                    ContactoId = table.Column<int>(type: "integer", nullable: true),
                    AsientoContableId = table.Column<int>(type: "integer", nullable: true),
                    BancoDestinoId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioCreacion = table.Column<string>(type: "text", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransaccionesBanco", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransaccionesBanco_AsientosContables_AsientoContableId",
                        column: x => x.AsientoContableId,
                        principalTable: "AsientosContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TransaccionesBanco_Bancos_BancoDestinoId",
                        column: x => x.BancoDestinoId,
                        principalTable: "Bancos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransaccionesBanco_Bancos_BancoId",
                        column: x => x.BancoId,
                        principalTable: "Bancos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransaccionesBanco_ConciliacionesBancarias_ConciliacionId",
                        column: x => x.ConciliacionId,
                        principalTable: "ConciliacionesBancarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransaccionesBanco_Contactos_ContactoId",
                        column: x => x.ContactoId,
                        principalTable: "Contactos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransaccionesBanco_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AjustesConciliacion_AsientoContableId",
                table: "AjustesConciliacion",
                column: "AsientoContableId");

            migrationBuilder.CreateIndex(
                name: "IX_AjustesConciliacion_ConciliacionId",
                table: "AjustesConciliacion",
                column: "ConciliacionId");

            migrationBuilder.CreateIndex(
                name: "IX_AsientosContables_EmpresaId",
                table: "AsientosContables",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Bancos_CuentaContableId",
                table: "Bancos",
                column: "CuentaContableId");

            migrationBuilder.CreateIndex(
                name: "IX_Bancos_EmpresaId",
                table: "Bancos",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConciliacionesBancarias_BancoId",
                table: "ConciliacionesBancarias",
                column: "BancoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConciliacionesBancarias_EmpresaId",
                table: "ConciliacionesBancarias",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesAsientoContable_AsientoContableId",
                table: "DetallesAsientoContable",
                column: "AsientoContableId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesAsientoContable_ContactoId",
                table: "DetallesAsientoContable",
                column: "ContactoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesAsientoContable_CuentaContableId",
                table: "DetallesAsientoContable",
                column: "CuentaContableId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionesBanco_AsientoContableId",
                table: "TransaccionesBanco",
                column: "AsientoContableId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionesBanco_BancoDestinoId",
                table: "TransaccionesBanco",
                column: "BancoDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionesBanco_BancoId",
                table: "TransaccionesBanco",
                column: "BancoId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionesBanco_ConciliacionId",
                table: "TransaccionesBanco",
                column: "ConciliacionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionesBanco_ContactoId",
                table: "TransaccionesBanco",
                column: "ContactoId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionesBanco_EmpresaId",
                table: "TransaccionesBanco",
                column: "EmpresaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AjustesConciliacion");

            migrationBuilder.DropTable(
                name: "DetallesAsientoContable");

            migrationBuilder.DropTable(
                name: "TransaccionesBanco");

            migrationBuilder.DropTable(
                name: "AsientosContables");

            migrationBuilder.DropTable(
                name: "ConciliacionesBancarias");

            migrationBuilder.DropTable(
                name: "Bancos");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Contactos");
        }
    }
}
