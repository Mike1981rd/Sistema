using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class CreateClientesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsPredeterminado",
                table: "PlazosPago",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ListasPrecios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Porcentaje = table.Column<decimal>(type: "numeric", nullable: true),
                    EsPredeterminada = table.Column<bool>(type: "boolean", nullable: false),
                    Activa = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListasPrecios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provincias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provincias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposIdentificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposIdentificacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposNcf",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposNcf", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PorcentajeComision = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Municipios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProvinciaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipios_Provincias_ProvinciaId",
                        column: x => x.ProvinciaId,
                        principalTable: "Provincias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreRazonSocial = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TipoIdentificacionId = table.Column<int>(type: "integer", nullable: false),
                    NumeroIdentificacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MunicipioId = table.Column<int>(type: "integer", nullable: true),
                    Direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Celular = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TipoNcfId = table.Column<int>(type: "integer", nullable: true),
                    PlazoPagoId = table.Column<int>(type: "integer", nullable: true),
                    ListaPrecioId = table.Column<int>(type: "integer", nullable: true),
                    VendedorId = table.Column<int>(type: "integer", nullable: true),
                    LimiteCredito = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    EsCliente = table.Column<bool>(type: "boolean", nullable: false),
                    EsProveedor = table.Column<bool>(type: "boolean", nullable: false),
                    ImagenUrl = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CuentaPorCobrarId = table.Column<int>(type: "integer", nullable: true),
                    CuentaPorPagarId = table.Column<int>(type: "integer", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_CuentasContables_CuentaPorCobrarId",
                        column: x => x.CuentaPorCobrarId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Clientes_CuentasContables_CuentaPorPagarId",
                        column: x => x.CuentaPorPagarId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Clientes_ListasPrecios_ListaPrecioId",
                        column: x => x.ListaPrecioId,
                        principalTable: "ListasPrecios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Clientes_Municipios_MunicipioId",
                        column: x => x.MunicipioId,
                        principalTable: "Municipios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Clientes_PlazosPago_PlazoPagoId",
                        column: x => x.PlazoPagoId,
                        principalTable: "PlazosPago",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Clientes_TiposIdentificacion_TipoIdentificacionId",
                        column: x => x.TipoIdentificacionId,
                        principalTable: "TiposIdentificacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clientes_TiposNcf_TipoNcfId",
                        column: x => x.TipoNcfId,
                        principalTable: "TiposNcf",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Clientes_Vendedores_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Vendedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EsPredeterminado", "FechaCreacion" },
                values: new object[] { false, new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(4890) });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EsPredeterminado", "FechaCreacion" },
                values: new object[] { false, new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(4893) });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EsPredeterminado", "FechaCreacion" },
                values: new object[] { false, new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(4895) });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EsPredeterminado", "FechaCreacion" },
                values: new object[] { false, new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(4897) });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EsPredeterminado", "FechaCreacion" },
                values: new object[] { false, new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(4899) });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EsPredeterminado", "FechaCreacion" },
                values: new object[] { false, new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(4900) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(5901), new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(5902) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(5904), new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(5905) });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CuentaPorCobrarId",
                table: "Clientes",
                column: "CuentaPorCobrarId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CuentaPorPagarId",
                table: "Clientes",
                column: "CuentaPorPagarId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_ListaPrecioId",
                table: "Clientes",
                column: "ListaPrecioId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_MunicipioId",
                table: "Clientes",
                column: "MunicipioId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_PlazoPagoId",
                table: "Clientes",
                column: "PlazoPagoId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_TipoIdentificacionId",
                table: "Clientes",
                column: "TipoIdentificacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_TipoNcfId",
                table: "Clientes",
                column: "TipoNcfId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_VendedorId",
                table: "Clientes",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_ProvinciaId",
                table: "Municipios",
                column: "ProvinciaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "ListasPrecios");

            migrationBuilder.DropTable(
                name: "Municipios");

            migrationBuilder.DropTable(
                name: "TiposIdentificacion");

            migrationBuilder.DropTable(
                name: "TiposNcf");

            migrationBuilder.DropTable(
                name: "Vendedores");

            migrationBuilder.DropTable(
                name: "Provincias");

            migrationBuilder.DropColumn(
                name: "EsPredeterminado",
                table: "PlazosPago");

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 15, 7, 23, 24, DateTimeKind.Utc).AddTicks(2671));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 15, 7, 23, 24, DateTimeKind.Utc).AddTicks(2674));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 15, 7, 23, 24, DateTimeKind.Utc).AddTicks(2676));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 15, 7, 23, 24, DateTimeKind.Utc).AddTicks(2677));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 15, 7, 23, 24, DateTimeKind.Utc).AddTicks(2679));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 15, 7, 23, 24, DateTimeKind.Utc).AddTicks(2681));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 15, 7, 23, 24, DateTimeKind.Utc).AddTicks(3795), new DateTime(2025, 5, 8, 15, 7, 23, 24, DateTimeKind.Utc).AddTicks(3796) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 15, 7, 23, 24, DateTimeKind.Utc).AddTicks(3798), new DateTime(2025, 5, 8, 15, 7, 23, 24, DateTimeKind.Utc).AddTicks(3798) });
        }
    }
}
