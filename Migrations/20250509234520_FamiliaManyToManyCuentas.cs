using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class FamiliaManyToManyCuentas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Familias_CuentasContables_CuentaContableId",
                table: "Familias");

            migrationBuilder.DropIndex(
                name: "IX_Familias_CuentaContableId",
                table: "Familias");

            migrationBuilder.DropColumn(
                name: "CuentaContableId",
                table: "Familias");

            migrationBuilder.CreateTable(
                name: "FamiliasCuentasContables",
                columns: table => new
                {
                    FamiliaId = table.Column<int>(type: "integer", nullable: false),
                    CuentaContableId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamiliasCuentasContables", x => new { x.FamiliaId, x.CuentaContableId });
                    table.ForeignKey(
                        name: "FK_FamiliasCuentasContables_CuentasContables_CuentaContableId",
                        column: x => x.CuentaContableId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamiliasCuentasContables_Familias_FamiliaId",
                        column: x => x.FamiliaId,
                        principalTable: "Familias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_FamiliasCuentasContables_CuentaContableId",
                table: "FamiliasCuentasContables",
                column: "CuentaContableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamiliasCuentasContables");

            migrationBuilder.AddColumn<int>(
                name: "CuentaContableId",
                table: "Familias",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 28, 59, 75, DateTimeKind.Utc).AddTicks(4150));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 28, 59, 75, DateTimeKind.Utc).AddTicks(4153));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 28, 59, 75, DateTimeKind.Utc).AddTicks(4155));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 28, 59, 75, DateTimeKind.Utc).AddTicks(4157));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 28, 59, 75, DateTimeKind.Utc).AddTicks(4158));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 28, 59, 75, DateTimeKind.Utc).AddTicks(4160));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 23, 28, 59, 75, DateTimeKind.Utc).AddTicks(5851), new DateTime(2025, 5, 9, 23, 28, 59, 75, DateTimeKind.Utc).AddTicks(5851) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 9, 23, 28, 59, 75, DateTimeKind.Utc).AddTicks(5854), new DateTime(2025, 5, 9, 23, 28, 59, 75, DateTimeKind.Utc).AddTicks(5855) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 28, 59, 78, DateTimeKind.Utc).AddTicks(1986));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 9, 23, 28, 59, 78, DateTimeKind.Utc).AddTicks(1992));

            migrationBuilder.CreateIndex(
                name: "IX_Familias_CuentaContableId",
                table: "Familias",
                column: "CuentaContableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_CuentasContables_CuentaContableId",
                table: "Familias",
                column: "CuentaContableId",
                principalTable: "CuentasContables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
