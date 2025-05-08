using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class AddCuentaContableDestinoIdToTransaccionBanco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CuentaContableDestinoId",
                table: "TransaccionesBanco",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionesBanco_CuentaContableDestinoId",
                table: "TransaccionesBanco",
                column: "CuentaContableDestinoId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransaccionesBanco_CuentasContables_CuentaContableDestinoId",
                table: "TransaccionesBanco",
                column: "CuentaContableDestinoId",
                principalTable: "CuentasContables",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransaccionesBanco_CuentasContables_CuentaContableDestinoId",
                table: "TransaccionesBanco");

            migrationBuilder.DropIndex(
                name: "IX_TransaccionesBanco_CuentaContableDestinoId",
                table: "TransaccionesBanco");

            migrationBuilder.DropColumn(
                name: "CuentaContableDestinoId",
                table: "TransaccionesBanco");
        }
    }
}
