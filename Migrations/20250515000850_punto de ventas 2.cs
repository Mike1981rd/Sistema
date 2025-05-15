using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class puntodeventas2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PrecioVenta",
                table: "ProductosVenta",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "ProductosVenta",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Costo",
                table: "ProductosVenta",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "ProductosVenta",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ColorBotonTPV",
                table: "ProductosVenta",
                type: "character varying(7)",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "ProductosVenta",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsActivo",
                table: "ProductosVenta",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "ProductosVenta",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreCortoTPV",
                table: "ProductosVenta",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrdenClasificacion",
                table: "ProductosVenta",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PLU",
                table: "ProductosVenta",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PermiteModificadores",
                table: "ProductosVenta",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequierePuntoCoccion",
                table: "ProductosVenta",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RutaImpresoraId",
                table: "ProductosVenta",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GruposModificadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EsForzado = table.Column<bool>(type: "boolean", nullable: false),
                    MinSeleccion = table.Column<int>(type: "integer", nullable: false),
                    MaxSeleccion = table.Column<int>(type: "integer", nullable: false),
                    TipoVisualizacionTPV = table.Column<int>(type: "integer", nullable: false),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GruposModificadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaquetesComponentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoPaqueteId = table.Column<int>(type: "integer", nullable: false),
                    ComponenteProductoId = table.Column<int>(type: "integer", nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    GrupoEleccion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EsOpcional = table.Column<bool>(type: "boolean", nullable: false),
                    PrecioComponenteEnPaquete = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaquetesComponentes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaquetesComponentes_ProductosVenta_ComponenteProductoId",
                        column: x => x.ComponenteProductoId,
                        principalTable: "ProductosVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaquetesComponentes_ProductosVenta_ProductoPaqueteId",
                        column: x => x.ProductoPaqueteId,
                        principalTable: "ProductosVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecetasIngredientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoCompuestoId = table.Column<int>(type: "integer", nullable: false),
                    IngredienteProductoId = table.Column<int>(type: "integer", nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    UnidadMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecetasIngredientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecetasIngredientes_ProductosVenta_IngredienteProductoId",
                        column: x => x.IngredienteProductoId,
                        principalTable: "ProductosVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecetasIngredientes_ProductosVenta_ProductoCompuestoId",
                        column: x => x.ProductoCompuestoId,
                        principalTable: "ProductosVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VariantesProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PLUVariante = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PrecioAdicionalOAbsoluto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AjustePrecioTipo = table.Column<int>(type: "integer", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: true),
                    OrdenClasificacion = table.Column<int>(type: "integer", nullable: false),
                    EsActivo = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariantesProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariantesProducto_ProductosVenta_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "ProductosVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modificadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrupoModificadoresId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PrecioAdicional = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    StockControl = table.Column<bool>(type: "boolean", nullable: false),
                    ProductoConsumidoId = table.Column<int>(type: "integer", nullable: true),
                    CantidadConsumida = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    OrdenClasificacion = table.Column<int>(type: "integer", nullable: false),
                    EsActivo = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modificadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modificadores_GruposModificadores_GrupoModificadoresId",
                        column: x => x.GrupoModificadoresId,
                        principalTable: "GruposModificadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Modificadores_ProductosVenta_ProductoConsumidoId",
                        column: x => x.ProductoConsumidoId,
                        principalTable: "ProductosVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductosModificadoresGrupos",
                columns: table => new
                {
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    GrupoModificadoresId = table.Column<int>(type: "integer", nullable: false),
                    OrdenEspecificoProducto = table.Column<int>(type: "integer", nullable: false),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductosModificadoresGrupos", x => new { x.ProductoId, x.GrupoModificadoresId });
                    table.ForeignKey(
                        name: "FK_ProductosModificadoresGrupos_GruposModificadores_GrupoModif~",
                        column: x => x.GrupoModificadoresId,
                        principalTable: "GruposModificadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductosModificadoresGrupos_ProductosVenta_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "ProductosVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(5703));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(5706));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(5708));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(5717));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(5719));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(5721));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(7175), new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(7176) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(7179), new DateTime(2025, 5, 15, 0, 8, 49, 354, DateTimeKind.Utc).AddTicks(7179) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 357, DateTimeKind.Utc).AddTicks(219));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 15, 0, 8, 49, 357, DateTimeKind.Utc).AddTicks(228));

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_CategoriaId",
                table: "ProductosVenta",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_PLU",
                table: "ProductosVenta",
                column: "PLU",
                unique: true,
                filter: "\"PLU\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_RutaImpresoraId",
                table: "ProductosVenta",
                column: "RutaImpresoraId");

            migrationBuilder.CreateIndex(
                name: "IX_Modificadores_GrupoModificadoresId",
                table: "Modificadores",
                column: "GrupoModificadoresId");

            migrationBuilder.CreateIndex(
                name: "IX_Modificadores_ProductoConsumidoId",
                table: "Modificadores",
                column: "ProductoConsumidoId");

            migrationBuilder.CreateIndex(
                name: "IX_PaquetesComponentes_ComponenteProductoId",
                table: "PaquetesComponentes",
                column: "ComponenteProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_PaquetesComponentes_ProductoPaqueteId",
                table: "PaquetesComponentes",
                column: "ProductoPaqueteId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosModificadoresGrupos_GrupoModificadoresId",
                table: "ProductosModificadoresGrupos",
                column: "GrupoModificadoresId");

            migrationBuilder.CreateIndex(
                name: "IX_RecetasIngredientes_IngredienteProductoId",
                table: "RecetasIngredientes",
                column: "IngredienteProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_RecetasIngredientes_ProductoCompuestoId",
                table: "RecetasIngredientes",
                column: "ProductoCompuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_VariantesProducto_PLUVariante",
                table: "VariantesProducto",
                column: "PLUVariante",
                unique: true,
                filter: "\"PLUVariante\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VariantesProducto_ProductoId",
                table: "VariantesProducto",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_Categorias_CategoriaId",
                table: "ProductosVenta",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosVenta_RutasImpresora_RutaImpresoraId",
                table: "ProductosVenta",
                column: "RutaImpresoraId",
                principalTable: "RutasImpresora",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_Categorias_CategoriaId",
                table: "ProductosVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosVenta_RutasImpresora_RutaImpresoraId",
                table: "ProductosVenta");

            migrationBuilder.DropTable(
                name: "Modificadores");

            migrationBuilder.DropTable(
                name: "PaquetesComponentes");

            migrationBuilder.DropTable(
                name: "ProductosModificadoresGrupos");

            migrationBuilder.DropTable(
                name: "RecetasIngredientes");

            migrationBuilder.DropTable(
                name: "VariantesProducto");

            migrationBuilder.DropTable(
                name: "GruposModificadores");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_CategoriaId",
                table: "ProductosVenta");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_PLU",
                table: "ProductosVenta");

            migrationBuilder.DropIndex(
                name: "IX_ProductosVenta_RutaImpresoraId",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "ColorBotonTPV",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "EsActivo",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "NombreCortoTPV",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "OrdenClasificacion",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "PLU",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "PermiteModificadores",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "RequierePuntoCoccion",
                table: "ProductosVenta");

            migrationBuilder.DropColumn(
                name: "RutaImpresoraId",
                table: "ProductosVenta");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrecioVenta",
                table: "ProductosVenta",
                type: "numeric(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "ProductosVenta",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<decimal>(
                name: "Costo",
                table: "ProductosVenta",
                type: "numeric(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(2530));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(2533));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(2535));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(2545));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(2547));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(2549));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(5403), new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(5405) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(5410), new DateTime(2025, 5, 14, 22, 34, 52, 380, DateTimeKind.Utc).AddTicks(5411) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 385, DateTimeKind.Utc).AddTicks(8799));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 14, 22, 34, 52, 385, DateTimeKind.Utc).AddTicks(8809));
        }
    }
}
