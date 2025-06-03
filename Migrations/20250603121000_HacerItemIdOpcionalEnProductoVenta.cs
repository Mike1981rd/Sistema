using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class HacerItemIdOpcionalEnProductoVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Eliminar la restricción de foreign key existente
            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_Items_ItemId",
                table: "ProductosVenta");
            
            // Hacer ItemId nullable
            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ProductosVenta",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
            
            // Recrear la foreign key con la nueva configuración
            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_Items_ItemId",
                table: "ProductosVenta",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar la foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_Items_ItemId",
                table: "ProductosVenta");
            
            // Actualizar registros nulos con un valor por defecto
            // NOTA: Esto puede fallar si no existe un Item con Id = 1
            migrationBuilder.Sql(@"
                UPDATE ""ProductosVenta"" 
                SET ""ItemId"" = (SELECT MIN(""Id"") FROM ""Items"" LIMIT 1)
                WHERE ""ItemId"" IS NULL
            ");
            
            // Hacer ItemId NOT NULL nuevamente
            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ProductosVenta",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
            
            // Recrear la foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_Items_ItemId",
                table: "ProductosVenta",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}