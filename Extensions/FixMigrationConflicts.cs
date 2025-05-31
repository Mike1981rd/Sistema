using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaContable.Data;

namespace SistemaContable.Extensions
{
    /// <summary>
    /// Extensiones para manejar conflictos de migración en Entity Framework Core con PostgreSQL
    /// </summary>
    public static class MigrationExtensions
    {
        /// <summary>
        /// Arregla conflictos de migración verificando índices existentes y actualizando el historial
        /// </summary>
        public static void FixMigrationConflicts(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                using var scope = app.ApplicationServices.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                FixExistingIndexes(context).GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// Arregla los índices existentes y actualiza el historial de migraciones
        /// </summary>
        private static async Task FixExistingIndexes(ApplicationDbContext context)
        {
            using var conn = context.Database.GetDbConnection() as NpgsqlConnection;
            await conn.OpenAsync();

            try
            {
                // 1. Verificar si la tabla de historial existe
                await EnsureHistoryTableExistsAsync(context);

                // 2. Obtener índices existentes
                var existingIndexes = await GetExistingIndexesAsync(conn);

                // 3. Obtener migraciones pendientes
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

                // 4. Filtrar migraciones que crean índices que ya existen
                var migrationsToRegister = await FilterMigrationsWithExistingIndexesAsync(
                    context, pendingMigrations, existingIndexes);

                // 5. Registrar migraciones como aplicadas
                if (migrationsToRegister.Any())
                {
                    await RegisterMigrationsAsAppliedAsync(context, migrationsToRegister);
                }
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        /// <summary>
        /// Asegura que la tabla de historial de migraciones exista
        /// </summary>
        private static async Task EnsureHistoryTableExistsAsync(ApplicationDbContext context)
        {
            await context.Database.ExecuteSqlRawAsync(@"
                CREATE TABLE IF NOT EXISTS ""__EFMigrationsHistory"" (
                    ""MigrationId"" character varying(150) NOT NULL,
                    ""ProductVersion"" character varying(32) NOT NULL,
                    CONSTRAINT ""PK___EFMigrationsHistory"" PRIMARY KEY (""MigrationId"")
                );
            ");
        }

        /// <summary>
        /// Obtiene los índices existentes en la base de datos
        /// </summary>
        private static async Task<HashSet<string>> GetExistingIndexesAsync(NpgsqlConnection conn)
        {
            var indexes = new HashSet<string>();
            using var cmd = new NpgsqlCommand(@"
                SELECT indexname 
                FROM pg_indexes 
                WHERE schemaname = 'public'
            ", conn);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                indexes.Add(reader.GetString(0));
            }

            return indexes;
        }

        /// <summary>
        /// Filtra las migraciones pendientes que crean índices que ya existen
        /// </summary>
        private static async Task<List<string>> FilterMigrationsWithExistingIndexesAsync(
            ApplicationDbContext context,
            IEnumerable<string> pendingMigrations,
            HashSet<string> existingIndexes)
        {
            var migrationsToRegister = new List<string>();
            var efCoreVersion = typeof(DbContext).Assembly.GetName().Version?.ToString() ?? "8.0.0";

            foreach (var migration in pendingMigrations)
            {
                // Verificar si la migración crea índices que ya existen
                var shouldRegister = await ShouldRegisterMigrationAsync(context, migration, existingIndexes);
                if (shouldRegister)
                {
                    migrationsToRegister.Add(migration);
                }
            }

            return migrationsToRegister;
        }

        /// <summary>
        /// Determina si una migración debe registrarse como aplicada
        /// </summary>
        private static async Task<bool> ShouldRegisterMigrationAsync(
            ApplicationDbContext context,
            string migrationId,
            HashSet<string> existingIndexes)
        {
            // Verificar índices específicos según la migración
            if (migrationId.Contains("SkipExistingIndexes") || 
                migrationId.Contains("FixClientesEmpresaId"))
            {
                return existingIndexes.Contains("IX_Clientes_EmpresaId");
            }

            // Agregar más condiciones según sea necesario para otras migraciones
            return false;
        }

        /// <summary>
        /// Registra las migraciones como aplicadas en el historial
        /// </summary>
        private static async Task RegisterMigrationsAsAppliedAsync(
            ApplicationDbContext context,
            List<string> migrations)
        {
            var efCoreVersion = typeof(DbContext).Assembly.GetName().Version?.ToString() ?? "8.0.0";

            foreach (var migration in migrations)
            {
                await context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
                    VALUES ({0}, {1})
                    ON CONFLICT (""MigrationId"") DO NOTHING;
                ", migration, efCoreVersion);
            }
        }
    }
} 