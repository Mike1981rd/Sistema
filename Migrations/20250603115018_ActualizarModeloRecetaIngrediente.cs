using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarModeloRecetaIngrediente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Eliminar la columna IngredienteProductoId y UnidadMedida
            migrationBuilder.DropForeignKey(
                name: "FK_RecetasIngredientes_ProductosVenta_IngredienteProductoId",
                table: "RecetasIngredientes");
                
            migrationBuilder.DropColumn(
                name: "IngredienteProductoId",
                table: "RecetasIngredientes");
                
            migrationBuilder.DropColumn(
                name: "UnidadMedida",
                table: "RecetasIngredientes");
            
            // Agregar nuevas columnas
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "RecetasIngredientes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
                
            migrationBuilder.AddColumn<int>(
                name: "ItemContenedorId",
                table: "RecetasIngredientes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
                
            migrationBuilder.AddColumn<decimal>(
                name: "CostoUnitario",
                table: "RecetasIngredientes",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);
                
            migrationBuilder.AddColumn<decimal>(
                name: "CostoTotal",
                table: "RecetasIngredientes",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);
            
            // Agregar campos a ProductoVenta
            migrationBuilder.AddColumn<string>(
                name: "NotasReceta",
                table: "ProductosVenta",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);
                
            migrationBuilder.AddColumn<decimal>(
                name: "MargenGananciaReceta",
                table: "ProductosVenta",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);
                
            migrationBuilder.AddColumn<decimal>(
                name: "CostoTotalReceta",
                table: "ProductosVenta",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);
            
            // Crear índices
            migrationBuilder.CreateIndex(
                name: "IX_RecetasIngredientes_ItemId",
                table: "RecetasIngredientes",
                column: "ItemId");
                
            migrationBuilder.CreateIndex(
                name: "IX_RecetasIngredientes_ItemContenedorId",
                table: "RecetasIngredientes",
                column: "ItemContenedorId");
            
            // Agregar foreign keys
            migrationBuilder.AddForeignKey(
                name: "FK_RecetasIngredientes_Items_ItemId",
                table: "RecetasIngredientes",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
                
            migrationBuilder.AddForeignKey(
                name: "FK_RecetasIngredientes_ItemContenedores_ItemContenedorId",
                table: "RecetasIngredientes",
                column: "ItemContenedorId",
                principalTable: "ItemContenedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar foreign keys
            migrationBuilder.DropForeignKey(
                name: "FK_RecetasIngredientes_Items_ItemId",
                table: "RecetasIngredientes");
                
            migrationBuilder.DropForeignKey(
                name: "FK_RecetasIngredientes_ItemContenedores_ItemContenedorId",
                table: "RecetasIngredientes");
            
            // Eliminar índices
            migrationBuilder.DropIndex(
                name: "IX_RecetasIngredientes_ItemId",
                table: "RecetasIngredientes");
                
            migrationBuilder.DropIndex(
                name: "IX_RecetasIngredientes_ItemContenedorId",
                table: "RecetasIngredientes");
            
            // Eliminar columnas nuevas
            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "RecetasIngredientes");
                
            migrationBuilder.DropColumn(
                name: "ItemContenedorId",
                table: "RecetasIngredientes");
                
            migrationBuilder.DropColumn(
                name: "CostoUnitario",
                table: "RecetasIngredientes");
                
            migrationBuilder.DropColumn(
                name: "CostoTotal",
                table: "RecetasIngredientes");
                
            migrationBuilder.DropColumn(
                name: "NotasReceta",
                table: "ProductosVenta");
                
            migrationBuilder.DropColumn(
                name: "MargenGananciaReceta",
                table: "ProductosVenta");
                
            migrationBuilder.DropColumn(
                name: "CostoTotalReceta",
                table: "ProductosVenta");
            
            // Restaurar columnas originales
            migrationBuilder.AddColumn<int>(
                name: "IngredienteProductoId",
                table: "RecetasIngredientes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
                
            migrationBuilder.AddColumn<string>(
                name: "UnidadMedida",
                table: "RecetasIngredientes",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
            
            migrationBuilder.CreateIndex(
                name: "IX_RecetasIngredientes_IngredienteProductoId",
                table: "RecetasIngredientes",
                column: "IngredienteProductoId");
            
            migrationBuilder.AddForeignKey(
                name: "FK_RecetasIngredientes_ProductosVenta_IngredienteProductoId",
                table: "RecetasIngredientes",
                column: "IngredienteProductoId",
                principalTable: "ProductosVenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}