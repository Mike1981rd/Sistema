using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class AgregarModuloRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // COMENTADO: Las columnas Descripcion y Estado ya existen en PlazosPago
            // migrationBuilder.AddColumn<string>(
            //     name: "Descripcion",
            //     table: "PlazosPago",
            //     type: "character varying(255)",
            //     maxLength: 255,
            //     nullable: true);

            // migrationBuilder.AddColumn<bool>(
            //     name: "Estado",
            //     table: "PlazosPago",
            //     type: "boolean",
            //     nullable: false,
            //     defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    Prioridad = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Permisos = table.Column<List<string>>(type: "jsonb", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // COMENTADO: No actualizar columnas que ya existen
            // migrationBuilder.UpdateData(
            //     table: "PlazosPago",
            //     keyColumn: "Id",
            //     keyValue: 1,
            //     columns: new[] { "Descripcion", "Estado", "FechaCreacion" },
            //     values: new object[] { null, true, new DateTime(2025, 5, 31, 20, 48, 28, 831, DateTimeKind.Utc).AddTicks(1935) });

            // migrationBuilder.UpdateData(
            //     table: "PlazosPago",
            //     keyColumn: "Id",
            //     keyValue: 2,
            //     columns: new[] { "Descripcion", "Estado", "FechaCreacion" },
            //     values: new object[] { null, true, new DateTime(2025, 5, 31, 20, 48, 28, 831, DateTimeKind.Utc).AddTicks(1937) });

            // migrationBuilder.UpdateData(
            //     table: "PlazosPago",
            //     keyColumn: "Id",
            //     keyValue: 3,
            //     columns: new[] { "Descripcion", "Estado", "FechaCreacion" },
            //     values: new object[] { null, true, new DateTime(2025, 5, 31, 20, 48, 28, 831, DateTimeKind.Utc).AddTicks(1939) });

            // migrationBuilder.UpdateData(
            //     table: "PlazosPago",
            //     keyColumn: "Id",
            //     keyValue: 4,
            //     columns: new[] { "Descripcion", "Estado", "FechaCreacion" },
            //     values: new object[] { null, true, new DateTime(2025, 5, 31, 20, 48, 28, 831, DateTimeKind.Utc).AddTicks(1952) });

            // migrationBuilder.UpdateData(
            //     table: "PlazosPago",
            //     keyColumn: "Id",
            //     keyValue: 5,
            //     columns: new[] { "Descripcion", "Estado", "FechaCreacion" },
            //     values: new object[] { null, true, new DateTime(2025, 5, 31, 20, 48, 28, 831, DateTimeKind.Utc).AddTicks(1954) });

            // migrationBuilder.UpdateData(
            //     table: "PlazosPago",
            //     keyColumn: "Id",
            //     keyValue: 6,
            //     columns: new[] { "Descripcion", "Estado", "FechaCreacion" },
            //     values: new object[] { null, true, new DateTime(2025, 5, 31, 20, 48, 28, 831, DateTimeKind.Utc).AddTicks(1956) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 31, 20, 48, 28, 831, DateTimeKind.Utc).AddTicks(3386), new DateTime(2025, 5, 31, 20, 48, 28, 831, DateTimeKind.Utc).AddTicks(3387) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 31, 20, 48, 28, 831, DateTimeKind.Utc).AddTicks(3389), new DateTime(2025, 5, 31, 20, 48, 28, 831, DateTimeKind.Utc).AddTicks(3390) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 31, 20, 48, 28, 836, DateTimeKind.Utc).AddTicks(1039));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 31, 20, 48, 28, 836, DateTimeKind.Utc).AddTicks(1050));

            migrationBuilder.CreateIndex(
                name: "IX_Roles_EmpresaId",
                table: "Roles",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Nombre_EmpresaId",
                table: "Roles",
                columns: new[] { "Nombre", "EmpresaId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");

            // COMENTADO: No eliminar columnas que ya existían antes de esta migración
            // migrationBuilder.DropColumn(
            //     name: "Descripcion",
            //     table: "PlazosPago");

            // migrationBuilder.DropColumn(
            //     name: "Estado",
            //     table: "PlazosPago");

            // COMENTADO: No actualizar PlazosPago en Down()
            // migrationBuilder.UpdateData(
            //     table: "PlazosPago",
            //     keyColumn: "Id",
            //     keyValue: 1,
            //     column: "FechaCreacion",
            //     value: new DateTime(2025, 5, 21, 13, 2, 21, 64, DateTimeKind.Utc).AddTicks(8562));

            // migrationBuilder.UpdateData(
            //     table: "PlazosPago",
            //     keyColumn: "Id",
            //     keyValue: 2,
            //     column: "FechaCreacion",
            //     value: new DateTime(2025, 5, 21, 13, 2, 21, 64, DateTimeKind.Utc).AddTicks(8565));

            // migrationBuilder.UpdateData(
            //     table: "PlazosPago",
            //     keyColumn: "Id",
            //     keyValue: 3,
            //     column: "FechaCreacion",
            //     value: new DateTime(2025, 5, 21, 13, 2, 21, 64, DateTimeKind.Utc).AddTicks(8568));

            // migrationBuilder.UpdateData(
            //     table: "PlazosPago",
            //     keyColumn: "Id",
            //     keyValue: 4,
            //     column: "FechaCreacion",
            //     value: new DateTime(2025, 5, 21, 13, 2, 21, 64, DateTimeKind.Utc).AddTicks(8590));

            // migrationBuilder.UpdateData(
            //     table: "PlazosPago",
            //     keyColumn: "Id",
            //     keyValue: 5,
            //     column: "FechaCreacion",
            //     value: new DateTime(2025, 5, 21, 13, 2, 21, 64, DateTimeKind.Utc).AddTicks(8591));

            // migrationBuilder.UpdateData(
            //     table: "PlazosPago",
            //     keyColumn: "Id",
            //     keyValue: 6,
            //     column: "FechaCreacion",
            //     value: new DateTime(2025, 5, 21, 13, 2, 21, 64, DateTimeKind.Utc).AddTicks(8593));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 21, 13, 2, 21, 65, DateTimeKind.Utc).AddTicks(2375), new DateTime(2025, 5, 21, 13, 2, 21, 65, DateTimeKind.Utc).AddTicks(2378) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 21, 13, 2, 21, 65, DateTimeKind.Utc).AddTicks(2381), new DateTime(2025, 5, 21, 13, 2, 21, 65, DateTimeKind.Utc).AddTicks(2382) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 21, 13, 2, 21, 67, DateTimeKind.Utc).AddTicks(6255));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 21, 13, 2, 21, 67, DateTimeKind.Utc).AddTicks(6262));
        }
    }
}
