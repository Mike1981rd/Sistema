using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarPropinaCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PropinaImpuestoId",
                table: "Categorias",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 17, 32, 58, 209, DateTimeKind.Utc).AddTicks(6397));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 17, 32, 58, 209, DateTimeKind.Utc).AddTicks(6400));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 17, 32, 58, 209, DateTimeKind.Utc).AddTicks(6401));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 17, 32, 58, 209, DateTimeKind.Utc).AddTicks(6403));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 17, 32, 58, 209, DateTimeKind.Utc).AddTicks(6407));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 17, 32, 58, 209, DateTimeKind.Utc).AddTicks(6409));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 17, 32, 58, 209, DateTimeKind.Utc).AddTicks(8048), new DateTime(2025, 5, 10, 17, 32, 58, 209, DateTimeKind.Utc).AddTicks(8049) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 17, 32, 58, 209, DateTimeKind.Utc).AddTicks(8052), new DateTime(2025, 5, 10, 17, 32, 58, 209, DateTimeKind.Utc).AddTicks(8052) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 17, 32, 58, 212, DateTimeKind.Utc).AddTicks(2592));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 17, 32, 58, 212, DateTimeKind.Utc).AddTicks(2598));

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_PropinaImpuestoId",
                table: "Categorias",
                column: "PropinaImpuestoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Impuestos_PropinaImpuestoId",
                table: "Categorias",
                column: "PropinaImpuestoId",
                principalTable: "Impuestos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Impuestos_PropinaImpuestoId",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_PropinaImpuestoId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "PropinaImpuestoId",
                table: "Categorias");

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(895));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(899));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(901));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(904));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(906));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(909));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(5793), new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(5796) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(5800), new DateTime(2025, 5, 10, 16, 4, 25, 736, DateTimeKind.Utc).AddTicks(5800) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 739, DateTimeKind.Utc).AddTicks(509));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 10, 16, 4, 25, 739, DateTimeKind.Utc).AddTicks(519));
        }
    }
}
