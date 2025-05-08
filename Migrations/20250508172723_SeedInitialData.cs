using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ListasPrecios",
                columns: new[] { "Id", "Activa", "Descripcion", "EsPredeterminada", "Nombre", "Porcentaje" },
                values: new object[,]
                {
                    { 1, true, "Precios regulares", true, "Lista Regular", null },
                    { 2, true, "Precios para mayoristas", false, "Mayoristas", 10m },
                    { 3, true, "Precios para clientes VIP", false, "VIP", 20m }
                });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 27, 22, 903, DateTimeKind.Utc).AddTicks(9185));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 27, 22, 903, DateTimeKind.Utc).AddTicks(9188));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 27, 22, 903, DateTimeKind.Utc).AddTicks(9190));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 27, 22, 903, DateTimeKind.Utc).AddTicks(9191));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 27, 22, 903, DateTimeKind.Utc).AddTicks(9193));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 27, 22, 903, DateTimeKind.Utc).AddTicks(9195));

            migrationBuilder.InsertData(
                table: "Provincias",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Santo Domingo" },
                    { 2, "Santiago" },
                    { 3, "La Vega" }
                });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 17, 27, 22, 904, DateTimeKind.Utc).AddTicks(377), new DateTime(2025, 5, 8, 17, 27, 22, 904, DateTimeKind.Utc).AddTicks(378) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 17, 27, 22, 904, DateTimeKind.Utc).AddTicks(380), new DateTime(2025, 5, 8, 17, 27, 22, 904, DateTimeKind.Utc).AddTicks(381) });

            migrationBuilder.InsertData(
                table: "TiposIdentificacion",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Cédula de identidad y electoral", "Cédula" },
                    { 2, "Registro Nacional del Contribuyente", "RNC" },
                    { 3, "Pasaporte", "Pasaporte" }
                });

            migrationBuilder.InsertData(
                table: "TiposNcf",
                columns: new[] { "Id", "Codigo", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "B01", null, "Factura de Crédito Fiscal" },
                    { 2, "B02", null, "Factura de Consumo" },
                    { 3, "B03", null, "Nota de Débito" },
                    { 4, "B04", null, "Nota de Crédito" }
                });

            migrationBuilder.InsertData(
                table: "Vendedores",
                columns: new[] { "Id", "Activo", "Email", "FechaCreacion", "Nombre", "PorcentajeComision", "Telefono" },
                values: new object[,]
                {
                    { 1, true, "juan@example.com", new DateTime(2025, 5, 8, 17, 27, 22, 905, DateTimeKind.Utc).AddTicks(7275), "Juan Pérez", 5m, "809-555-1234" },
                    { 2, true, "maria@example.com", new DateTime(2025, 5, 8, 17, 27, 22, 905, DateTimeKind.Utc).AddTicks(7281), "María González", 7m, "809-555-5678" }
                });

            migrationBuilder.InsertData(
                table: "Municipios",
                columns: new[] { "Id", "Nombre", "ProvinciaId" },
                values: new object[,]
                {
                    { 1, "Santo Domingo Este", 1 },
                    { 2, "Santo Domingo Norte", 1 },
                    { 3, "Santiago", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ListasPrecios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ListasPrecios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ListasPrecios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Municipios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Municipios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Municipios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Provincias",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TiposIdentificacion",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TiposIdentificacion",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TiposIdentificacion",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TiposNcf",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TiposNcf",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TiposNcf",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TiposNcf",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Provincias",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Provincias",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(4890));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(4893));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(4895));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(4897));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(4899));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 17, 24, 32, 217, DateTimeKind.Utc).AddTicks(4900));

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
        }
    }
}
