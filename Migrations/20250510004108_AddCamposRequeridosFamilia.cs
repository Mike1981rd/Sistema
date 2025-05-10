using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposRequeridosFamilia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "FamiliasCuentasContables",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "FamiliasCuentasContables",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "FamiliasCuentasContables",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Familias",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Familias",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Familias",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 277, DateTimeKind.Utc).AddTicks(9253));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 277, DateTimeKind.Utc).AddTicks(9257));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 277, DateTimeKind.Utc).AddTicks(9259));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 277, DateTimeKind.Utc).AddTicks(9261));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 277, DateTimeKind.Utc).AddTicks(9262));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 277, DateTimeKind.Utc).AddTicks(9264));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 0, 41, 7, 278, DateTimeKind.Utc).AddTicks(942), new DateTime(2025, 5, 10, 0, 41, 7, 278, DateTimeKind.Utc).AddTicks(943) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 0, 41, 7, 278, DateTimeKind.Utc).AddTicks(946), new DateTime(2025, 5, 10, 0, 41, 7, 278, DateTimeKind.Utc).AddTicks(947) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 280, DateTimeKind.Utc).AddTicks(6796));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 0, 41, 7, 280, DateTimeKind.Utc).AddTicks(6803));

            migrationBuilder.CreateIndex(
                name: "IX_FamiliasCuentasContables_EmpresaId",
                table: "FamiliasCuentasContables",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Familias_EmpresaId",
                table: "Familias",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_Empresas_EmpresaId",
                table: "Familias",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FamiliasCuentasContables_Empresas_EmpresaId",
                table: "FamiliasCuentasContables",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Familias_Empresas_EmpresaId",
                table: "Familias");

            migrationBuilder.DropForeignKey(
                name: "FK_FamiliasCuentasContables_Empresas_EmpresaId",
                table: "FamiliasCuentasContables");

            migrationBuilder.DropIndex(
                name: "IX_FamiliasCuentasContables_EmpresaId",
                table: "FamiliasCuentasContables");

            migrationBuilder.DropIndex(
                name: "IX_Familias_EmpresaId",
                table: "Familias");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "FamiliasCuentasContables");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "FamiliasCuentasContables");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "FamiliasCuentasContables");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Familias");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Familias");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Familias");

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 45, 19, 486, DateTimeKind.Utc).AddTicks(2116));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 45, 19, 486, DateTimeKind.Utc).AddTicks(2119));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 45, 19, 486, DateTimeKind.Utc).AddTicks(2120));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 45, 19, 486, DateTimeKind.Utc).AddTicks(2122));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 45, 19, 486, DateTimeKind.Utc).AddTicks(2124));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 45, 19, 486, DateTimeKind.Utc).AddTicks(2126));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 23, 45, 19, 486, DateTimeKind.Utc).AddTicks(5156), new DateTime(2025, 5, 9, 23, 45, 19, 486, DateTimeKind.Utc).AddTicks(5158) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 23, 45, 19, 486, DateTimeKind.Utc).AddTicks(5160), new DateTime(2025, 5, 9, 23, 45, 19, 486, DateTimeKind.Utc).AddTicks(5161) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 45, 19, 489, DateTimeKind.Utc).AddTicks(6565));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 45, 19, 489, DateTimeKind.Utc).AddTicks(6573));
        }
    }
}
