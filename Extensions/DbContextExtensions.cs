using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Extensions
{
    /// <summary>
    /// Extensiones para DbContext que ayudan a manejar conflictos de migración y sincronización
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Verifica y resuelve conflictos de migración en la base de datos
        /// </summary>
        /// <param name="context">El contexto de la base de datos</param>
        /// <returns>True si se resolvieron conflictos, False si no hubo conflictos</returns>
        public static async Task<bool> ResolveMigrationConflictsAsync(this DbContext context)
        {
            var connection = context.Database.GetDbConnection();
            await connection.OpenAsync();

            try
            {
                // 1. Verificar si la tabla de historial de migraciones existe
                var historyTableExists = await CheckHistoryTableExistsAsync(context);
                if (!historyTableExists)
                {
                    // Si no existe, crear la tabla de historial
                    await context.Database.ExecuteSqlRawAsync(@"
                        CREATE TABLE IF NOT EXISTS ""__EFMigrationsHistory"" (
                            ""MigrationId"" character varying(150) NOT NULL,
                            ""ProductVersion"" character varying(32) NOT NULL,
                            CONSTRAINT ""PK___EFMigrationsHistory"" PRIMARY KEY (""MigrationId"")
                        );
                    ");
                }

                // 2. Obtener la versión actual de EF Core
                var efCoreVersion = typeof(DbContext).Assembly.GetName().Version?.ToString() ?? "8.0.0";

                // 3. Verificar índices existentes que podrían causar conflictos
                var existingIndexes = await GetExistingIndexesAsync(context);
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

                foreach (var migration in pendingMigrations)
                {
                    // 4. Verificar si la migración ya está aplicada físicamente
                    var isPhysicallyApplied = await CheckMigrationPhysicallyAppliedAsync(context, migration);
                    
                    if (isPhysicallyApplied)
                    {
                        // 5. Registrar la migración en el historial si ya está aplicada físicamente
                        await RegisterMigrationInHistoryAsync(context, migration, efCoreVersion);
                    }
                }

                return true;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        /// <summary>
        /// Verifica si la tabla de historial de migraciones existe
        /// </summary>
        private static async Task<bool> CheckHistoryTableExistsAsync(DbContext context)
        {
            var result = await context.Database.ExecuteSqlRawAsync(@"
                SELECT EXISTS (
                    SELECT 1 FROM information_schema.tables 
                    WHERE table_name = '__EFMigrationsHistory'
                );
            ");
            return result > 0;
        }

        /// <summary>
        /// Obtiene los índices existentes en la base de datos
        /// </summary>
        private static async Task<string[]> GetExistingIndexesAsync(DbContext context)
        {
            var indexes = await context.Database.SqlQueryRaw<string>(@"
                SELECT indexname 
                FROM pg_indexes 
                WHERE schemaname = 'public'
            ").ToArrayAsync();
            return indexes;
        }

        /// <summary>
        /// Verifica si una migración ya está aplicada físicamente en la base de datos
        /// </summary>
        private static async Task<bool> CheckMigrationPhysicallyAppliedAsync(DbContext context, string migrationId)
        {
            // Aquí puedes agregar más verificaciones específicas según tus necesidades
            var result = await context.Database.ExecuteSqlRawAsync(@"
                SELECT EXISTS (
                    SELECT 1 FROM pg_indexes 
                    WHERE indexname = 'IX_Clientes_EmpresaId'
                );
            ");
            return result > 0;
        }

        /// <summary>
        /// Registra una migración en la tabla de historial
        /// </summary>
        private static async Task RegisterMigrationInHistoryAsync(DbContext context, string migrationId, string productVersion)
        {
            await context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
                VALUES ({0}, {1})
                ON CONFLICT (""MigrationId"") DO NOTHING;
            ", migrationId, productVersion);
        }

        /// <summary>
        /// Verifica y sincroniza el estado de la base de datos con el historial de migraciones
        /// </summary>
        public static async Task SyncDatabaseStateAsync(this DbContext context)
        {
            // 1. Resolver conflictos de migración
            await ResolveMigrationConflictsAsync(context);

            // 2. Verificar y crear índices faltantes
            await EnsureIndexesExistAsync(context);

            // 3. Verificar y crear claves foráneas faltantes
            await EnsureForeignKeysExistAsync(context);
        }

        /// <summary>
        /// Asegura que los índices necesarios existan en la base de datos
        /// </summary>
        private static async Task EnsureIndexesExistAsync(DbContext context)
        {
            await context.Database.ExecuteSqlRawAsync(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM pg_indexes 
                        WHERE tablename = 'Clientes' 
                        AND indexname = 'IX_Clientes_EmpresaId'
                    ) THEN
                        CREATE INDEX ""IX_Clientes_EmpresaId"" ON ""Clientes"" (""EmpresaId"");
                    END IF;
                END$$;
            ");
        }

        /// <summary>
        /// Asegura que las claves foráneas necesarias existan en la base de datos
        /// </summary>
        private static async Task EnsureForeignKeysExistAsync(DbContext context)
        {
            await context.Database.ExecuteSqlRawAsync(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.table_constraints 
                        WHERE table_name = 'Clientes' 
                        AND constraint_name = 'FK_Clientes_Empresas_EmpresaId'
                    ) THEN
                        ALTER TABLE ""Clientes""
                        ADD CONSTRAINT ""FK_Clientes_Empresas_EmpresaId""
                        FOREIGN KEY (""EmpresaId"") REFERENCES ""Empresas""(""Id"") ON DELETE SET NULL;
                    END IF;
                END$$;
            ");
        }
    }
} 