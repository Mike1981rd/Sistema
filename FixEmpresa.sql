-- Script para resolver problemas con la tabla Empresas y el error 404
-- Ejecutar este script directamente en PostgreSQL

-- 1. Terminar todas las conexiones activas a la base de datos
SELECT pg_terminate_backend(pg_stat_activity.pid)
FROM pg_stat_activity
WHERE pg_stat_activity.datname = 'SistemaContable'
AND pid <> pg_backend_pid();

-- 2. Verificar si la tabla Empresas existe
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Empresas') THEN
        RAISE NOTICE 'La tabla Empresas no existe. Asegúrate de que las migraciones se hayan aplicado correctamente.';
    ELSE
        RAISE NOTICE 'La tabla Empresas existe.';
    END IF;
END $$;

-- 3. Verificar si hay empresas registradas
SELECT COUNT(*) AS cantidad_empresas FROM "Empresas";

-- 4. Insertar una empresa predeterminada si no hay ninguna
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Empresas') AND
       NOT EXISTS (SELECT 1 FROM "Empresas" LIMIT 1) THEN
        
        INSERT INTO "Empresas" (
            "Nombre", "NumeroIdentificacion", "TipoIdentificacion", 
            "Direccion", "Ciudad", "Provincia", "CodigoPostal", 
            "Pais", "Telefono", "Email", "SitioWeb", 
            "NombreComercial", "MonedaPrincipal", "NumeroEmpleados", 
            "PrecisionDecimal", "SeparadorDecimal", "LogoUrl", 
            "ResponsabilidadTributaria", "FechaCreacion", "Activo"
        ) VALUES (
            'Empresa Predeterminada', '000-0000000-0', 'RNC',
            'Dirección Predeterminada', 'Ciudad', 'Provincia', '00000',
            'República Dominicana', '000-000-0000', 'info@empresa.com', 'www.empresa.com',
            'Empresa Demo', 'DOP', 5,
            2, '.', '/images/logo.png',
            'Persona Jurídica', NOW(), TRUE
        );
        
        RAISE NOTICE 'Se ha insertado una empresa predeterminada.';
    ELSE
        RAISE NOTICE 'Ya existe al menos una empresa o la tabla no existe.';
    END IF;
END $$;

-- 5. Mostrar las empresas existentes
SELECT * FROM "Empresas";

-- 6. Verificar las tablas importantes para la aplicación
SELECT table_name, table_type
FROM information_schema.tables
WHERE table_schema = 'public'
ORDER BY table_name;

-- Mensaje final
DO $$
BEGIN
    RAISE NOTICE 'Script de corrección completado. Revisa los resultados anteriores.';
END $$;