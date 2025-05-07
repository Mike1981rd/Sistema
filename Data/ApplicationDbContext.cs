using Microsoft.EntityFrameworkCore;
using SistemaContable.Models;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Empresa>().ToTable("Empresas");

            // Configuración de CuentaContable
            builder.Entity<CuentaContable>()
                .HasOne(c => c.CuentaPadre)
                .WithMany(c => c.SubCuentas)
                .HasForeignKey(c => c.CuentaPadreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CuentaContable>()
                .HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de SaldoInicial
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
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => (e.Entity is CuentaContable || e.Entity is Contacto) && 
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
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ConvertDatesToUtc();
            
            var entries = ChangeTracker.Entries()
                .Where(e => (e.Entity is CuentaContable || e.Entity is Contacto) && 
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