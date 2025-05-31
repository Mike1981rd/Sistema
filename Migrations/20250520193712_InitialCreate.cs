using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaContable.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "ComprobantesFiscales",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         TipoDocumento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         Preferida = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
            //         Electronica = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
            //         Prefijo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
            //         NumeroInicial = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
            //         NumeroFinal = table.Column<int>(type: "integer", nullable: true),
            //         SiguienteNumero = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
            //         FechaFinalizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         FechaVencimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         Sucursal = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         UltimaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         Activo = table.Column<bool>(type: "boolean", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ComprobantesFiscales", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Contenedores",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "text", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Contenedores", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Empresas",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         NumeroIdentificacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         TipoIdentificacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         Direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
            //         Ciudad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Provincia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         CodigoPostal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         Pais = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         SitioWeb = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         NombreComercial = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         MonedaPrincipal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         NumeroEmpleados = table.Column<int>(type: "integer", nullable: false),
            //         PrecisionDecimal = table.Column<int>(type: "integer", nullable: false),
            //         SeparadorDecimal = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
            //         LogoUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         ResponsabilidadTributaria = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         Activo = table.Column<bool>(type: "boolean", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Empresas", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "GruposModificadores",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         EsForzado = table.Column<bool>(type: "boolean", nullable: false),
            //         MinSeleccion = table.Column<int>(type: "integer", nullable: false),
            //         MaxSeleccion = table.Column<int>(type: "integer", nullable: false),
            //         TipoVisualizacionTPV = table.Column<string>(type: "text", nullable: false),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_GruposModificadores", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "ListasPrecios",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         Descripcion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //         Porcentaje = table.Column<decimal>(type: "numeric", nullable: true),
            //         EsPredeterminada = table.Column<bool>(type: "boolean", nullable: false),
            //         Activa = table.Column<bool>(type: "boolean", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ListasPrecios", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Marcas",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         Estado = table.Column<bool>(type: "boolean", nullable: false),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Marcas", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Paises",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Codigo = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
            //         Bandera = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Paises", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "PlazosPago",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Dias = table.Column<int>(type: "integer", nullable: true),
            //         EsPredeterminado = table.Column<bool>(type: "boolean", nullable: false),
            //         EsVencimientoManual = table.Column<bool>(type: "boolean", nullable: false),
            //         EstaEnUso = table.Column<bool>(type: "boolean", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_PlazosPago", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Provincias",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Provincias", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Retenciones",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Porcentaje = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
            //         Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
            //         CuentaContableVentas = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //         CuentaContableCompras = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //         CuentaContableRetencionesAsumidas = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //         Activo = table.Column<bool>(type: "boolean", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Retenciones", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "TiposEntradaDiario",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_TiposEntradaDiario", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "TiposIdentificacion",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         Descripcion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_TiposIdentificacion", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "TiposNcf",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         Descripcion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //         Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_TiposNcf", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "UnidadesMedida",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Abreviatura = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
            //         Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         Estado = table.Column<bool>(type: "boolean", nullable: false),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_UnidadesMedida", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Vendedores",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //         Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //         PorcentajeComision = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
            //         Activo = table.Column<bool>(type: "boolean", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Vendedores", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Almacenes",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         CorreoElectronico = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         Estado = table.Column<bool>(type: "boolean", nullable: false),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Almacenes", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Almacenes_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AsientosContables",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         Concepto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
            //         Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         Contabilizado = table.Column<bool>(type: "boolean", nullable: false),
            //         FechaContabilizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         Tipo = table.Column<int>(type: "integer", nullable: false),
            //         MontoTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         OrigenDocumento = table.Column<string>(type: "text", nullable: true),
            //         OrigenId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         UsuarioCreacion = table.Column<string>(type: "text", nullable: true),
            //         UsuarioModificacion = table.Column<string>(type: "text", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AsientosContables", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_AsientosContables_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Contactos",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Identificacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //         Telefono = table.Column<string>(type: "text", nullable: true),
            //         Email = table.Column<string>(type: "text", nullable: true),
            //         Direccion = table.Column<string>(type: "text", nullable: true),
            //         EsCliente = table.Column<bool>(type: "boolean", nullable: false),
            //         EsProveedor = table.Column<bool>(type: "boolean", nullable: false),
            //         Activo = table.Column<bool>(type: "boolean", nullable: false),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Contactos", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Contactos_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "CuentasContables",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         Categoria = table.Column<string>(type: "text", nullable: false),
            //         Naturaleza = table.Column<string>(type: "text", nullable: false),
            //         TipoCuenta = table.Column<string>(type: "text", nullable: false),
            //         UsoCuenta = table.Column<string>(type: "text", nullable: true),
            //         VerSaldoPorTercero = table.Column<bool>(type: "boolean", nullable: false),
            //         CuentaPadreId = table.Column<int>(type: "integer", nullable: true),
            //         Nivel = table.Column<int>(type: "integer", nullable: false),
            //         Orden = table.Column<int>(type: "integer", nullable: false),
            //         Activo = table.Column<bool>(type: "boolean", nullable: false),
            //         Estado = table.Column<bool>(type: "boolean", nullable: false),
            //         EsCuentaSistema = table.Column<bool>(type: "boolean", nullable: false),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_CuentasContables", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_CuentasContables_CuentasContables_CuentaPadreId",
            //             column: x => x.CuentaPadreId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_CuentasContables_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Impresoras",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Modelo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         RutasFisicas = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //         Estado = table.Column<bool>(type: "boolean", nullable: false),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Impresoras", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Impresoras_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "RutasImpresora",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Default = table.Column<bool>(type: "boolean", nullable: false),
            //         Estado = table.Column<bool>(type: "boolean", nullable: false),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_RutasImpresora", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_RutasImpresora_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Municipios",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         ProvinciaId = table.Column<int>(type: "integer", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Municipios", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Municipios_Provincias_ProvinciaId",
            //             column: x => x.ProvinciaId,
            //             principalTable: "Provincias",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "NumeracionesEntradaDiario",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         TipoEntradaDiarioId = table.Column<int>(type: "integer", nullable: false),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Prefijo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
            //         NumeroActual = table.Column<int>(type: "integer", nullable: false),
            //         EsPreferida = table.Column<bool>(type: "boolean", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_NumeracionesEntradaDiario", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_NumeracionesEntradaDiario_TiposEntradaDiario_TipoEntradaDia~",
            //             column: x => x.TipoEntradaDiarioId,
            //             principalTable: "TiposEntradaDiario",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Bancos",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         NumeroCuenta = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         TipoCuenta = table.Column<int>(type: "integer", nullable: false),
            //         EntidadBancaria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Moneda = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //         Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         SaldoInicial = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         SaldoActual = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         SaldoConciliado = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         FechaApertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         Activo = table.Column<bool>(type: "boolean", nullable: false),
            //         LogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         CuentaContableId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         UsuarioCreacion = table.Column<string>(type: "text", nullable: true),
            //         UsuarioModificacion = table.Column<string>(type: "text", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Bancos", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Bancos_CuentasContables_CuentaContableId",
            //             column: x => x.CuentaContableId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Bancos_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "DetallesAsientoContable",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         AsientoContableId = table.Column<int>(type: "integer", nullable: false),
            //         CuentaContableId = table.Column<int>(type: "integer", nullable: false),
            //         Debe = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         Haber = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         Concepto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            //         ContactoId = table.Column<int>(type: "integer", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_DetallesAsientoContable", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_DetallesAsientoContable_AsientosContables_AsientoContableId",
            //             column: x => x.AsientoContableId,
            //             principalTable: "AsientosContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_DetallesAsientoContable_Contactos_ContactoId",
            //             column: x => x.ContactoId,
            //             principalTable: "Contactos",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_DetallesAsientoContable_CuentasContables_CuentaContableId",
            //             column: x => x.CuentaContableId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Familias",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Nota = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         Estado = table.Column<bool>(type: "boolean", nullable: false),
            //         CuentaVentasId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaComprasInventariosId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaCostoVentasGastosId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaDescuentosId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaDevolucionesId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaAjustesId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaCostoMateriaPrimaId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Familias", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Familias_CuentasContables_CuentaAjustesId",
            //             column: x => x.CuentaAjustesId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Familias_CuentasContables_CuentaComprasInventariosId",
            //             column: x => x.CuentaComprasInventariosId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Familias_CuentasContables_CuentaCostoMateriaPrimaId",
            //             column: x => x.CuentaCostoMateriaPrimaId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Familias_CuentasContables_CuentaCostoVentasGastosId",
            //             column: x => x.CuentaCostoVentasGastosId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Familias_CuentasContables_CuentaDescuentosId",
            //             column: x => x.CuentaDescuentosId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Familias_CuentasContables_CuentaDevolucionesId",
            //             column: x => x.CuentaDevolucionesId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Familias_CuentasContables_CuentaVentasId",
            //             column: x => x.CuentaVentasId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Familias_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Impuestos",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Tipo = table.Column<int>(type: "integer", nullable: false),
            //         Porcentaje = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
            //         Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         EsAcreditable = table.Column<bool>(type: "boolean", nullable: false),
            //         CuentaContableVentasId = table.Column<int>(type: "integer", nullable: false),
            //         CuentaContableComprasId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         EstaEnUso = table.Column<bool>(type: "boolean", nullable: false),
            //         Estado = table.Column<bool>(type: "boolean", nullable: false),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         Activo = table.Column<bool>(type: "boolean", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Impuestos", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Impuestos_CuentasContables_CuentaContableComprasId",
            //             column: x => x.CuentaContableComprasId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Impuestos_CuentasContables_CuentaContableVentasId",
            //             column: x => x.CuentaContableVentasId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Impuestos_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "SaldosIniciales",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         CuentaContableId = table.Column<int>(type: "integer", nullable: false),
            //         ContactoId = table.Column<int>(type: "integer", nullable: true),
            //         Valor = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         FechaInicial = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_SaldosIniciales", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_SaldosIniciales_Contactos_ContactoId",
            //             column: x => x.ContactoId,
            //             principalTable: "Contactos",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull);
            //         table.ForeignKey(
            //             name: "FK_SaldosIniciales_CuentasContables_CuentaContableId",
            //             column: x => x.CuentaContableId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_SaldosIniciales_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Clientes",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         NombreRazonSocial = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
            //         TipoIdentificacionId = table.Column<int>(type: "integer", nullable: false),
            //         NumeroIdentificacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         MunicipioId = table.Column<int>(type: "integer", nullable: true),
            //         PaisId = table.Column<int>(type: "integer", nullable: true),
            //         Direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            //         Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //         Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //         Celular = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //         TipoNcfId = table.Column<int>(type: "integer", nullable: true),
            //         PlazoPagoId = table.Column<int>(type: "integer", nullable: true),
            //         ListaPrecioId = table.Column<int>(type: "integer", nullable: true),
            //         VendedorId = table.Column<int>(type: "integer", nullable: true),
            //         LimiteCredito = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
            //         EsCliente = table.Column<bool>(type: "boolean", nullable: false),
            //         EsProveedor = table.Column<bool>(type: "boolean", nullable: false),
            //         ImagenUrl = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
            //         CuentaPorCobrarId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaPorPagarId = table.Column<int>(type: "integer", nullable: true),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Clientes", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Clientes_ComprobantesFiscales_TipoNcfId",
            //             column: x => x.TipoNcfId,
            //             principalTable: "ComprobantesFiscales",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull);
            //         table.ForeignKey(
            //             name: "FK_Clientes_CuentasContables_CuentaPorCobrarId",
            //             column: x => x.CuentaPorCobrarId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull);
            //         table.ForeignKey(
            //             name: "FK_Clientes_CuentasContables_CuentaPorPagarId",
            //             column: x => x.CuentaPorPagarId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull);
            //         table.ForeignKey(
            //             name: "FK_Clientes_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Clientes_ListasPrecios_ListaPrecioId",
            //             column: x => x.ListaPrecioId,
            //             principalTable: "ListasPrecios",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull);
            //         table.ForeignKey(
            //             name: "FK_Clientes_Municipios_MunicipioId",
            //             column: x => x.MunicipioId,
            //             principalTable: "Municipios",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull);
            //         table.ForeignKey(
            //             name: "FK_Clientes_Paises_PaisId",
            //             column: x => x.PaisId,
            //             principalTable: "Paises",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Clientes_PlazosPago_PlazoPagoId",
            //             column: x => x.PlazoPagoId,
            //             principalTable: "PlazosPago",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull);
            //         table.ForeignKey(
            //             name: "FK_Clientes_TiposIdentificacion_TipoIdentificacionId",
            //             column: x => x.TipoIdentificacionId,
            //             principalTable: "TiposIdentificacion",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Clientes_Vendedores_VendedorId",
            //             column: x => x.VendedorId,
            //             principalTable: "Vendedores",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "EntradasDiario",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         TipoEntradaId = table.Column<int>(type: "integer", nullable: false),
            //         NumeracionId = table.Column<int>(type: "integer", nullable: false),
            //         Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         Estado = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         FechaCierre = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         FechaAnulacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_EntradasDiario", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_EntradasDiario_NumeracionesEntradaDiario_NumeracionId",
            //             column: x => x.NumeracionId,
            //             principalTable: "NumeracionesEntradaDiario",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_EntradasDiario_TiposEntradaDiario_TipoEntradaId",
            //             column: x => x.TipoEntradaId,
            //             principalTable: "TiposEntradaDiario",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "ConciliacionesBancarias",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         BancoId = table.Column<int>(type: "integer", nullable: false),
            //         FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaConciliacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         SaldoSegunLibro = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         SaldoSegunBanco = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         DiferenciaConciliacion = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         Notas = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         Estado = table.Column<int>(type: "integer", nullable: false),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         UsuarioCreacion = table.Column<string>(type: "text", nullable: true),
            //         UsuarioModificacion = table.Column<string>(type: "text", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ConciliacionesBancarias", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_ConciliacionesBancarias_Bancos_BancoId",
            //             column: x => x.BancoId,
            //             principalTable: "Bancos",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_ConciliacionesBancarias_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "FamiliaCuentasContables",
            //     columns: table => new
            //     {
            //         FamiliaId = table.Column<int>(type: "integer", nullable: false),
            //         CuentaContableId = table.Column<int>(type: "integer", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_FamiliaCuentasContables", x => new { x.FamiliaId, x.CuentaContableId });
            //         table.ForeignKey(
            //             name: "FK_FamiliaCuentasContables_CuentasContables_CuentaContableId",
            //             column: x => x.CuentaContableId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_FamiliaCuentasContables_Familias_FamiliaId",
            //             column: x => x.FamiliaId,
            //             principalTable: "Familias",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Categorias",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Nota = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         Estado = table.Column<bool>(type: "boolean", nullable: false),
            //         FamiliaId = table.Column<int>(type: "integer", nullable: false),
            //         CuentaVentasId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaComprasInventariosId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaCostoVentasGastosId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaDescuentosId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaDevolucionesId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaAjustesId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaCostoMateriaPrimaId = table.Column<int>(type: "integer", nullable: true),
            //         ImpuestoId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         RutaImpresoraId = table.Column<int>(type: "integer", nullable: true),
            //         Impuestos = table.Column<string>(type: "text", nullable: true),
            //         Propina = table.Column<decimal>(type: "numeric", nullable: true),
            //         CanalesImpresora = table.Column<string>(type: "text", nullable: true),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         PropinaImpuestoId = table.Column<int>(type: "integer", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Categorias", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Categorias_CuentasContables_CuentaAjustesId",
            //             column: x => x.CuentaAjustesId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Categorias_CuentasContables_CuentaComprasInventariosId",
            //             column: x => x.CuentaComprasInventariosId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Categorias_CuentasContables_CuentaCostoMateriaPrimaId",
            //             column: x => x.CuentaCostoMateriaPrimaId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Categorias_CuentasContables_CuentaCostoVentasGastosId",
            //             column: x => x.CuentaCostoVentasGastosId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Categorias_CuentasContables_CuentaDescuentosId",
            //             column: x => x.CuentaDescuentosId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Categorias_CuentasContables_CuentaDevolucionesId",
            //             column: x => x.CuentaDevolucionesId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Categorias_CuentasContables_CuentaVentasId",
            //             column: x => x.CuentaVentasId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Categorias_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_Categorias_Familias_FamiliaId",
            //             column: x => x.FamiliaId,
            //             principalTable: "Familias",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Categorias_Impuestos_ImpuestoId",
            //             column: x => x.ImpuestoId,
            //             principalTable: "Impuestos",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Categorias_Impuestos_PropinaImpuestoId",
            //             column: x => x.PropinaImpuestoId,
            //             principalTable: "Impuestos",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Categorias_RutasImpresora_RutaImpresoraId",
            //             column: x => x.RutaImpresoraId,
            //             principalTable: "RutasImpresora",
            //             principalColumn: "Id");
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Compras",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         ProveedorId = table.Column<int>(type: "integer", nullable: false),
            //         AlmacenId = table.Column<int>(type: "integer", nullable: true),
            //         Referencia = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //         Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         PlazoPagoId = table.Column<int>(type: "integer", nullable: true),
            //         FechaVencimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         Subtotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         Descuento = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         Impuestos = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         Total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         EntradaDiarioId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Compras", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Compras_Almacenes_AlmacenId",
            //             column: x => x.AlmacenId,
            //             principalTable: "Almacenes",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Compras_Clientes_ProveedorId",
            //             column: x => x.ProveedorId,
            //             principalTable: "Clientes",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Compras_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_Compras_EntradasDiario_EntradaDiarioId",
            //             column: x => x.EntradaDiarioId,
            //             principalTable: "EntradasDiario",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Compras_PlazosPago_PlazoPagoId",
            //             column: x => x.PlazoPagoId,
            //             principalTable: "PlazosPago",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "MovimientosContables",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         EntradaDiarioId = table.Column<int>(type: "integer", nullable: false),
            //         CuentaContableId = table.Column<int>(type: "integer", nullable: false),
            //         ContactoId = table.Column<int>(type: "integer", nullable: true),
            //         TipoContacto = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
            //         NumeroDocumento = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
            //         Descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            //         Debito = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
            //         Credito = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_MovimientosContables", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_MovimientosContables_Clientes_ContactoId",
            //             column: x => x.ContactoId,
            //             principalTable: "Clientes",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull);
            //         table.ForeignKey(
            //             name: "FK_MovimientosContables_CuentasContables_CuentaContableId",
            //             column: x => x.CuentaContableId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_MovimientosContables_EntradasDiario_EntradaDiarioId",
            //             column: x => x.EntradaDiarioId,
            //             principalTable: "EntradasDiario",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AjustesConciliacion",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         ConciliacionId = table.Column<int>(type: "integer", nullable: false),
            //         Tipo = table.Column<int>(type: "integer", nullable: false),
            //         Monto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         Concepto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
            //         Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         Aplicado = table.Column<bool>(type: "boolean", nullable: false),
            //         AsientoContableId = table.Column<int>(type: "integer", nullable: true),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         UsuarioCreacion = table.Column<string>(type: "text", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AjustesConciliacion", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_AjustesConciliacion_AsientosContables_AsientoContableId",
            //             column: x => x.AsientoContableId,
            //             principalTable: "AsientosContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull);
            //         table.ForeignKey(
            //             name: "FK_AjustesConciliacion_ConciliacionesBancarias_ConciliacionId",
            //             column: x => x.ConciliacionId,
            //             principalTable: "ConciliacionesBancarias",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "TransaccionesBanco",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         BancoId = table.Column<int>(type: "integer", nullable: false),
            //         Tipo = table.Column<int>(type: "integer", nullable: false),
            //         Monto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
            //         Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         Concepto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
            //         Referencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //         Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         Conciliado = table.Column<bool>(type: "boolean", nullable: false),
            //         ConciliacionId = table.Column<int>(type: "integer", nullable: true),
            //         ContactoId = table.Column<int>(type: "integer", nullable: true),
            //         AsientoContableId = table.Column<int>(type: "integer", nullable: true),
            //         BancoDestinoId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaContableDestinoId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         UsuarioCreacion = table.Column<string>(type: "text", nullable: true),
            //         UsuarioModificacion = table.Column<string>(type: "text", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_TransaccionesBanco", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_TransaccionesBanco_AsientosContables_AsientoContableId",
            //             column: x => x.AsientoContableId,
            //             principalTable: "AsientosContables",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull);
            //         table.ForeignKey(
            //             name: "FK_TransaccionesBanco_Bancos_BancoDestinoId",
            //             column: x => x.BancoDestinoId,
            //             principalTable: "Bancos",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_TransaccionesBanco_Bancos_BancoId",
            //             column: x => x.BancoId,
            //             principalTable: "Bancos",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_TransaccionesBanco_ConciliacionesBancarias_ConciliacionId",
            //             column: x => x.ConciliacionId,
            //             principalTable: "ConciliacionesBancarias",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_TransaccionesBanco_Contactos_ContactoId",
            //             column: x => x.ContactoId,
            //             principalTable: "Contactos",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_TransaccionesBanco_CuentasContables_CuentaContableDestinoId",
            //             column: x => x.CuentaContableDestinoId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_TransaccionesBanco_Empresas_EmpresaId",
            //             column: x => x.EmpresaId,
            //             principalTable: "Empresas",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Items",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         CodigoBarras = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         Estado = table.Column<bool>(type: "boolean", nullable: false),
            //         ImagenUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
            //         CategoriaId = table.Column<int>(type: "integer", nullable: false),
            //         MarcaId = table.Column<int>(type: "integer", nullable: true),
            //         UnidadMedidaInventarioId = table.Column<int>(type: "integer", nullable: true),
            //         Rendimiento = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
            //         NivelMinimo = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
            //         StockActual = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
            //         ImpuestoId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaVentasId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaComprasInventariosId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaCostoVentasGastosId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaDescuentosId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaDevolucionesId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaAjustesId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaCostoMateriaPrimaId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         Activo = table.Column<bool>(type: "boolean", nullable: false),
            //         CostoEstandar = table.Column<decimal>(type: "numeric", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Items", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Items_Categorias_CategoriaId",
            //             column: x => x.CategoriaId,
            //             principalTable: "Categorias",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_Items_CuentasContables_CuentaAjustesId",
            //             column: x => x.CuentaAjustesId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Items_CuentasContables_CuentaComprasInventariosId",
            //             column: x => x.CuentaComprasInventariosId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Items_CuentasContables_CuentaCostoMateriaPrimaId",
            //             column: x => x.CuentaCostoMateriaPrimaId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Items_CuentasContables_CuentaCostoVentasGastosId",
            //             column: x => x.CuentaCostoVentasGastosId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Items_CuentasContables_CuentaDescuentosId",
            //             column: x => x.CuentaDescuentosId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Items_CuentasContables_CuentaDevolucionesId",
            //             column: x => x.CuentaDevolucionesId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Items_CuentasContables_CuentaVentasId",
            //             column: x => x.CuentaVentasId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Items_Impuestos_ImpuestoId",
            //             column: x => x.ImpuestoId,
            //             principalTable: "Impuestos",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Items_Marcas_MarcaId",
            //             column: x => x.MarcaId,
            //             principalTable: "Marcas",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Items_UnidadesMedida_UnidadMedidaInventarioId",
            //             column: x => x.UnidadMedidaInventarioId,
            //             principalTable: "UnidadesMedida",
            //             principalColumn: "Id");
            //     });

            // migrationBuilder.CreateTable(
            //     name: "ComprasDetalles",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         CompraId = table.Column<int>(type: "integer", nullable: false),
            //         ItemId = table.Column<int>(type: "integer", nullable: false),
            //         Descripcion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
            //         Cantidad = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
            //         Precio = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
            //         Subtotal = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
            //         PorcentajeDescuento = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
            //         MontoDescuento = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
            //         ImpuestoId = table.Column<int>(type: "integer", nullable: true),
            //         MontoImpuesto = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
            //         Total = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
            //         UnidadMedidaId = table.Column<int>(type: "integer", nullable: false),
            //         FactorConversion = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ComprasDetalles", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_ComprasDetalles_Compras_CompraId",
            //             column: x => x.CompraId,
            //             principalTable: "Compras",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_ComprasDetalles_Impuestos_ImpuestoId",
            //             column: x => x.ImpuestoId,
            //             principalTable: "Impuestos",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_ComprasDetalles_Items_ItemId",
            //             column: x => x.ItemId,
            //             principalTable: "Items",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_ComprasDetalles_UnidadesMedida_UnidadMedidaId",
            //             column: x => x.UnidadMedidaId,
            //             principalTable: "UnidadesMedida",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "ItemAlmacenes",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         ItemId = table.Column<int>(type: "integer", nullable: false),
            //         AlmacenId = table.Column<int>(type: "integer", nullable: false),
            //         Stock = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
            //         NivelMinimo = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
            //         Ubicacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ItemAlmacenes", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_ItemAlmacenes_Almacenes_AlmacenId",
            //             column: x => x.AlmacenId,
            //             principalTable: "Almacenes",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_ItemAlmacenes_Items_ItemId",
            //             column: x => x.ItemId,
            //             principalTable: "Items",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "ItemContenedores",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         ItemId = table.Column<int>(type: "integer", nullable: false),
            //         UnidadMedidaId = table.Column<int>(type: "integer", nullable: false),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         Etiqueta = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
            //         ContenedorSuperiorId = table.Column<int>(type: "integer", nullable: true),
            //         Factor = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
            //         Costo = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
            //         EsPrincipal = table.Column<bool>(type: "boolean", nullable: false),
            //         EsContenedorCompra = table.Column<bool>(type: "boolean", nullable: false),
            //         Orden = table.Column<int>(type: "integer", nullable: false),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ItemContenedores", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_ItemContenedores_ItemContenedores_ContenedorSuperiorId",
            //             column: x => x.ContenedorSuperiorId,
            //             principalTable: "ItemContenedores",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_ItemContenedores_Items_ItemId",
            //             column: x => x.ItemId,
            //             principalTable: "Items",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_ItemContenedores_UnidadesMedida_UnidadMedidaId",
            //             column: x => x.UnidadMedidaId,
            //             principalTable: "UnidadesMedida",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "ItemProveedores",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         ItemId = table.Column<int>(type: "integer", nullable: false),
            //         ProveedorId = table.Column<int>(type: "integer", nullable: false),
            //         NombreCompra = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //         CodigoProveedor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //         PrecioCompra = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
            //         UnidadMedidaCompraId = table.Column<int>(type: "integer", nullable: false),
            //         FactorConversion = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
            //         EsPrincipal = table.Column<bool>(type: "boolean", nullable: false),
            //         UltimaActualizacionPrecio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ItemProveedores", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_ItemProveedores_Clientes_ProveedorId",
            //             column: x => x.ProveedorId,
            //             principalTable: "Clientes",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_ItemProveedores_Items_ItemId",
            //             column: x => x.ItemId,
            //             principalTable: "Items",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_ItemProveedores_UnidadesMedida_UnidadMedidaCompraId",
            //             column: x => x.UnidadMedidaCompraId,
            //             principalTable: "UnidadesMedida",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "ItemTaras",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         ItemId = table.Column<int>(type: "integer", nullable: false),
            //         ItemContenedorId = table.Column<int>(type: "integer", nullable: false),
            //         ValorTara = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
            //         Observacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ItemTaras", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_ItemTaras_ItemContenedores_ItemContenedorId",
            //             column: x => x.ItemContenedorId,
            //             principalTable: "ItemContenedores",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_ItemTaras_Items_ItemId",
            //             column: x => x.ItemId,
            //             principalTable: "Items",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "ProductosVenta",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         NombreCortoTPV = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //         Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
            //         PLU = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //         PrecioVenta = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
            //         Costo = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
            //         ImagenUrl = table.Column<string>(type: "text", nullable: true),
            //         ColorBotonTPV = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
            //         OrdenClasificacion = table.Column<int>(type: "integer", nullable: false),
            //         EsActivo = table.Column<bool>(type: "boolean", nullable: false),
            //         PermiteModificadores = table.Column<bool>(type: "boolean", nullable: false),
            //         RequierePuntoCoccion = table.Column<bool>(type: "boolean", nullable: false),
            //         ItemId = table.Column<int>(type: "integer", nullable: false),
            //         ItemContenedorId = table.Column<int>(type: "integer", nullable: true),
            //         CategoriaId = table.Column<int>(type: "integer", nullable: false),
            //         ImpuestoId = table.Column<int>(type: "integer", nullable: true),
            //         RutaImpresoraId = table.Column<int>(type: "integer", nullable: true),
            //         Cantidad = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
            //         CostoTotal = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
            //         DisponibleParaVenta = table.Column<bool>(type: "boolean", nullable: false),
            //         RequierePreparacion = table.Column<bool>(type: "boolean", nullable: false),
            //         TiempoPreparacion = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         CuentaVentasId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaComprasInventariosId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaCostoVentasGastosId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaDescuentosId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaDevolucionesId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaAjustesId = table.Column<int>(type: "integer", nullable: true),
            //         CuentaCostoMateriaPrimaId = table.Column<int>(type: "integer", nullable: true),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ProductosVenta", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_ProductosVenta_Categorias_CategoriaId",
            //             column: x => x.CategoriaId,
            //             principalTable: "Categorias",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_ProductosVenta_CuentasContables_CuentaAjustesId",
            //             column: x => x.CuentaAjustesId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_ProductosVenta_CuentasContables_CuentaComprasInventariosId",
            //             column: x => x.CuentaComprasInventariosId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_ProductosVenta_CuentasContables_CuentaCostoMateriaPrimaId",
            //             column: x => x.CuentaCostoMateriaPrimaId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_ProductosVenta_CuentasContables_CuentaCostoVentasGastosId",
            //             column: x => x.CuentaCostoVentasGastosId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_ProductosVenta_CuentasContables_CuentaDescuentosId",
            //             column: x => x.CuentaDescuentosId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_ProductosVenta_CuentasContables_CuentaDevolucionesId",
            //             column: x => x.CuentaDevolucionesId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_ProductosVenta_CuentasContables_CuentaVentasId",
            //             column: x => x.CuentaVentasId,
            //             principalTable: "CuentasContables",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_ProductosVenta_Impuestos_ImpuestoId",
            //             column: x => x.ImpuestoId,
            //             principalTable: "Impuestos",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull);
            //         table.ForeignKey(
            //             name: "FK_ProductosVenta_ItemContenedores_ItemContenedorId",
            //             column: x => x.ItemContenedorId,
            //             principalTable: "ItemContenedores",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_ProductosVenta_Items_ItemId",
            //             column: x => x.ItemId,
            //             principalTable: "Items",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_ProductosVenta_RutasImpresora_RutaImpresoraId",
            //             column: x => x.RutaImpresoraId,
            //             principalTable: "RutasImpresora",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Modificadores",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         GrupoModificadoresId = table.Column<int>(type: "integer", nullable: false),
            //         Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         PrecioAdicional = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
            //         StockControl = table.Column<bool>(type: "boolean", nullable: false),
            //         ProductoConsumidoId = table.Column<int>(type: "integer", nullable: true),
            //         CantidadConsumida = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
            //         OrdenClasificacion = table.Column<int>(type: "integer", nullable: false),
            //         EsActivo = table.Column<bool>(type: "boolean", nullable: false),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Modificadores", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Modificadores_GruposModificadores_GrupoModificadoresId",
            //             column: x => x.GrupoModificadoresId,
            //             principalTable: "GruposModificadores",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_Modificadores_ProductosVenta_ProductoConsumidoId",
            //             column: x => x.ProductoConsumidoId,
            //             principalTable: "ProductosVenta",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "PaquetesComponentes",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         ProductoPaqueteId = table.Column<int>(type: "integer", nullable: false),
            //         ComponenteProductoId = table.Column<int>(type: "integer", nullable: false),
            //         Cantidad = table.Column<int>(type: "integer", nullable: false),
            //         GrupoEleccion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //         EsOpcional = table.Column<bool>(type: "boolean", nullable: false),
            //         PrecioComponenteEnPaquete = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_PaquetesComponentes", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_PaquetesComponentes_ProductosVenta_ComponenteProductoId",
            //             column: x => x.ComponenteProductoId,
            //             principalTable: "ProductosVenta",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_PaquetesComponentes_ProductosVenta_ProductoPaqueteId",
            //             column: x => x.ProductoPaqueteId,
            //             principalTable: "ProductosVenta",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "ProductosModificadoresGrupos",
            //     columns: table => new
            //     {
            //         ProductoId = table.Column<int>(type: "integer", nullable: false),
            //         GrupoModificadoresId = table.Column<int>(type: "integer", nullable: false),
            //         OrdenEspecificoProducto = table.Column<int>(type: "integer", nullable: false),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ProductosModificadoresGrupos", x => new { x.ProductoId, x.GrupoModificadoresId });
            //         table.ForeignKey(
            //             name: "FK_ProductosModificadoresGrupos_GruposModificadores_GrupoModif~",
            //             column: x => x.GrupoModificadoresId,
            //             principalTable: "GruposModificadores",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_ProductosModificadoresGrupos_ProductosVenta_ProductoId",
            //             column: x => x.ProductoId,
            //             principalTable: "ProductosVenta",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "RecetasIngredientes",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         ProductoCompuestoId = table.Column<int>(type: "integer", nullable: false),
            //         IngredienteProductoId = table.Column<int>(type: "integer", nullable: false),
            //         Cantidad = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
            //         UnidadMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_RecetasIngredientes", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_RecetasIngredientes_ProductosVenta_IngredienteProductoId",
            //             column: x => x.IngredienteProductoId,
            //             principalTable: "ProductosVenta",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_RecetasIngredientes_ProductosVenta_ProductoCompuestoId",
            //             column: x => x.ProductoCompuestoId,
            //             principalTable: "ProductosVenta",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "VariantesProducto",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         ProductoId = table.Column<int>(type: "integer", nullable: false),
            //         Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         PLUVariante = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //         PrecioAdicionalOAbsoluto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
            //         AjustePrecioTipo = table.Column<string>(type: "text", nullable: false),
            //         Stock = table.Column<int>(type: "integer", nullable: true),
            //         OrdenClasificacion = table.Column<int>(type: "integer", nullable: false),
            //         EsActivo = table.Column<bool>(type: "boolean", nullable: false),
            //         UsuarioCreacionId = table.Column<int>(type: "integer", nullable: true),
            //         UsuarioModificacionId = table.Column<int>(type: "integer", nullable: true),
            //         EmpresaId = table.Column<int>(type: "integer", nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_VariantesProducto", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_VariantesProducto_ProductosVenta_ProductoId",
            //             column: x => x.ProductoId,
            //             principalTable: "ProductosVenta",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.InsertData(
            //     table: "ListasPrecios",
            //     columns: new[] { "Id", "Activa", "Descripcion", "EsPredeterminada", "Nombre", "Porcentaje" },
            //     values: new object[,]
            //     {
            //         { 1, true, "Precios regulares", true, "Lista Regular", null },
            //         { 2, true, "Precios para mayoristas", false, "Mayoristas", 10m },
            //         { 3, true, "Precios para clientes VIP", false, "VIP", 20m }
            //     });

            // migrationBuilder.InsertData(
            //     table: "Paises",
            //     columns: new[] { "Id", "Bandera", "Codigo", "Nombre" },
            //     values: new object[,]
            //     {
            //         { 1, "/images/flags/DO.png", "DO", "República Dominicana" },
            //         { 2, "/images/flags/US.png", "US", "Estados Unidos" },
            //         { 3, "/images/flags/ES.png", "ES", "España" },
            //         { 4, "/images/flags/MX.png", "MX", "México" },
            //         { 5, "/images/flags/CO.png", "CO", "Colombia" }
            //     });

            // migrationBuilder.InsertData(
            //     table: "PlazosPago",
            //     columns: new[] { "Id", "Dias", "EsPredeterminado", "EsVencimientoManual", "EstaEnUso", "FechaCreacion", "FechaModificacion", "Nombre" },
            //     values: new object[,]
            //     {
            //         { 1, 0, false, false, false, new DateTime(2025, 5, 20, 19, 37, 11, 564, DateTimeKind.Utc).AddTicks(9317), null, "De contado" },
            //         { 2, 8, false, false, false, new DateTime(2025, 5, 20, 19, 37, 11, 564, DateTimeKind.Utc).AddTicks(9319), null, "8 días" },
            //         { 3, 15, false, false, false, new DateTime(2025, 5, 20, 19, 37, 11, 564, DateTimeKind.Utc).AddTicks(9320), null, "15 días" },
            //         { 4, 30, false, false, false, new DateTime(2025, 5, 20, 19, 37, 11, 564, DateTimeKind.Utc).AddTicks(9330), null, "30 días" },
            //         { 5, 60, false, false, false, new DateTime(2025, 5, 20, 19, 37, 11, 564, DateTimeKind.Utc).AddTicks(9331), null, "60 días" },
            //         { 6, null, false, true, false, new DateTime(2025, 5, 20, 19, 37, 11, 564, DateTimeKind.Utc).AddTicks(9333), null, "Vencimiento manual" }
            //     });

            // migrationBuilder.InsertData(
            //     table: "Provincias",
            //     columns: new[] { "Id", "Nombre" },
            //     values: new object[,]
            //     {
            //         { 1, "Santo Domingo" },
            //         { 2, "Santiago" },
            //         { 3, "La Vega" }
            //     });

            // migrationBuilder.InsertData(
            //     table: "Retenciones",
            //     columns: new[] { "Id", "Activo", "CuentaContableCompras", "CuentaContableRetencionesAsumidas", "CuentaContableVentas", "Descripcion", "FechaCreacion", "FechaModificacion", "Nombre", "Porcentaje", "Tipo" },
            //     values: new object[,]
            //     {
            //         { 1, true, null, null, null, "Impuesto Sobre la Renta al 10%", new DateTime(2025, 5, 20, 19, 37, 11, 565, DateTimeKind.Utc).AddTicks(864), new DateTime(2025, 5, 20, 19, 37, 11, 565, DateTimeKind.Utc).AddTicks(865), "ISR 10%", 10.00m, "ISR" },
            //         { 2, true, null, null, null, "Retención del IVA al 15%", new DateTime(2025, 5, 20, 19, 37, 11, 565, DateTimeKind.Utc).AddTicks(868), new DateTime(2025, 5, 20, 19, 37, 11, 565, DateTimeKind.Utc).AddTicks(868), "IVA Retenido 15%", 15.00m, "IVA" }
            //     });

            // migrationBuilder.InsertData(
            //     table: "TiposEntradaDiario",
            //     columns: new[] { "Id", "Codigo", "Nombre" },
            //     values: new object[,]
            //     {
            //         { 1, "AC", "Ajuste contable" },
            //         { 2, "CA", "Cierre de periodos contables" },
            //         { 3, "CPC", "Cuentas por cobrar" },
            //         { 4, "CPP", "Cuentas por pagar" },
            //         { 5, "D", "Depreciaciones" },
            //         { 6, "IMP", "Impuestos" }
            //     });

            // migrationBuilder.InsertData(
            //     table: "TiposIdentificacion",
            //     columns: new[] { "Id", "Descripcion", "Nombre" },
            //     values: new object[,]
            //     {
            //         { 1, "Cédula de identidad y electoral", "Cédula" },
            //         { 2, "Registro Nacional del Contribuyente", "RNC" },
            //         { 3, "Pasaporte", "Pasaporte" }
            //     });

            // migrationBuilder.InsertData(
            //     table: "TiposNcf",
            //     columns: new[] { "Id", "Codigo", "Descripcion", "Nombre" },
            //     values: new object[,]
            //     {
            //         { 1, "B01", null, "Factura de Crédito Fiscal" },
            //         { 2, "B02", null, "Factura de Consumo" },
            //         { 3, "B03", null, "Nota de Débito" },
            //         { 4, "B04", null, "Nota de Crédito" }
            //     });

            // migrationBuilder.InsertData(
            //     table: "Vendedores",
            //     columns: new[] { "Id", "Activo", "Email", "FechaCreacion", "Nombre", "PorcentajeComision", "Telefono" },
            //     values: new object[,]
            //     {
            //         { 1, true, "juan@example.com", new DateTime(2025, 5, 20, 19, 37, 11, 567, DateTimeKind.Utc).AddTicks(3998), "Juan Pérez", 5m, "809-555-1234" },
            //         { 2, true, "maria@example.com", new DateTime(2025, 5, 20, 19, 37, 11, 567, DateTimeKind.Utc).AddTicks(4005), "María González", 7m, "809-555-5678" }
            //     });

            // migrationBuilder.InsertData(
            //     table: "Municipios",
            //     columns: new[] { "Id", "Nombre", "ProvinciaId" },
            //     values: new object[,]
            //     {
            //         { 1, "Santo Domingo Este", 1 },
            //         { 2, "Santo Domingo Norte", 1 },
            //         { 3, "Santiago", 2 }
            //     });

            // migrationBuilder.InsertData(
            //     table: "NumeracionesEntradaDiario",
            //     columns: new[] { "Id", "EsPreferida", "Nombre", "NumeroActual", "Prefijo", "TipoEntradaDiarioId" },
            //     values: new object[,]
            //     {
            //         { 1, true, "Ajuste contable", 1, "AC", 1 },
            //         { 2, true, "Cierre contable", 1, "CA", 2 },
            //         { 3, true, "Cuentas por cobrar", 1, "CPC", 3 },
            //         { 4, true, "Cuentas por pagar", 1, "CPP", 4 },
            //         { 5, true, "Depreciaciones", 1, "D", 5 },
            //         { 6, true, "Impuestos", 1, "IMP", 6 }
            //     });

            // migrationBuilder.CreateIndex(
            //     name: "IX_AjustesConciliacion_AsientoContableId",
            //     table: "AjustesConciliacion",
            //     column: "AsientoContableId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_AjustesConciliacion_ConciliacionId",
            //     table: "AjustesConciliacion",
            //     column: "ConciliacionId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Almacenes_EmpresaId",
            //     table: "Almacenes",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_AsientosContables_EmpresaId",
            //     table: "AsientosContables",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Bancos_CuentaContableId",
            //     table: "Bancos",
            //     column: "CuentaContableId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Bancos_EmpresaId",
            //     table: "Bancos",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Categorias_CuentaAjustesId",
            //     table: "Categorias",
            //     column: "CuentaAjustesId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Categorias_CuentaComprasInventariosId",
            //     table: "Categorias",
            //     column: "CuentaComprasInventariosId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Categorias_CuentaCostoMateriaPrimaId",
            //     table: "Categorias",
            //     column: "CuentaCostoMateriaPrimaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Categorias_CuentaCostoVentasGastosId",
            //     table: "Categorias",
            //     column: "CuentaCostoVentasGastosId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Categorias_CuentaDescuentosId",
            //     table: "Categorias",
            //     column: "CuentaDescuentosId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Categorias_CuentaDevolucionesId",
            //     table: "Categorias",
            //     column: "CuentaDevolucionesId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Categorias_CuentaVentasId",
            //     table: "Categorias",
            //     column: "CuentaVentasId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Categorias_EmpresaId",
            //     table: "Categorias",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Categorias_FamiliaId",
            //     table: "Categorias",
            //     column: "FamiliaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Categorias_ImpuestoId",
            //     table: "Categorias",
            //     column: "ImpuestoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Categorias_PropinaImpuestoId",
            //     table: "Categorias",
            //     column: "PropinaImpuestoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Categorias_RutaImpresoraId",
            //     table: "Categorias",
            //     column: "RutaImpresoraId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Clientes_CuentaPorCobrarId",
            //     table: "Clientes",
            //     column: "CuentaPorCobrarId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Clientes_CuentaPorPagarId",
            //     table: "Clientes",
            //     column: "CuentaPorPagarId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Clientes_EmpresaId",
            //     table: "Clientes",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Clientes_ListaPrecioId",
            //     table: "Clientes",
            //     column: "ListaPrecioId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Clientes_MunicipioId",
            //     table: "Clientes",
            //     column: "MunicipioId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Clientes_PaisId",
            //     table: "Clientes",
            //     column: "PaisId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Clientes_PlazoPagoId",
            //     table: "Clientes",
            //     column: "PlazoPagoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Clientes_TipoIdentificacionId",
            //     table: "Clientes",
            //     column: "TipoIdentificacionId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Clientes_TipoNcfId",
            //     table: "Clientes",
            //     column: "TipoNcfId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Clientes_VendedorId",
            //     table: "Clientes",
            //     column: "VendedorId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Compras_AlmacenId",
            //     table: "Compras",
            //     column: "AlmacenId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Compras_EmpresaId",
            //     table: "Compras",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Compras_EntradaDiarioId",
            //     table: "Compras",
            //     column: "EntradaDiarioId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Compras_PlazoPagoId",
            //     table: "Compras",
            //     column: "PlazoPagoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Compras_ProveedorId",
            //     table: "Compras",
            //     column: "ProveedorId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ComprasDetalles_CompraId",
            //     table: "ComprasDetalles",
            //     column: "CompraId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ComprasDetalles_ImpuestoId",
            //     table: "ComprasDetalles",
            //     column: "ImpuestoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ComprasDetalles_ItemId",
            //     table: "ComprasDetalles",
            //     column: "ItemId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ComprasDetalles_UnidadMedidaId",
            //     table: "ComprasDetalles",
            //     column: "UnidadMedidaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ConciliacionesBancarias_BancoId",
            //     table: "ConciliacionesBancarias",
            //     column: "BancoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ConciliacionesBancarias_EmpresaId",
            //     table: "ConciliacionesBancarias",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Contactos_EmpresaId",
            //     table: "Contactos",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_CuentasContables_CuentaPadreId",
            //     table: "CuentasContables",
            //     column: "CuentaPadreId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_CuentasContables_EmpresaId",
            //     table: "CuentasContables",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_DetallesAsientoContable_AsientoContableId",
            //     table: "DetallesAsientoContable",
            //     column: "AsientoContableId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_DetallesAsientoContable_ContactoId",
            //     table: "DetallesAsientoContable",
            //     column: "ContactoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_DetallesAsientoContable_CuentaContableId",
            //     table: "DetallesAsientoContable",
            //     column: "CuentaContableId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_EntradasDiario_NumeracionId",
            //     table: "EntradasDiario",
            //     column: "NumeracionId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_EntradasDiario_TipoEntradaId",
            //     table: "EntradasDiario",
            //     column: "TipoEntradaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_FamiliaCuentasContables_CuentaContableId",
            //     table: "FamiliaCuentasContables",
            //     column: "CuentaContableId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Familias_CuentaAjustesId",
            //     table: "Familias",
            //     column: "CuentaAjustesId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Familias_CuentaComprasInventariosId",
            //     table: "Familias",
            //     column: "CuentaComprasInventariosId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Familias_CuentaCostoMateriaPrimaId",
            //     table: "Familias",
            //     column: "CuentaCostoMateriaPrimaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Familias_CuentaCostoVentasGastosId",
            //     table: "Familias",
            //     column: "CuentaCostoVentasGastosId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Familias_CuentaDescuentosId",
            //     table: "Familias",
            //     column: "CuentaDescuentosId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Familias_CuentaDevolucionesId",
            //     table: "Familias",
            //     column: "CuentaDevolucionesId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Familias_CuentaVentasId",
            //     table: "Familias",
            //     column: "CuentaVentasId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Familias_EmpresaId",
            //     table: "Familias",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Impresoras_EmpresaId",
            //     table: "Impresoras",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Impuestos_CuentaContableComprasId",
            //     table: "Impuestos",
            //     column: "CuentaContableComprasId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Impuestos_CuentaContableVentasId",
            //     table: "Impuestos",
            //     column: "CuentaContableVentasId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Impuestos_EmpresaId",
            //     table: "Impuestos",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ItemAlmacenes_AlmacenId",
            //     table: "ItemAlmacenes",
            //     column: "AlmacenId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ItemAlmacenes_ItemId",
            //     table: "ItemAlmacenes",
            //     column: "ItemId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ItemContenedores_ContenedorSuperiorId",
            //     table: "ItemContenedores",
            //     column: "ContenedorSuperiorId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ItemContenedores_ItemId",
            //     table: "ItemContenedores",
            //     column: "ItemId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ItemContenedores_UnidadMedidaId",
            //     table: "ItemContenedores",
            //     column: "UnidadMedidaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ItemProveedores_ItemId",
            //     table: "ItemProveedores",
            //     column: "ItemId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ItemProveedores_ProveedorId",
            //     table: "ItemProveedores",
            //     column: "ProveedorId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ItemProveedores_UnidadMedidaCompraId",
            //     table: "ItemProveedores",
            //     column: "UnidadMedidaCompraId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Items_CategoriaId",
            //     table: "Items",
            //     column: "CategoriaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Items_CuentaAjustesId",
            //     table: "Items",
            //     column: "CuentaAjustesId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Items_CuentaComprasInventariosId",
            //     table: "Items",
            //     column: "CuentaComprasInventariosId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Items_CuentaCostoMateriaPrimaId",
            //     table: "Items",
            //     column: "CuentaCostoMateriaPrimaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Items_CuentaCostoVentasGastosId",
            //     table: "Items",
            //     column: "CuentaCostoVentasGastosId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Items_CuentaDescuentosId",
            //     table: "Items",
            //     column: "CuentaDescuentosId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Items_CuentaDevolucionesId",
            //     table: "Items",
            //     column: "CuentaDevolucionesId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Items_CuentaVentasId",
            //     table: "Items",
            //     column: "CuentaVentasId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Items_ImpuestoId",
            //     table: "Items",
            //     column: "ImpuestoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Items_MarcaId",
            //     table: "Items",
            //     column: "MarcaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Items_UnidadMedidaInventarioId",
            //     table: "Items",
            //     column: "UnidadMedidaInventarioId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ItemTaras_ItemContenedorId",
            //     table: "ItemTaras",
            //     column: "ItemContenedorId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ItemTaras_ItemId",
            //     table: "ItemTaras",
            //     column: "ItemId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Modificadores_GrupoModificadoresId",
            //     table: "Modificadores",
            //     column: "GrupoModificadoresId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Modificadores_ProductoConsumidoId",
            //     table: "Modificadores",
            //     column: "ProductoConsumidoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_MovimientosContables_ContactoId",
            //     table: "MovimientosContables",
            //     column: "ContactoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_MovimientosContables_CuentaContableId",
            //     table: "MovimientosContables",
            //     column: "CuentaContableId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_MovimientosContables_EntradaDiarioId",
            //     table: "MovimientosContables",
            //     column: "EntradaDiarioId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Municipios_ProvinciaId",
            //     table: "Municipios",
            //     column: "ProvinciaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_NumeracionesEntradaDiario_TipoEntradaDiarioId",
            //     table: "NumeracionesEntradaDiario",
            //     column: "TipoEntradaDiarioId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_PaquetesComponentes_ComponenteProductoId",
            //     table: "PaquetesComponentes",
            //     column: "ComponenteProductoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_PaquetesComponentes_ProductoPaqueteId",
            //     table: "PaquetesComponentes",
            //     column: "ProductoPaqueteId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductosModificadoresGrupos_GrupoModificadoresId",
            //     table: "ProductosModificadoresGrupos",
            //     column: "GrupoModificadoresId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductosVenta_CategoriaId",
            //     table: "ProductosVenta",
            //     column: "CategoriaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductosVenta_CuentaAjustesId",
            //     table: "ProductosVenta",
            //     column: "CuentaAjustesId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductosVenta_CuentaComprasInventariosId",
            //     table: "ProductosVenta",
            //     column: "CuentaComprasInventariosId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductosVenta_CuentaCostoMateriaPrimaId",
            //     table: "ProductosVenta",
            //     column: "CuentaCostoMateriaPrimaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductosVenta_CuentaCostoVentasGastosId",
            //     table: "ProductosVenta",
            //     column: "CuentaCostoVentasGastosId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductosVenta_CuentaDescuentosId",
            //     table: "ProductosVenta",
            //     column: "CuentaDescuentosId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductosVenta_CuentaDevolucionesId",
            //     table: "ProductosVenta",
            //     column: "CuentaDevolucionesId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductosVenta_CuentaVentasId",
            //     table: "ProductosVenta",
            //     column: "CuentaVentasId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductosVenta_ImpuestoId",
            //     table: "ProductosVenta",
            //     column: "ImpuestoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductosVenta_ItemContenedorId",
            //     table: "ProductosVenta",
            //     column: "ItemContenedorId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductosVenta_ItemId",
            //     table: "ProductosVenta",
            //     column: "ItemId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductosVenta_PLU",
            //     table: "ProductosVenta",
            //     column: "PLU",
            //     unique: true,
            //     filter: "\"PLU\" IS NOT NULL");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductosVenta_RutaImpresoraId",
            //     table: "ProductosVenta",
            //     column: "RutaImpresoraId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_RecetasIngredientes_IngredienteProductoId",
            //     table: "RecetasIngredientes",
            //     column: "IngredienteProductoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_RecetasIngredientes_ProductoCompuestoId",
            //     table: "RecetasIngredientes",
            //     column: "ProductoCompuestoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_RutasImpresora_EmpresaId",
            //     table: "RutasImpresora",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_SaldosIniciales_ContactoId",
            //     table: "SaldosIniciales",
            //     column: "ContactoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_SaldosIniciales_CuentaContableId",
            //     table: "SaldosIniciales",
            //     column: "CuentaContableId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_SaldosIniciales_EmpresaId",
            //     table: "SaldosIniciales",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_TransaccionesBanco_AsientoContableId",
            //     table: "TransaccionesBanco",
            //     column: "AsientoContableId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_TransaccionesBanco_BancoDestinoId",
            //     table: "TransaccionesBanco",
            //     column: "BancoDestinoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_TransaccionesBanco_BancoId",
            //     table: "TransaccionesBanco",
            //     column: "BancoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_TransaccionesBanco_ConciliacionId",
            //     table: "TransaccionesBanco",
            //     column: "ConciliacionId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_TransaccionesBanco_ContactoId",
            //     table: "TransaccionesBanco",
            //     column: "ContactoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_TransaccionesBanco_CuentaContableDestinoId",
            //     table: "TransaccionesBanco",
            //     column: "CuentaContableDestinoId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_TransaccionesBanco_EmpresaId",
            //     table: "TransaccionesBanco",
            //     column: "EmpresaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_VariantesProducto_PLUVariante",
            //     table: "VariantesProducto",
            //     column: "PLUVariante",
            //     unique: true,
            //     filter: "\"PLUVariante\" IS NOT NULL");

            // migrationBuilder.CreateIndex(
            //     name: "IX_VariantesProducto_ProductoId",
            //     table: "VariantesProducto",
            //     column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //     name: "AjustesConciliacion");

            // migrationBuilder.DropTable(
            //     name: "ComprasDetalles");

            // migrationBuilder.DropTable(
            //     name: "Contenedores");

            // migrationBuilder.DropTable(
            //     name: "DetallesAsientoContable");

            // migrationBuilder.DropTable(
            //     name: "FamiliaCuentasContables");

            // migrationBuilder.DropTable(
            //     name: "Impresoras");

            // migrationBuilder.DropTable(
            //     name: "ItemAlmacenes");

            // migrationBuilder.DropTable(
            //     name: "ItemProveedores");

            // migrationBuilder.DropTable(
            //     name: "ItemTaras");

            // migrationBuilder.DropTable(
            //     name: "Modificadores");

            // migrationBuilder.DropTable(
            //     name: "MovimientosContables");

            // migrationBuilder.DropTable(
            //     name: "PaquetesComponentes");

            // migrationBuilder.DropTable(
            //     name: "ProductosModificadoresGrupos");

            // migrationBuilder.DropTable(
            //     name: "RecetasIngredientes");

            // migrationBuilder.DropTable(
            //     name: "Retenciones");

            // migrationBuilder.DropTable(
            //     name: "SaldosIniciales");

            // migrationBuilder.DropTable(
            //     name: "TiposNcf");

            // migrationBuilder.DropTable(
            //     name: "TransaccionesBanco");

            // migrationBuilder.DropTable(
            //     name: "VariantesProducto");

            // migrationBuilder.DropTable(
            //     name: "Compras");

            // migrationBuilder.DropTable(
            //     name: "GruposModificadores");

            // migrationBuilder.DropTable(
            //     name: "AsientosContables");

            // migrationBuilder.DropTable(
            //     name: "ConciliacionesBancarias");

            // migrationBuilder.DropTable(
            //     name: "Contactos");

            // migrationBuilder.DropTable(
            //     name: "ProductosVenta");

            // migrationBuilder.DropTable(
            //     name: "Almacenes");

            // migrationBuilder.DropTable(
            //     name: "Clientes");

            // migrationBuilder.DropTable(
            //     name: "EntradasDiario");

            // migrationBuilder.DropTable(
            //     name: "Bancos");

            // migrationBuilder.DropTable(
            //     name: "ItemContenedores");

            // migrationBuilder.DropTable(
            //     name: "ComprobantesFiscales");

            // migrationBuilder.DropTable(
            //     name: "ListasPrecios");

            // migrationBuilder.DropTable(
            //     name: "Municipios");

            // migrationBuilder.DropTable(
            //     name: "Paises");

            // migrationBuilder.DropTable(
            //     name: "PlazosPago");

            // migrationBuilder.DropTable(
            //     name: "TiposIdentificacion");

            // migrationBuilder.DropTable(
            //     name: "Vendedores");

            // migrationBuilder.DropTable(
            //     name: "NumeracionesEntradaDiario");

            // migrationBuilder.DropTable(
            //     name: "Items");

            // migrationBuilder.DropTable(
            //     name: "Provincias");

            // migrationBuilder.DropTable(
            //     name: "TiposEntradaDiario");

            // migrationBuilder.DropTable(
            //     name: "Categorias");

            // migrationBuilder.DropTable(
            //     name: "Marcas");

            // migrationBuilder.DropTable(
            //     name: "UnidadesMedida");

            // migrationBuilder.DropTable(
            //     name: "Familias");

            // migrationBuilder.DropTable(
            //     name: "Impuestos");

            // migrationBuilder.DropTable(
            //     name: "RutasImpresora");

            // migrationBuilder.DropTable(
            //     name: "CuentasContables");

            // migrationBuilder.DropTable(
            //     name: "Empresas");
        }
    }
}
