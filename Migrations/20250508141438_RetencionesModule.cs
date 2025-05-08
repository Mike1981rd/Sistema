using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class RetencionesModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuentasContables_Empresas_EmpresaId1",
                table: "CuentasContables");

            migrationBuilder.DropIndex(
                name: "IX_CuentasContables_EmpresaId1",
                table: "CuentasContables");

            migrationBuilder.DropColumn(
                name: "EmpresaId1",
                table: "CuentasContables");

            migrationBuilder.CreateTable(
                name: "Retenciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Porcentaje = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CuentaContableVentas = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CuentaContableCompras = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CuentaContableRetencionesAsumidas = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Retenciones", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 14, 37, 321, DateTimeKind.Utc).AddTicks(8999));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 14, 37, 321, DateTimeKind.Utc).AddTicks(9002));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 14, 37, 321, DateTimeKind.Utc).AddTicks(9004));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 14, 37, 321, DateTimeKind.Utc).AddTicks(9006));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 14, 37, 321, DateTimeKind.Utc).AddTicks(9072));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 14, 14, 37, 321, DateTimeKind.Utc).AddTicks(9074));

            migrationBuilder.InsertData(
                table: "Retenciones",
                columns: new[] { "Id", "Activo", "CuentaContableCompras", "CuentaContableRetencionesAsumidas", "CuentaContableVentas", "Descripcion", "FechaCreacion", "FechaModificacion", "Nombre", "Porcentaje", "Tipo" },
                values: new object[,]
                {
                    { 1, true, null, null, null, "Impuesto Sobre la Renta al 10%", new DateTime(2025, 5, 8, 14, 14, 37, 322, DateTimeKind.Utc).AddTicks(1963), new DateTime(2025, 5, 8, 14, 14, 37, 322, DateTimeKind.Utc).AddTicks(1965), "ISR 10%", 10.00m, "ISR" },
                    { 2, true, null, null, null, "Retención del IVA al 15%", new DateTime(2025, 5, 8, 14, 14, 37, 322, DateTimeKind.Utc).AddTicks(1969), new DateTime(2025, 5, 8, 14, 14, 37, 322, DateTimeKind.Utc).AddTicks(1970), "IVA Retenido 15%", 15.00m, "IVA" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Retenciones");

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId1",
                table: "CuentasContables",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 12, 47, 40, 134, DateTimeKind.Utc).AddTicks(6716));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 12, 47, 40, 134, DateTimeKind.Utc).AddTicks(6719));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 12, 47, 40, 134, DateTimeKind.Utc).AddTicks(6721));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 12, 47, 40, 134, DateTimeKind.Utc).AddTicks(6722));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 12, 47, 40, 134, DateTimeKind.Utc).AddTicks(6724));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 12, 47, 40, 134, DateTimeKind.Utc).AddTicks(6726));

            migrationBuilder.CreateIndex(
                name: "IX_CuentasContables_EmpresaId1",
                table: "CuentasContables",
                column: "EmpresaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CuentasContables_Empresas_EmpresaId1",
                table: "CuentasContables",
                column: "EmpresaId1",
                principalTable: "Empresas",
                principalColumn: "Id");
        }
    }
}
