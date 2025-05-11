using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoriaRutaImpresora : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RutasFisicas",
                table: "Impresoras",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "ImpuestoId",
                table: "Categorias",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RutaImpresoraId",
                table: "Categorias",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 14, 45, 40, 33, DateTimeKind.Utc).AddTicks(4040));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 14, 45, 40, 33, DateTimeKind.Utc).AddTicks(4043));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 14, 45, 40, 33, DateTimeKind.Utc).AddTicks(4045));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 14, 45, 40, 33, DateTimeKind.Utc).AddTicks(4066));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 14, 45, 40, 33, DateTimeKind.Utc).AddTicks(4068));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 14, 45, 40, 33, DateTimeKind.Utc).AddTicks(4070));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 11, 14, 45, 40, 33, DateTimeKind.Utc).AddTicks(5483), new DateTime(2025, 5, 11, 14, 45, 40, 33, DateTimeKind.Utc).AddTicks(5484) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 11, 14, 45, 40, 33, DateTimeKind.Utc).AddTicks(5487), new DateTime(2025, 5, 11, 14, 45, 40, 33, DateTimeKind.Utc).AddTicks(5487) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 14, 45, 40, 35, DateTimeKind.Utc).AddTicks(7740));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 14, 45, 40, 35, DateTimeKind.Utc).AddTicks(7749));

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_ImpuestoId",
                table: "Categorias",
                column: "ImpuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_RutaImpresoraId",
                table: "Categorias",
                column: "RutaImpresoraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Impuestos_ImpuestoId",
                table: "Categorias",
                column: "ImpuestoId",
                principalTable: "Impuestos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_RutasImpresora_RutaImpresoraId",
                table: "Categorias",
                column: "RutaImpresoraId",
                principalTable: "RutasImpresora",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Impuestos_ImpuestoId",
                table: "Categorias");

            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_RutasImpresora_RutaImpresoraId",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_ImpuestoId",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_RutaImpresoraId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "ImpuestoId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "RutaImpresoraId",
                table: "Categorias");

            migrationBuilder.AlterColumn<string>(
                name: "RutasFisicas",
                table: "Impresoras",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 314, DateTimeKind.Utc).AddTicks(9701));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 314, DateTimeKind.Utc).AddTicks(9703));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 314, DateTimeKind.Utc).AddTicks(9705));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 314, DateTimeKind.Utc).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 314, DateTimeKind.Utc).AddTicks(9708));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 314, DateTimeKind.Utc).AddTicks(9710));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 11, 1, 41, 9, 315, DateTimeKind.Utc).AddTicks(1342), new DateTime(2025, 5, 11, 1, 41, 9, 315, DateTimeKind.Utc).AddTicks(1342) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 11, 1, 41, 9, 315, DateTimeKind.Utc).AddTicks(1346), new DateTime(2025, 5, 11, 1, 41, 9, 315, DateTimeKind.Utc).AddTicks(1346) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 317, DateTimeKind.Utc).AddTicks(6862));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 11, 1, 41, 9, 317, DateTimeKind.Utc).AddTicks(6870));
        }
    }
}
