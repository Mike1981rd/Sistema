using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class SincronizarClientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Activo",
                table: "Impuestos",
                newName: "Estado");

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "CuentasContables",
                type: "boolean",
                nullable: false,
                defaultValue: false);



            migrationBuilder.AddColumn<int>(
                name: "UsuarioCreacionId",
                table: "Categorias",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Marcas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marcas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadesMedida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Abreviatura = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesMedida", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CodigoBarras = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    ImagenUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CategoriaId = table.Column<int>(type: "integer", nullable: false),
                    MarcaId = table.Column<int>(type: "integer", nullable: true),
                    UnidadMedidaInventarioId = table.Column<int>(type: "integer", nullable: false),
                    Rendimiento = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    NivelMinimo = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    StockActual = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    ImpuestoId = table.Column<int>(type: "integer", nullable: true),
                    CuentaVentasId = table.Column<int>(type: "integer", nullable: true),
                    CuentaComprasInventariosId = table.Column<int>(type: "integer", nullable: true),
                    CuentaCostoVentasGastosId = table.Column<int>(type: "integer", nullable: true),
                    CuentaDescuentosId = table.Column<int>(type: "integer", nullable: true),
                    CuentaDevolucionesId = table.Column<int>(type: "integer", nullable: true),
                    CuentaAjustesId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_CuentasContables_CuentaAjustesId",
                        column: x => x.CuentaAjustesId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_CuentasContables_CuentaComprasInventariosId",
                        column: x => x.CuentaComprasInventariosId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_CuentasContables_CuentaCostoVentasGastosId",
                        column: x => x.CuentaCostoVentasGastosId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_CuentasContables_CuentaDescuentosId",
                        column: x => x.CuentaDescuentosId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_CuentasContables_CuentaDevolucionesId",
                        column: x => x.CuentaDevolucionesId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_CuentasContables_CuentaVentasId",
                        column: x => x.CuentaVentasId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Impuestos_ImpuestoId",
                        column: x => x.ImpuestoId,
                        principalTable: "Impuestos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Marcas_MarcaId",
                        column: x => x.MarcaId,
                        principalTable: "Marcas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_UnidadesMedida_UnidadMedidaInventarioId",
                        column: x => x.UnidadMedidaInventarioId,
                        principalTable: "UnidadesMedida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemAlmacenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    AlmacenId = table.Column<int>(type: "integer", nullable: false),
                    Stock = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    NivelMinimo = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Ubicacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemAlmacenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemAlmacenes_Almacenes_AlmacenId",
                        column: x => x.AlmacenId,
                        principalTable: "Almacenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemAlmacenes_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemContenedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    UnidadMedidaId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Etiqueta = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ContenedorSuperiorId = table.Column<int>(type: "integer", nullable: true),
                    Factor = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    Costo = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    EsPrincipal = table.Column<bool>(type: "boolean", nullable: false),
                    EsContenedorCompra = table.Column<bool>(type: "boolean", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemContenedores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemContenedores_ItemContenedores_ContenedorSuperiorId",
                        column: x => x.ContenedorSuperiorId,
                        principalTable: "ItemContenedores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemContenedores_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemContenedores_UnidadesMedida_UnidadMedidaId",
                        column: x => x.UnidadMedidaId,
                        principalTable: "UnidadesMedida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemProveedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    ProveedorId = table.Column<int>(type: "integer", nullable: false),
                    NombreCompra = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CodigoProveedor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PrecioCompra = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    UnidadMedidaCompraId = table.Column<int>(type: "integer", nullable: false),
                    FactorConversion = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    EsPrincipal = table.Column<bool>(type: "boolean", nullable: false),
                    UltimaActualizacionPrecio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemProveedores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemProveedores_Clientes_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemProveedores_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemProveedores_UnidadesMedida_UnidadMedidaCompraId",
                        column: x => x.UnidadMedidaCompraId,
                        principalTable: "UnidadesMedida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTaras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    ItemContenedorId = table.Column<int>(type: "integer", nullable: false),
                    ValorTara = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Observacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTaras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTaras_ItemContenedores_ItemContenedorId",
                        column: x => x.ItemContenedorId,
                        principalTable: "ItemContenedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemTaras_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductosVenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    ItemContenedorId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Costo = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    CostoTotal = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    ImpuestoId = table.Column<int>(type: "integer", nullable: true),
                    DisponibleParaVenta = table.Column<bool>(type: "boolean", nullable: false),
                    RequierePreparacion = table.Column<bool>(type: "boolean", nullable: false),
                    TiempoPreparacion = table.Column<int>(type: "integer", nullable: true),
                    UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductosVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductosVenta_Impuestos_ImpuestoId",
                        column: x => x.ImpuestoId,
                        principalTable: "Impuestos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductosVenta_ItemContenedores_ItemContenedorId",
                        column: x => x.ItemContenedorId,
                        principalTable: "ItemContenedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductosVenta_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(4475));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(4478));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(4480));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(4482));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(4483));

            migrationBuilder.UpdateData(
                table: "PlazosPago",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(4485));

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(5912), new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(5913) });

            migrationBuilder.UpdateData(
                table: "Retenciones",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "FechaModificacion" },
                values: new object[] { new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(5918), new DateTime(2025, 5, 12, 3, 30, 10, 369, DateTimeKind.Utc).AddTicks(5918) });

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 372, DateTimeKind.Utc).AddTicks(1546));

            migrationBuilder.UpdateData(
                table: "Vendedores",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 5, 12, 3, 30, 10, 372, DateTimeKind.Utc).AddTicks(1555));

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_EmpresaId",
                table: "Clientes",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemAlmacenes_AlmacenId",
                table: "ItemAlmacenes",
                column: "AlmacenId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemAlmacenes_ItemId",
                table: "ItemAlmacenes",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemContenedores_ContenedorSuperiorId",
                table: "ItemContenedores",
                column: "ContenedorSuperiorId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemContenedores_ItemId",
                table: "ItemContenedores",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemContenedores_UnidadMedidaId",
                table: "ItemContenedores",
                column: "UnidadMedidaId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemProveedores_ItemId",
                table: "ItemProveedores",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemProveedores_ProveedorId",
                table: "ItemProveedores",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemProveedores_UnidadMedidaCompraId",
                table: "ItemProveedores",
                column: "UnidadMedidaCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoriaId",
                table: "Items",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CuentaAjustesId",
                table: "Items",
                column: "CuentaAjustesId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CuentaComprasInventariosId",
                table: "Items",
                column: "CuentaComprasInventariosId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CuentaCostoVentasGastosId",
                table: "Items",
                column: "CuentaCostoVentasGastosId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CuentaDescuentosId",
                table: "Items",
                column: "CuentaDescuentosId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CuentaDevolucionesId",
                table: "Items",
                column: "CuentaDevolucionesId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CuentaVentasId",
                table: "Items",
                column: "CuentaVentasId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ImpuestoId",
                table: "Items",
                column: "ImpuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_MarcaId",
                table: "Items",
                column: "MarcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_UnidadMedidaInventarioId",
                table: "Items",
                column: "UnidadMedidaInventarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTaras_ItemContenedorId",
                table: "ItemTaras",
                column: "ItemContenedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTaras_ItemId",
                table: "ItemTaras",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_ImpuestoId",
                table: "ProductosVenta",
                column: "ImpuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_ItemContenedorId",
                table: "ProductosVenta",
                column: "ItemContenedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosVenta_ItemId",
                table: "ProductosVenta",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Empresas_EmpresaId",
                table: "Clientes",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Empresas_EmpresaId",
                table: "Clientes");

            migrationBuilder.DropTable(
                name: "ItemAlmacenes");

            migrationBuilder.DropTable(
                name: "ItemProveedores");

            migrationBuilder.DropTable(
                name: "ItemTaras");

            migrationBuilder.DropTable(
                name: "ProductosVenta");

            migrationBuilder.DropTable(
                name: "ItemContenedores");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Marcas");

            migrationBuilder.DropTable(
                name: "UnidadesMedida");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_EmpresaId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "CuentasContables");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "UsuarioCreacionId",
                table: "Categorias");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Impuestos",
                newName: "Activo");

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
        }
    }
}
