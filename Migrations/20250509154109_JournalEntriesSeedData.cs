using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class JournalEntriesSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 841, DateTimeKind.Utc).AddTicks(9385));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 841, DateTimeKind.Utc).AddTicks(9389));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 841, DateTimeKind.Utc).AddTicks(9391));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 841, DateTimeKind.Utc).AddTicks(9393));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 841, DateTimeKind.Utc).AddTicks(9394));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 841, DateTimeKind.Utc).AddTicks(9396));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 41, 8, 842, DateTimeKind.Utc).AddTicks(959), new DateTime(2025, 5, 9, 15, 41, 8, 842, DateTimeKind.Utc).AddTicks(960) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 41, 8, 842, DateTimeKind.Utc).AddTicks(963), new DateTime(2025, 5, 9, 15, 41, 8, 842, DateTimeKind.Utc).AddTicks(963) });

            migrationBuilder.InsertData(
                table: "TiposEntradaDiario",
                columns: new[] { "Id", "Codigo", "Nombre" },
                values: new object[,]
                {
                    { 1, "AC", "Ajuste contable" },
                    { 2, "CA", "Cierre de periodos contables" },
                    { 3, "CPC", "Cuentas por cobrar" },
                    { 4, "CPP", "Cuentas por pagar" },
                    { 5, "D", "Depreciaciones" },
                    { 6, "IMP", "Impuestos" }
                });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 844, DateTimeKind.Utc).AddTicks(9825));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 41, 8, 844, DateTimeKind.Utc).AddTicks(9832));

            migrationBuilder.InsertData(
                table: "NumeracionesEntradaDiario",
                columns: new[] { "Id", "EsPreferida", "Nombre", "NumeroActual", "Prefijo", "TipoEntradaDiarioId" },
                values: new object[,]
                {
                    { 1, true, "Ajuste contable", 1, "AC", 1 },
                    { 2, true, "Cierre contable", 1, "CA", 2 },
                    { 3, true, "Cuentas por cobrar", 1, "CPC", 3 },
                    { 4, true, "Cuentas por pagar", 1, "CPP", 4 },
                    { 5, true, "Depreciaciones", 1, "D", 5 },
                    { 6, true, "Impuestos", 1, "IMP", 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NumeracionesEntradaDiario",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NumeracionesEntradaDiario",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "NumeracionesEntradaDiario",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "NumeracionesEntradaDiario",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "NumeracionesEntradaDiario",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "NumeracionesEntradaDiario",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TiposEntradaDiario",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TiposEntradaDiario",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TiposEntradaDiario",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TiposEntradaDiario",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TiposEntradaDiario",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TiposEntradaDiario",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 38, 14, 796, DateTimeKind.Utc).AddTicks(9349));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 38, 14, 796, DateTimeKind.Utc).AddTicks(9353));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 38, 14, 796, DateTimeKind.Utc).AddTicks(9355));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 38, 14, 796, DateTimeKind.Utc).AddTicks(9358));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 38, 14, 796, DateTimeKind.Utc).AddTicks(9360));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 38, 14, 796, DateTimeKind.Utc).AddTicks(9363));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 38, 14, 797, DateTimeKind.Utc).AddTicks(2263), new DateTime(2025, 5, 9, 15, 38, 14, 797, DateTimeKind.Utc).AddTicks(2266) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 38, 14, 797, DateTimeKind.Utc).AddTicks(2272), new DateTime(2025, 5, 9, 15, 38, 14, 797, DateTimeKind.Utc).AddTicks(2273) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 38, 14, 803, DateTimeKind.Utc).AddTicks(7566));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 15, 38, 14, 803, DateTimeKind.Utc).AddTicks(7575));
        }
    }
}
