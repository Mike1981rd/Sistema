using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class AddPaisesSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Paises",
                columns: new[] { "Id", "Bandera", "Codigo", "Nombre" },
                values: new object[,]
                {
                    { 1, "/images/flags/DO.png", "DO", "República Dominicana" },
                    { 2, "/images/flags/US.png", "US", "Estados Unidos" },
                    { 3, "/images/flags/ES.png", "ES", "España" },
                    { 4, "/images/flags/MX.png", "MX", "México" },
                    { 5, "/images/flags/CO.png", "CO", "Colombia" }
                });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 54, 0, 369, DateTimeKind.Utc).AddTicks(4673));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 54, 0, 369, DateTimeKind.Utc).AddTicks(4676));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 54, 0, 369, DateTimeKind.Utc).AddTicks(4678));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 54, 0, 369, DateTimeKind.Utc).AddTicks(4679));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 54, 0, 369, DateTimeKind.Utc).AddTicks(4681));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 54, 0, 369, DateTimeKind.Utc).AddTicks(4682));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 18, 54, 0, 369, DateTimeKind.Utc).AddTicks(6271), new DateTime(2025, 5, 8, 18, 54, 0, 369, DateTimeKind.Utc).AddTicks(6272) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 18, 54, 0, 369, DateTimeKind.Utc).AddTicks(6274), new DateTime(2025, 5, 8, 18, 54, 0, 369, DateTimeKind.Utc).AddTicks(6275) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 54, 0, 374, DateTimeKind.Utc).AddTicks(865));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 54, 0, 374, DateTimeKind.Utc).AddTicks(876));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(3223));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(3226));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(3228));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(3230));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(3231));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(3233));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(5057), new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(5058) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(5061), new DateTime(2025, 5, 8, 18, 43, 48, 668, DateTimeKind.Utc).AddTicks(5062) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 672, DateTimeKind.Utc).AddTicks(6140));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 8, 18, 43, 48, 672, DateTimeKind.Utc).AddTicks(6152));
        }
    }
}
