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
        public DbSet<TipoIdentificacion> TiposIdentificacion { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<TipoNcf> TiposNcf { get; set; }
        public DbSet<ListaPrecio> ListasPrecios { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
                    new PlazoPago { Id = 1, Nombre = "De contado", Dias = 0, FechaCreacion = DateTime.UtcNow },
                    new PlazoPago { Id = 2, Nombre = "8 días", Dias = 8, FechaCreacion = DateTime.UtcNow },
                    new PlazoPago { Id = 3, Nombre = "15 días", Dias = 15, FechaCreacion = DateTime.UtcNow },
                    new PlazoPago { Id = 4, Nombre = "30 días", Dias = 30, FechaCreacion = DateTime.UtcNow },
                    new PlazoPago { Id = 5, Nombre = "60 días", Dias = 60, FechaCreacion = DateTime.UtcNow },
                    new PlazoPago { Id = 6, Nombre = "Vencimiento manual", Dias = null, EsVencimientoManual = true, FechaCreacion = DateTime.UtcNow }
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
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => (e.Entity is CuentaContable || e.Entity is Contacto || e.Entity is Impuesto || 
                            e.Entity is PlazoPago || e.Entity is Retencion || e.Entity is ComprobanteFiscal) && 
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.Entity is CuentaContable cuentaContable)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        cuentaContable.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    cuentaContable.FechaModificacion = DateTime.UtcNow;
                }
                else if (entityEntry.Entity is Contacto contacto)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        contacto.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    contacto.FechaModificacion = DateTime.UtcNow;
                }
                else if (entityEntry.Entity is Impuesto impuesto)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        impuesto.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    impuesto.FechaModificacion = DateTime.UtcNow;
                }
                else if (entityEntry.Entity is PlazoPago plazoPago)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        plazoPago.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    plazoPago.FechaModificacion = DateTime.UtcNow;
                }
                else if (entityEntry.Entity is Retencion retencion)
                {
                    // Manejo de fechas para Retenciones
                    if (entityEntry.State == EntityState.Added)
                    {
                        // Solo establecer fecha de creación para entidades nuevas
                        if (!entityEntry.Property("FechaCreacion").IsModified)
                        {
                            retencion.FechaCreacion = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        // No modificar la fecha de creación para entidades existentes
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    retencion.FechaModificacion = DateTime.UtcNow;
                }
                else if (entityEntry.Entity is ComprobanteFiscal comprobanteFiscal)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        comprobanteFiscal.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    comprobanteFiscal.UltimaModificacion = DateTime.UtcNow;
                }
            }

            // Convertir las fechas a UTC
            ConvertDatesToUtc();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ConvertDatesToUtc();
            
            var entries = ChangeTracker.Entries()
                .Where(e => (e.Entity is CuentaContable || e.Entity is Contacto || e.Entity is Impuesto || e.Entity is PlazoPago) && 
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.Entity is CuentaContable cuentaContable)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        cuentaContable.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    cuentaContable.FechaModificacion = DateTime.UtcNow;
                }
                else if (entityEntry.Entity is Contacto contacto)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        contacto.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    contacto.FechaModificacion = DateTime.UtcNow;
                }
                else if (entityEntry.Entity is Impuesto impuesto)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        impuesto.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    impuesto.FechaModificacion = DateTime.UtcNow;
                }
                else if (entityEntry.Entity is PlazoPago plazoPago)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        plazoPago.FechaCreacion = DateTime.UtcNow;
                    }
                    else
                    {
                        entityEntry.Property("FechaCreacion").IsModified = false;
                    }
                    plazoPago.FechaModificacion = DateTime.UtcNow;
                }
            }
            
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ConvertDatesToUtc()
        {
            var entities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entity in entities)
            {
                var dateProperties = entity.Entity.GetType().GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

                foreach (var property in dateProperties)
                {
                    if (property.PropertyType == typeof(DateTime))
                    {
                        var rawValue = property.GetValue(entity.Entity);
                        if (rawValue != null)
                        {
                            var value = (DateTime)rawValue;
                            if (value.Kind != DateTimeKind.Utc)
                            {
                                property.SetValue(entity.Entity, DateTime.SpecifyKind(value, DateTimeKind.Utc));
                            }
                        }
                    }
                    else if (property.PropertyType == typeof(DateTime?))
                    {
                        var value = (DateTime?)property.GetValue(entity.Entity);
                        if (value.HasValue && value.Value.Kind != DateTimeKind.Utc)
                        {
                            property.SetValue(entity.Entity, DateTime.SpecifyKind(value.Value, DateTimeKind.Utc));
                        }
                    }
                }
            }
        }
    }
} 