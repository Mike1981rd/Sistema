using Microsoft.EntityFrameworkCore;
using SistemaContable.Models;
using SistemaContable.Models.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaContable.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Contacto> Contactos { get; set; }
        public DbSet<CuentaContable> CuentasContables { get; set; }
        public DbSet<SaldoInicial> SaldosIniciales { get; set; }
        public DbSet<SistemaContable.Models.ComprobanteFiscal> ComprobanteFiscal { get; set; } = null!;
        
        // Banking module DbSets
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<TransaccionBanco> TransaccionesBanco { get; set; }
        public DbSet<ConciliacionBancaria> ConciliacionesBancarias { get; set; }
        public DbSet<AjusteConciliacion> AjustesConciliacion { get; set; }
        public DbSet<AsientoContable> AsientosContables { get; set; }
        public DbSet<DetalleAsientoContable> DetallesAsientoContable { get; set; }
        
        // Impuestos
        public DbSet<Impuesto> Impuestos { get; set; }
        
        // Plazos de Pago
        public DbSet<PlazoPago> PlazosPago { get; set; }
        
        // Retenciones
        public DbSet<Retencion> Retenciones { get; set; }
        
        // Comprobantes Fiscales
        public DbSet<ComprobanteFiscal> ComprobantesFiscales { get; set; }
        
        // Clientes module
        public DbSet<Cliente> Clientes { get; set; }
        
        // Alias para filtrar Clientes que son Proveedores
        public IQueryable<Cliente> Proveedores => Clientes.Where(c => c.EsProveedor);
        
        public DbSet<TipoIdentificacion> TiposIdentificacion { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<TipoNcf> TiposNcf { get; set; }
        public DbSet<ListaPrecio> ListasPrecios { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        
        // Entradas de Diario module
        public DbSet<EntradaDiario> EntradasDiario { get; set; }
        public DbSet<MovimientoContable> MovimientosContables { get; set; }
        public DbSet<TipoEntradaDiario> TiposEntradaDiario { get; set; }
        public DbSet<NumeracionEntradaDiario> NumeracionesEntradaDiario { get; set; }

        // Familias
        public DbSet<Familia> Familias { get; set; }

        // Inventario y modelos relacionados
        public DbSet<Item> Items { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<UnidadMedida> UnidadesMedida { get; set; }
        public DbSet<ItemProveedor> ItemProveedores { get; set; }
        public DbSet<ItemContenedor> ItemContenedores { get; set; }
        public DbSet<ItemTara> ItemTaras { get; set; }
        public DbSet<ItemAlmacen> ItemAlmacenes { get; set; }
        public DbSet<ProductoVenta> ProductosVenta { get; set; }

        // Compras module
        public DbSet<Compra> Compras { get; set; }
        public DbSet<CompraDetalle> ComprasDetalles { get; set; }

        public DbSet<Categoria> Categorias { get; set; }
        #pragma warning disable CS0618 // Suprimir advertencia sobre tipo obsoleto
        public DbSet<FamiliaCuentaContable> FamiliaCuentasContables { get; set; }
        #pragma warning restore CS0618

        // Almacenes
        public DbSet<Almacen> Almacenes { get; set; }
        public DbSet<Impresora> Impresoras { get; set; }
        public DbSet<RutaImpresora> RutasImpresora { get; set; }

        // Agregado para catálogo de contenedores
        public DbSet<Contenedor> Contenedores { get; set; }
        
        // Nuevas entidades para TPV
        public DbSet<VarianteProducto> VariantesProducto { get; set; }
        public DbSet<GrupoModificadores> GruposModificadores { get; set; }
        public DbSet<Modificador> Modificadores { get; set; }
        public DbSet<ProductoModificadorGrupo> ProductosModificadoresGrupos { get; set; }
        public DbSet<RecetaIngrediente> RecetasIngredientes { get; set; }
        public DbSet<PaqueteComponente> PaquetesComponentes { get; set; }
        
        // Roles y permisos
        public DbSet<Rol> Roles { get; set; }
        
        // Usuarios
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración para Familia
            builder.Entity<Familia>(entity =>
            {
                entity.ToTable("Familias");
                entity.HasKey(e => e.Id);
                
                // Propiedades requeridas
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Nota).HasMaxLength(500).IsRequired(false);
                entity.Property(e => e.FechaCreacion).IsRequired();
                entity.Property(e => e.FechaModificacion).IsRequired(false);
                
                // Relación con Empresa
                entity.HasOne(f => f.Empresa)
                      .WithMany()
                      .HasForeignKey(f => f.EmpresaId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Configuración explícita de relaciones con CuentaContable
                entity.HasOne(f => f.CuentaVentas)
                      .WithMany()
                      .HasForeignKey(f => f.CuentaVentasId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(false);
                      
                entity.HasOne(f => f.CuentaComprasInventarios)
                      .WithMany()
                      .HasForeignKey(f => f.CuentaComprasInventariosId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(false);
                      
                entity.HasOne(f => f.CuentaCostoVentasGastos)
                      .WithMany()
                      .HasForeignKey(f => f.CuentaCostoVentasGastosId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(false);
                      
                entity.HasOne(f => f.CuentaDescuentos)
                      .WithMany()
                      .HasForeignKey(f => f.CuentaDescuentosId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(false);
                      
                entity.HasOne(f => f.CuentaDevoluciones)
                      .WithMany()
                      .HasForeignKey(f => f.CuentaDevolucionesId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(false);
                      
                entity.HasOne(f => f.CuentaAjustes)
                      .WithMany()
                      .HasForeignKey(f => f.CuentaAjustesId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(false);
                      
                entity.HasOne(f => f.CuentaCostoMateriaPrima)
                      .WithMany()
                      .HasForeignKey(f => f.CuentaCostoMateriaPrimaId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(false);
            });
            
            // Configuración obsoleta pero necesaria para compatibilidad con migraciones previas
            #pragma warning disable CS0618 // Suprimir advertencia sobre tipo obsoleto
            builder.Entity<FamiliaCuentaContable>(entity =>
            {
                entity.HasKey(e => new { e.FamiliaId, e.CuentaContableId });
                
                entity.HasOne(fc => fc.Familia)
                     .WithMany(f => f.FamiliaCuentasContables)
                     .HasForeignKey(fc => fc.FamiliaId);
                     
                entity.HasOne(fc => fc.CuentaContable)
                     .WithMany(c => c.FamiliaCuentasContables)
                     .HasForeignKey(fc => fc.CuentaContableId);
            });
            #pragma warning restore CS0618

            // NOTA: La siguiente configuración es para evitar la advertencia de EmpresaId1
            // que ocurre debido a una relación inversa no deseada entre Empresa y CuentaContable
            builder.Entity<Empresa>().ToTable("Empresas");
            builder.Entity<Empresa>().Ignore(e => e.CuentasContables);
            
            // Entity type configuration para CuentaContable-Empresa
            builder.Entity<CuentaContable>()
                .HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de CuentaContable
            builder.Entity<CuentaContable>()
                .HasOne(c => c.CuentaPadre)
                .WithMany(c => c.SubCuentas)
                .HasForeignKey(c => c.CuentaPadreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SaldoInicial>()
                .HasOne(s => s.CuentaContable)
                .WithMany()
                .HasForeignKey(s => s.CuentaContableId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SaldoInicial>()
                .HasOne(s => s.Contacto)
                .WithMany()
                .HasForeignKey(s => s.ContactoId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<SaldoInicial>()
                .HasOne(s => s.Empresa)
                .WithMany()
                .HasForeignKey(s => s.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de Contacto
            builder.Entity<Contacto>()
                .HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Configuración de Banco
            builder.Entity<Banco>()
                .HasOne(b => b.Empresa)
                .WithMany()
                .HasForeignKey(b => b.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<Banco>()
                .HasOne(b => b.CuentaContable)
                .WithMany()
                .HasForeignKey(b => b.CuentaContableId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Configuración de TransaccionBanco
            builder.Entity<TransaccionBanco>()
                .HasOne(t => t.Banco)
                .WithMany(b => b.Transacciones)
                .HasForeignKey(t => t.BancoId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<TransaccionBanco>()
                .HasOne(t => t.BancoDestino)
                .WithMany()
                .HasForeignKey(t => t.BancoDestinoId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.Entity<TransaccionBanco>()
                .HasOne(t => t.Conciliacion)
                .WithMany(r => r.TransaccionesConciliadas)
                .HasForeignKey(t => t.ConciliacionId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.Entity<TransaccionBanco>()
                .HasOne(t => t.AsientoContable)
                .WithMany()
                .HasForeignKey(t => t.AsientoContableId)
                .OnDelete(DeleteBehavior.SetNull);
                
            // Configuración de ConciliacionBancaria
            builder.Entity<ConciliacionBancaria>()
                .HasOne(r => r.Banco)
                .WithMany(b => b.Conciliaciones)
                .HasForeignKey(r => r.BancoId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Configuración de AjusteConciliacion
            builder.Entity<AjusteConciliacion>()
                .HasOne(a => a.Conciliacion)
                .WithMany(r => r.Ajustes)
                .HasForeignKey(a => a.ConciliacionId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<AjusteConciliacion>()
                .HasOne(a => a.AsientoContable)
                .WithMany()
                .HasForeignKey(a => a.AsientoContableId)
                .OnDelete(DeleteBehavior.SetNull);
                
            // Configuración de AsientoContable
            builder.Entity<AsientoContable>()
                .HasOne(a => a.Empresa)
                .WithMany()
                .HasForeignKey(a => a.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Configuración de DetalleAsientoContable
            builder.Entity<DetalleAsientoContable>()
                .HasOne(d => d.AsientoContable)
                .WithMany(a => a.Detalles)
                .HasForeignKey(d => d.AsientoContableId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<DetalleAsientoContable>()
                .HasOne(d => d.CuentaContable)
                .WithMany()
                .HasForeignKey(d => d.CuentaContableId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Configuración para la entidad Impuesto
            builder.Entity<Impuesto>(entity =>
            {
                entity.ToTable("Impuestos");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                entity.Property(e => e.Porcentaje).HasPrecision(5, 2);
                
                // Relaciones con CuentaContable
                entity.HasOne(e => e.CuentaContableVentas)
                      .WithMany()
                      .HasForeignKey(e => e.CuentaContableVentasId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.CuentaContableCompras)
                      .WithMany()
                      .HasForeignKey(e => e.CuentaContableComprasId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // Relación con Empresa
                entity.HasOne(e => e.Empresa)
                      .WithMany()
                      .HasForeignKey(e => e.EmpresaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configuración para PlazoPago
            builder.Entity<PlazoPago>(entity =>
            {
                entity.ToTable("PlazosPago");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Dias).IsRequired(false);
                
                // Datos semilla para plazos predeterminados
                entity.HasData(
                    new PlazoPago { Id = 1, Nombre = "De contado", Dias = 0, Estado = true, FechaCreacion = DateTime.UtcNow },
                    new PlazoPago { Id = 2, Nombre = "8 días", Dias = 8, Estado = true, FechaCreacion = DateTime.UtcNow },
                    new PlazoPago { Id = 3, Nombre = "15 días", Dias = 15, Estado = true, FechaCreacion = DateTime.UtcNow },
                    new PlazoPago { Id = 4, Nombre = "30 días", Dias = 30, Estado = true, FechaCreacion = DateTime.UtcNow },
                    new PlazoPago { Id = 5, Nombre = "60 días", Dias = 60, Estado = true, FechaCreacion = DateTime.UtcNow },
                    new PlazoPago { Id = 6, Nombre = "Vencimiento manual", Dias = null, EsVencimientoManual = true, Estado = true, FechaCreacion = DateTime.UtcNow }
                );
            });
            
            // Configuración para Retencion
            builder.Entity<Retencion>(entity =>
            {
                entity.ToTable("Retenciones");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Porcentaje).HasPrecision(5, 2);
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Descripcion).HasMaxLength(255).IsRequired(false);
                entity.Property(e => e.CuentaContableVentas).HasMaxLength(50).IsRequired(false);
                entity.Property(e => e.CuentaContableCompras).HasMaxLength(50).IsRequired(false);
                entity.Property(e => e.CuentaContableRetencionesAsumidas).HasMaxLength(50).IsRequired(false);
                
                // Datos semilla para retenciones predeterminadas
                entity.HasData(
                    new Retencion 
                    { 
                        Id = 1, 
                        Nombre = "ISR 10%", 
                        Porcentaje = 10.00m, 
                        Tipo = "ISR", 
                        Descripcion = "Impuesto Sobre la Renta al 10%",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaModificacion = DateTime.UtcNow
                    },
                    new Retencion 
                    { 
                        Id = 2, 
                        Nombre = "IVA Retenido 15%", 
                        Porcentaje = 15.00m, 
                        Tipo = "IVA", 
                        Descripcion = "Retención del IVA al 15%",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaModificacion = DateTime.UtcNow
                    }
                );
            });
            
            // Configuración para ComprobanteFiscal
            builder.Entity<ComprobanteFiscal>(entity =>
            {
                entity.ToTable("ComprobantesFiscales");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TipoDocumento).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Prefijo).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Sucursal).IsRequired().HasMaxLength(100);
                
                // Defaults
                entity.Property(e => e.NumeroInicial).HasDefaultValue(1);
                entity.Property(e => e.SiguienteNumero).HasDefaultValue(1);
                entity.Property(e => e.Preferida).HasDefaultValue(false);
                entity.Property(e => e.Electronica).HasDefaultValue(false);
            });
            
            // Configuración para Cliente
            builder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Clientes");
                entity.HasKey(e => e.Id);
                
                // Relaciones con entidades relacionadas
                entity.HasOne(c => c.TipoIdentificacion)
                      .WithMany()
                      .HasForeignKey(c => c.TipoIdentificacionId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(c => c.Municipio)
                      .WithMany()
                      .HasForeignKey(c => c.MunicipioId)
                      .OnDelete(DeleteBehavior.SetNull);
                      
                entity.HasOne(c => c.PlazoPago)
                      .WithMany()
                      .HasForeignKey(c => c.PlazoPagoId)
                      .OnDelete(DeleteBehavior.SetNull);
                      
                entity.HasOne(c => c.TipoNcf)
                      .WithMany()
                      .HasForeignKey(c => c.TipoNcfId)
                      .OnDelete(DeleteBehavior.SetNull);
                      
                entity.HasOne(c => c.ListaPrecio)
                      .WithMany()
                      .HasForeignKey(c => c.ListaPrecioId)
                      .OnDelete(DeleteBehavior.SetNull);
                      
                entity.HasOne(c => c.Vendedor)
                      .WithMany()
                      .HasForeignKey(c => c.VendedorId)
                      .OnDelete(DeleteBehavior.SetNull);
                      
                entity.HasOne(c => c.CuentaPorCobrar)
                      .WithMany()
                      .HasForeignKey(c => c.CuentaPorCobrarId)
                      .OnDelete(DeleteBehavior.SetNull);
                      
                entity.HasOne(c => c.CuentaPorPagar)
                      .WithMany()
                      .HasForeignKey(c => c.CuentaPorPagarId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
            
            // Configuración para TipoIdentificacion
            builder.Entity<TipoIdentificacion>(entity =>
            {
                entity.ToTable("TiposIdentificacion");
                entity.HasKey(e => e.Id);
                
                // Seed data
                entity.HasData(
                    new TipoIdentificacion { Id = 1, Nombre = "Cédula", Descripcion = "Cédula de identidad y electoral" },
                    new TipoIdentificacion { Id = 2, Nombre = "RNC", Descripcion = "Registro Nacional del Contribuyente" },
                    new TipoIdentificacion { Id = 3, Nombre = "Pasaporte", Descripcion = "Pasaporte" }
                );
            });
            
            // Configuración para Provincia
            builder.Entity<Provincia>(entity =>
            {
                entity.ToTable("Provincias");
                entity.HasKey(e => e.Id);
                
                // Seed data
                entity.HasData(
                    new Provincia { Id = 1, Nombre = "Santo Domingo" },
                    new Provincia { Id = 2, Nombre = "Santiago" },
                    new Provincia { Id = 3, Nombre = "La Vega" }
                );
            });
            
            // Configuración para Municipio
            builder.Entity<Municipio>(entity =>
            {
                entity.ToTable("Municipios");
                entity.HasKey(e => e.Id);
                
                // Seed data
                entity.HasData(
                    new Municipio { Id = 1, Nombre = "Santo Domingo Este", ProvinciaId = 1 },
                    new Municipio { Id = 2, Nombre = "Santo Domingo Norte", ProvinciaId = 1 },
                    new Municipio { Id = 3, Nombre = "Santiago", ProvinciaId = 2 }
                );
            });
            
            // Configuración para TipoNcf
            builder.Entity<TipoNcf>(entity =>
            {
                entity.ToTable("TiposNcf");
                entity.HasKey(e => e.Id);
                
                // Seed data
                entity.HasData(
                    new TipoNcf { Id = 1, Nombre = "Factura de Crédito Fiscal", Codigo = "B01" },
                    new TipoNcf { Id = 2, Nombre = "Factura de Consumo", Codigo = "B02" },
                    new TipoNcf { Id = 3, Nombre = "Nota de Débito", Codigo = "B03" },
                    new TipoNcf { Id = 4, Nombre = "Nota de Crédito", Codigo = "B04" }
                );
            });
            
            // Configuración para ListaPrecio
            builder.Entity<ListaPrecio>(entity =>
            {
                entity.ToTable("ListasPrecios");
                entity.HasKey(e => e.Id);
                
                // Seed data
                entity.HasData(
                    new ListaPrecio { Id = 1, Nombre = "Lista Regular", Descripcion = "Precios regulares", EsPredeterminada = true, Activa = true },
                    new ListaPrecio { Id = 2, Nombre = "Mayoristas", Descripcion = "Precios para mayoristas", Porcentaje = 10, EsPredeterminada = false, Activa = true },
                    new ListaPrecio { Id = 3, Nombre = "VIP", Descripcion = "Precios para clientes VIP", Porcentaje = 20, EsPredeterminada = false, Activa = true }
                );
            });
            
            // Configuración para Vendedor
            builder.Entity<Vendedor>(entity =>
            {
                entity.ToTable("Vendedores");
                entity.HasKey(e => e.Id);
                
                // Seed data
                entity.HasData(
                    new Vendedor { Id = 1, Nombre = "Juan Pérez", Email = "juan@example.com", Telefono = "809-555-1234", PorcentajeComision = 5, Activo = true },
                    new Vendedor { Id = 2, Nombre = "María González", Email = "maria@example.com", Telefono = "809-555-5678", PorcentajeComision = 7, Activo = true }
                );
            });
            
            // Configuración para Pais
            builder.Entity<Pais>(entity =>
            {
                entity.ToTable("Paises");
                entity.HasKey(e => e.Id);
                
                // Seed data
                entity.HasData(
                    new Pais { Id = 1, Nombre = "República Dominicana", Codigo = "DO", Bandera = "/images/flags/DO.png" },
                    new Pais { Id = 2, Nombre = "Estados Unidos", Codigo = "US", Bandera = "/images/flags/US.png" },
                    new Pais { Id = 3, Nombre = "España", Codigo = "ES", Bandera = "/images/flags/ES.png" },
                    new Pais { Id = 4, Nombre = "México", Codigo = "MX", Bandera = "/images/flags/MX.png" },
                    new Pais { Id = 5, Nombre = "Colombia", Codigo = "CO", Bandera = "/images/flags/CO.png" }
                );
            });

            // Configuraciones para el módulo de Entradas de Diario
            // Relación uno a muchos entre TipoEntradaDiario y EntradaDiario
            builder.Entity<TipoEntradaDiario>(entity => 
            {
                entity.ToTable("TiposEntradaDiario");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                
                // Relación uno a muchos con EntradaDiario
                entity.HasMany(t => t.EntradasDiario)
                      .WithOne(e => e.TipoEntrada)
                      .HasForeignKey(e => e.TipoEntradaId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Relación uno a muchos con NumeracionEntradaDiario
                entity.HasMany(t => t.Numeraciones)
                      .WithOne(n => n.TipoEntradaDiario)
                      .HasForeignKey(n => n.TipoEntradaDiarioId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Datos semilla para TipoEntradaDiario
                entity.HasData(
                    new TipoEntradaDiario { Id = 1, Codigo = "AC", Nombre = "Ajuste contable" },
                    new TipoEntradaDiario { Id = 2, Codigo = "CA", Nombre = "Cierre de periodos contables" },
                    new TipoEntradaDiario { Id = 3, Codigo = "CPC", Nombre = "Cuentas por cobrar" },
                    new TipoEntradaDiario { Id = 4, Codigo = "CPP", Nombre = "Cuentas por pagar" },
                    new TipoEntradaDiario { Id = 5, Codigo = "D", Nombre = "Depreciaciones" },
                    new TipoEntradaDiario { Id = 6, Codigo = "IMP", Nombre = "Impuestos" }
                );
            });

            // Relación uno a muchos entre NumeracionEntradaDiario y EntradaDiario
            builder.Entity<NumeracionEntradaDiario>(entity => 
            {
                entity.ToTable("NumeracionesEntradaDiario");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Prefijo).IsRequired().HasMaxLength(10);
                entity.Property(e => e.NumeroActual).IsRequired();
                
                // Relación uno a muchos con EntradaDiario
                entity.HasMany(n => n.EntradasDiario)
                      .WithOne(e => e.Numeracion)
                      .HasForeignKey(e => e.NumeracionId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Datos semilla para NumeracionEntradaDiario
                entity.HasData(
                    new NumeracionEntradaDiario { Id = 1, TipoEntradaDiarioId = 1, Nombre = "Ajuste contable", Prefijo = "AC", NumeroActual = 1, EsPreferida = true },
                    new NumeracionEntradaDiario { Id = 2, TipoEntradaDiarioId = 2, Nombre = "Cierre contable", Prefijo = "CA", NumeroActual = 1, EsPreferida = true },
                    new NumeracionEntradaDiario { Id = 3, TipoEntradaDiarioId = 3, Nombre = "Cuentas por cobrar", Prefijo = "CPC", NumeroActual = 1, EsPreferida = true },
                    new NumeracionEntradaDiario { Id = 4, TipoEntradaDiarioId = 4, Nombre = "Cuentas por pagar", Prefijo = "CPP", NumeroActual = 1, EsPreferida = true },
                    new NumeracionEntradaDiario { Id = 5, TipoEntradaDiarioId = 5, Nombre = "Depreciaciones", Prefijo = "D", NumeroActual = 1, EsPreferida = true },
                    new NumeracionEntradaDiario { Id = 6, TipoEntradaDiarioId = 6, Nombre = "Impuestos", Prefijo = "IMP", NumeroActual = 1, EsPreferida = true }
                );
            });

            // Configuración para EntradaDiario
            builder.Entity<EntradaDiario>(entity => 
            {
                entity.ToTable("EntradasDiario");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Fecha).IsRequired();
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Observaciones).HasMaxLength(500);
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.FechaCreacion).IsRequired();
                
                // La relación con TipoEntrada ya está configurada en TipoEntradaDiario
                // La relación con Numeracion ya está configurada en NumeracionEntradaDiario
                
                // Relación uno a muchos con MovimientoContable
                entity.HasMany(e => e.Movimientos)
                      .WithOne(m => m.EntradaDiario)
                      .HasForeignKey(m => m.EntradaDiarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para MovimientoContable
            builder.Entity<MovimientoContable>(entity => 
            {
                entity.ToTable("MovimientosContables");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Debito).HasPrecision(18, 2).IsRequired();
                entity.Property(e => e.Credito).HasPrecision(18, 2).IsRequired();
                entity.Property(e => e.TipoContacto).HasMaxLength(1);
                entity.Property(e => e.NumeroDocumento).HasMaxLength(30);
                entity.Property(e => e.Descripcion).HasMaxLength(200);
                
                // La relación con EntradaDiario ya está configurada en EntradaDiario
                
                // Relación con CuentaContable
                entity.HasOne(m => m.CuentaContable)
                      .WithMany()
                      .HasForeignKey(m => m.CuentaContableId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Relación con Contacto (Cliente/Proveedor)
                entity.HasOne(m => m.Contacto)
                      .WithMany()
                      .HasForeignKey(m => m.ContactoId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configuración para Categoria
            builder.Entity<Categoria>(entity =>
            {
                entity.ToTable("Categorias");
                entity.HasKey(e => e.Id);
                
                // Relación con Empresa
                entity.HasOne(f => f.Empresa)
                      .WithMany()
                      .HasForeignKey(f => f.EmpresaId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Relación con Familia
                entity.HasOne(f => f.Familia)
                      .WithMany()
                      .HasForeignKey(f => f.FamiliaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Almacen
            builder.Entity<Almacen>(entity =>
            {
                entity.ToTable("Almacenes");
                entity.HasKey(e => e.Id);
                
                // Relación con Empresa
                entity.HasOne(a => a.Empresa)
                      .WithMany()
                      .HasForeignKey(a => a.EmpresaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para Impresora
            builder.Entity<Impresora>(entity =>
            {
                entity.ToTable("Impresoras");
                entity.HasKey(e => e.Id);
                
                // Relación con Empresa
                entity.HasOne(i => i.Empresa)
                      .WithMany()
                      .HasForeignKey(i => i.EmpresaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para RutaImpresora
            builder.Entity<RutaImpresora>(entity =>
            {
                entity.ToTable("RutasImpresora");
                entity.HasKey(e => e.Id);
                
                // Relación con Empresa
                entity.HasOne(r => r.Empresa)
                      .WithMany()
                      .HasForeignKey(r => r.EmpresaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para la entidad Compra
            builder.Entity<Compra>(entity =>
            {
                entity.ToTable("Compras");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Numero).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Fecha).IsRequired();
                entity.Property(e => e.Referencia).HasMaxLength(50).IsRequired(false);
                entity.Property(e => e.Observaciones).HasMaxLength(500).IsRequired(false);
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(20);
                
                // Relación con Proveedor (Cliente)
                entity.HasOne(c => c.Proveedor)
                      .WithMany()
                      .HasForeignKey(c => c.ProveedorId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // Relación con Almacén
                entity.HasOne(c => c.Almacen)
                      .WithMany()
                      .HasForeignKey(c => c.AlmacenId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(false);
                      
                // Relación con PlazoPago
                entity.HasOne(c => c.PlazoPago)
                      .WithMany()
                      .HasForeignKey(c => c.PlazoPagoId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(false);
                      
                // Relación con EntradaDiario
                entity.HasOne(c => c.EntradaDiario)
                      .WithMany()
                      .HasForeignKey(c => c.EntradaDiarioId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(false);
                      
                // Relación con Empresa
                entity.HasOne(c => c.Empresa)
                      .WithMany()
                      .HasForeignKey(c => c.EmpresaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configuración para la entidad CompraDetalle
            builder.Entity<CompraDetalle>(entity =>
            {
                entity.ToTable("ComprasDetalles");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Descripcion).HasMaxLength(150).IsRequired(false);
                entity.Property(e => e.Cantidad).HasPrecision(18, 4).IsRequired();
                entity.Property(e => e.Precio).HasPrecision(18, 4).IsRequired();
                entity.Property(e => e.Subtotal).HasPrecision(18, 4);
                entity.Property(e => e.PorcentajeDescuento).HasPrecision(18, 4);
                entity.Property(e => e.MontoDescuento).HasPrecision(18, 4);
                entity.Property(e => e.MontoImpuesto).HasPrecision(18, 4);
                entity.Property(e => e.Total).HasPrecision(18, 4);
                entity.Property(e => e.FactorConversion).HasPrecision(18, 6);
                
                // Relación con Compra
                entity.HasOne(d => d.Compra)
                      .WithMany(c => c.Detalles)
                      .HasForeignKey(d => d.CompraId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Relación con Item
                entity.HasOne(d => d.Item)
                      .WithMany()
                      .HasForeignKey(d => d.ItemId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // Relación con Impuesto
                entity.HasOne(d => d.Impuesto)
                      .WithMany()
                      .HasForeignKey(d => d.ImpuestoId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(false);
                      
                // Relación con UnidadMedida
                entity.HasOne(d => d.UnidadMedida)
                      .WithMany()
                      .HasForeignKey(d => d.UnidadMedidaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            
            // Configuración para ProductoVenta
            builder.Entity<ProductoVenta>(entity =>
            {
                entity.ToTable("ProductosVenta");
                entity.HasKey(e => e.Id);
                
                // Configuración de precisión para decimales
                entity.Property(e => e.PrecioVenta).HasPrecision(18, 2);
                entity.Property(e => e.Costo).HasPrecision(18, 2);
                entity.Property(e => e.Cantidad).HasPrecision(18, 4);
                entity.Property(e => e.CostoTotal).HasPrecision(18, 4);
                
                // Índice único para PLU
                entity.HasIndex(p => p.PLU)
                      .IsUnique()
                      .HasFilter("\"PLU\" IS NOT NULL");
                      
                // Relaciones
                entity.HasOne(p => p.Categoria)
                      .WithMany()
                      .HasForeignKey(p => p.CategoriaId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(p => p.Impuesto)
                      .WithMany()
                      .HasForeignKey(p => p.ImpuestoId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .IsRequired(false);
                      
                entity.HasOne(p => p.RutaImpresora)
                      .WithMany()
                      .HasForeignKey(p => p.RutaImpresoraId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .IsRequired(false);
                      
                // Relación uno a muchos con VarianteProducto
                entity.HasMany(p => p.Variantes)
                      .WithOne(v => v.ProductoPadre)
                      .HasForeignKey(v => v.ProductoId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Relación muchos a muchos con GrupoModificadores a través de ProductoModificadorGrupo
                entity.HasMany(p => p.ProductoModificadorGrupos)
                      .WithOne(pmg => pmg.Producto)
                      .HasForeignKey(pmg => pmg.ProductoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configuración para VarianteProducto
            builder.Entity<VarianteProducto>(entity =>
            {
                entity.ToTable("VariantesProducto");
                entity.HasKey(e => e.Id);
                
                // Configuración de precisión para decimales
                entity.Property(e => e.PrecioAdicionalOAbsoluto).HasPrecision(18, 2);
                
                // Índice único para PLUVariante
                entity.HasIndex(v => v.PLUVariante)
                      .IsUnique()
                      .HasFilter("\"PLUVariante\" IS NOT NULL");
                      
                // Convertir enum a string en la base de datos
                entity.Property(e => e.AjustePrecioTipo)
                      .HasConversion<string>();
                      
                entity.HasOne(v => v.ProductoPadre)
                      .WithMany(p => p.Variantes)
                      .HasForeignKey(v => v.ProductoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configuración para GrupoModificadores
            builder.Entity<GrupoModificadores>(entity =>
            {
                entity.ToTable("GruposModificadores");
                entity.HasKey(e => e.Id);
                
                // Convertir enum a string en la base de datos
                entity.Property(e => e.TipoVisualizacionTPV)
                      .HasConversion<string>();
                      
                entity.HasMany(g => g.Modificadores)
                      .WithOne(m => m.GrupoModificadores)
                      .HasForeignKey(m => m.GrupoModificadoresId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasMany(g => g.ProductoModificadorGrupos)
                      .WithOne(pmg => pmg.GrupoModificadores)
                      .HasForeignKey(pmg => pmg.GrupoModificadoresId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configuración para Modificador
            builder.Entity<Modificador>(entity =>
            {
                entity.ToTable("Modificadores");
                entity.HasKey(e => e.Id);
                
                // Configuración de precisión para decimales
                entity.Property(e => e.PrecioAdicional).HasPrecision(18, 2);
                entity.Property(e => e.CantidadConsumida).HasPrecision(18, 4);
                
                entity.HasOne(m => m.GrupoModificadores)
                      .WithMany(g => g.Modificadores)
                      .HasForeignKey(m => m.GrupoModificadoresId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(m => m.ProductoConsumido)
                      .WithMany()
                      .HasForeignKey(m => m.ProductoConsumidoId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(false);
            });
            
            // Configuración para ProductoModificadorGrupo (relación muchos a muchos)
            builder.Entity<ProductoModificadorGrupo>(entity =>
            {
                entity.ToTable("ProductosModificadoresGrupos");
                entity.HasKey(pmg => new { pmg.ProductoId, pmg.GrupoModificadoresId });
                
                entity.HasOne(pmg => pmg.Producto)
                      .WithMany(p => p.ProductoModificadorGrupos)
                      .HasForeignKey(pmg => pmg.ProductoId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(pmg => pmg.GrupoModificadores)
                      .WithMany(g => g.ProductoModificadorGrupos)
                      .HasForeignKey(pmg => pmg.GrupoModificadoresId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configuración para RecetaIngrediente
            builder.Entity<RecetaIngrediente>(entity =>
            {
                entity.ToTable("RecetasIngredientes");
                entity.HasKey(e => e.Id);
                
                // Configuración de precisión para decimales
                entity.Property(e => e.Cantidad).HasPrecision(18, 4);
                
                entity.HasOne(ri => ri.ProductoCompuesto)
                      .WithMany(p => p.IngredientesDeEsteProducto)
                      .HasForeignKey(ri => ri.ProductoCompuestoId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(ri => ri.IngredienteProducto)
                      .WithMany(p => p.ApareceComoIngredienteEn)
                      .HasForeignKey(ri => ri.IngredienteProductoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            
            // Configuración para PaqueteComponente
            builder.Entity<PaqueteComponente>(entity =>
            {
                entity.ToTable("PaquetesComponentes");
                entity.HasKey(e => e.Id);
                
                // Configuración de precisión para decimales
                entity.Property(e => e.PrecioComponenteEnPaquete).HasPrecision(18, 2);
                
                entity.HasOne(pc => pc.ProductoPaquete)
                      .WithMany(p => p.ComponentesDeEstePaquete)
                      .HasForeignKey(pc => pc.ProductoPaqueteId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(pc => pc.ComponenteProducto)
                      .WithMany(p => p.ApareceComoComponenteEn)
                      .HasForeignKey(pc => pc.ComponenteProductoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            
            // Configuración para Rol
            builder.Entity<Rol>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.Id);
                
                // Propiedades
                entity.Property(e => e.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);
                      
                entity.Property(e => e.Descripcion)
                      .HasMaxLength(500);
                      
                entity.Property(e => e.Prioridad)
                      .HasDefaultValue(0);
                      
                // Configuración para lista de permisos como JSONB
                entity.Property(e => e.Permisos)
                      .HasColumnType("jsonb");
                      
                // Índice único para Nombre por EmpresaId
                entity.HasIndex(e => new { e.Nombre, e.EmpresaId })
                      .IsUnique();
                      
                // Relación con Empresa
                entity.HasOne(e => e.Empresa)
                      .WithMany()
                      .HasForeignKey(e => e.EmpresaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configuración para Usuario
            builder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(e => e.Id);
                
                // Propiedades requeridas
                entity.Property(e => e.NombreCompleto).IsRequired().HasMaxLength(100);
                entity.Property(e => e.NombreUsuario).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(256); // Requerido en BD pero no en modelo
                entity.Property(e => e.EmpresaId).IsRequired();
                entity.Property(e => e.FechaCreacion).IsRequired();
                entity.Property(e => e.Activo).HasDefaultValue(true);
                
                // Propiedades opcionales
                entity.Property(e => e.PinPOS).HasMaxLength(4);
                entity.Property(e => e.Telefono).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Direccion).HasMaxLength(200);
                entity.Property(e => e.Ciudad).HasMaxLength(100);
                entity.Property(e => e.EstadoProvincia).HasMaxLength(100);
                entity.Property(e => e.CodigoPostal).HasMaxLength(20);
                entity.Property(e => e.FotoUrl).HasMaxLength(500);
                
                // Índice único por empresa
                entity.HasIndex(e => new { e.NombreUsuario, e.EmpresaId }).IsUnique();
                
                // Relaciones
                entity.HasOne(u => u.Empresa)
                    .WithMany()
                    .HasForeignKey(u => u.EmpresaId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(u => u.Rol)
                    .WithMany(r => r.Usuarios)
                    .HasForeignKey(u => u.RolId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConvertDatesToUtc()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                var dateProperties = entityEntry.Entity.GetType().GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

                foreach (var property in dateProperties)
                {
                    if (property.PropertyType == typeof(DateTime))
                    {
                        var rawValue = property.GetValue(entityEntry.Entity);
                        if (rawValue != null)
                        {
                            var value = (DateTime)rawValue;
                            if (value.Kind != DateTimeKind.Utc)
                            {
                                property.SetValue(entityEntry.Entity, DateTime.SpecifyKind(value, DateTimeKind.Utc));
                            }
                        }
                    }
                    else if (property.PropertyType == typeof(DateTime?))
                    {
                        var value = (DateTime?)property.GetValue(entityEntry.Entity);
                        if (value.HasValue && value.Value.Kind != DateTimeKind.Utc)
                        {
                            property.SetValue(entityEntry.Entity, DateTime.SpecifyKind(value.Value, DateTimeKind.Utc));
                        }
                    }
                }
            }
        }

        public override int SaveChanges()
        {
            ConvertDatesToUtc();
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && e.State is EntityState.Added or EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.FechaCreacion = DateTime.UtcNow;
                }
                else
                {
                    entityEntry.Property("FechaCreacion").IsModified = false;
                }

                entity.FechaModificacion = DateTime.UtcNow;
            }

            // Manejo específico para entidades que no heredan de BaseEntity
            var otherEntries = ChangeTracker
                .Entries()
                .Where(e => !(e.Entity is BaseEntity) && e.State is EntityState.Added or EntityState.Modified);

            foreach (var entityEntry in otherEntries)
            {
                if (entityEntry.Entity is Familia familia)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        familia.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    familia.FechaModificacion = DateTime.UtcNow;
                }
                else if (entityEntry.Entity is Categoria categoria)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        categoria.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    categoria.FechaModificacion = DateTime.UtcNow;
                }
                else if (entityEntry.Entity is Almacen almacen)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        almacen.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    almacen.FechaModificacion = DateTime.UtcNow;
                }
                else if (entityEntry.Entity is Impresora impresora)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        impresora.FechaCreacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    impresora.FechaModificacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                }
                else if (entityEntry.Entity is RutaImpresora rutaImpresora)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        rutaImpresora.FechaCreacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    rutaImpresora.FechaModificacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                }
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ConvertDatesToUtc();
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && e.State is EntityState.Added or EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.FechaCreacion = DateTime.UtcNow;
                }
                else
                {
                    entityEntry.Property("FechaCreacion").IsModified = false;
                }

                entity.FechaModificacion = DateTime.UtcNow;
            }

            // Manejo específico para entidades que no heredan de BaseEntity
            var otherEntries = ChangeTracker
                .Entries()
                .Where(e => !(e.Entity is BaseEntity) && e.State is EntityState.Added or EntityState.Modified);

            foreach (var entityEntry in otherEntries)
            {
                if (entityEntry.Entity is Familia familia)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        familia.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    familia.FechaModificacion = DateTime.UtcNow;
                }
                else if (entityEntry.Entity is Categoria categoria)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        categoria.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    categoria.FechaModificacion = DateTime.UtcNow;
                }
                else if (entityEntry.Entity is Almacen almacen)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        almacen.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    almacen.FechaModificacion = DateTime.UtcNow;
                }
                else if (entityEntry.Entity is Impresora impresora)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        impresora.FechaCreacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    impresora.FechaModificacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                }
                else if (entityEntry.Entity is RutaImpresora rutaImpresora)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        rutaImpresora.FechaCreacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    rutaImpresora.FechaModificacion = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
} 