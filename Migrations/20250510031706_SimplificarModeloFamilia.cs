using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class SimplificarModeloFamilia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ejecutar SQL personalizado para eliminar la tabla si existe
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'FamiliasCuentasContables') THEN
                        DROP TABLE ""FamiliasCuentasContables"" CASCADE;
                    END IF;
                END
                $$;
            ");

            // Actualizar fechas en tablas de referencia
            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(8320));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(8323));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(8325));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(8326));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(8328));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(8329));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(9823), new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(9824) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(9826), new DateTime(2025, 5, 10, 3, 17, 5, 903, DateTimeKind.Utc).AddTicks(9827) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 906, DateTimeKind.Utc).AddTicks(2873));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 17, 5, 906, DateTimeKind.Utc).AddTicks(2880));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No es necesario restaurar una tabla que ya ha sido eliminada
            // Simplemente actualizamos las fechas de nuevo
            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 9, 12, 94, DateTimeKind.Utc).AddTicks(4202));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 9, 12, 94, DateTimeKind.Utc).AddTicks(4204));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 9, 12, 94, DateTimeKind.Utc).AddTicks(4205));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 9, 12, 94, DateTimeKind.Utc).AddTicks(4207));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 9, 12, 94, DateTimeKind.Utc).AddTicks(4209));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 9, 12, 94, DateTimeKind.Utc).AddTicks(4211));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 3, 9, 12, 94, DateTimeKind.Utc).AddTicks(5558), new DateTime(2025, 5, 10, 3, 9, 12, 94, DateTimeKind.Utc).AddTicks(5559) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 3, 9, 12, 94, DateTimeKind.Utc).AddTicks(5561), new DateTime(2025, 5, 10, 3, 9, 12, 94, DateTimeKind.Utc).AddTicks(5562) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 9, 12, 96, DateTimeKind.Utc).AddTicks(9359));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 3, 9, 12, 96, DateTimeKind.Utc).AddTicks(9367));
        }
    }
}
