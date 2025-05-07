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

        // Aquí irán los DbSets para tus entidades
        // Ejemplo: public DbSet<Cliente> Clientes { get; set; }

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