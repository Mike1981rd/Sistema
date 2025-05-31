-- Script para verificar y corregir problemas en la base de datos
-- Ejecutar este script para solucionar el error 404 persistente

-- 1. Verificar si existe la tabla Empresas y crear una empresa predeterminada si no hay ninguna
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Empresas') THEN
        -- Verificar si hay alguna empresa registrada
        IF NOT EXISTS (SELECT 1 FROM "Empresas" LIMIT 1) THEN
            -- Insertar empresa por defecto
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
            
            RAISE NOTICE 'Empresa predeterminada creada con éxito';
        ELSE
            RAISE NOTICE 'Ya existe al menos una empresa registrada';
        END IF;
    ELSE
        RAISE EXCEPTION 'La tabla Empresas no existe en la base de datos';
    END IF;
END $$;

-- 2. Obtener y mostrar el ID de la empresa actual para referencia
SELECT * FROM "Empresas" LIMIT 1;

-- 3. Verificar si existe la relación entre Clientes y Empresas después de la migración
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM information_schema.table_constraints tc
        JOIN information_schema.constraint_column_usage ccu USING (constraint_schema, constraint_name)
        WHERE tc.constraint_type = 'FOREIGN KEY'
        AND tc.table_name = 'Clientes'
        AND ccu.table_name = 'Empresas'
        AND ccu.column_name = 'Id'
    ) THEN
        RAISE NOTICE 'La relación entre Clientes y Empresas no existe o está mal configurada';
        
        -- Verificar si existe la columna EmpresaId en Clientes
        IF NOT EXISTS (
            SELECT 1
            FROM information_schema.columns
            WHERE table_name = 'Clientes' AND column_name = 'EmpresaId'
        ) THEN
            RAISE NOTICE 'La columna EmpresaId no existe en la tabla Clientes';
            
            -- Agregar la columna EmpresaId a Clientes si no existe
            ALTER TABLE "Clientes" ADD COLUMN "EmpresaId" integer;
            
            -- Crear índice para la columna EmpresaId
            CREATE INDEX "IX_Clientes_EmpresaId" ON "Clientes" ("EmpresaId");
            
            -- Agregar clave foránea a Empresas
            ALTER TABLE "Clientes" ADD CONSTRAINT "FK_Clientes_Empresas_EmpresaId"
                FOREIGN KEY ("EmpresaId") REFERENCES "Empresas"("Id")
                ON DELETE CASCADE;
                
            RAISE NOTICE 'Columna EmpresaId y relación con Empresas agregadas correctamente';
        END IF;
    ELSE
        RAISE NOTICE 'La relación entre Clientes y Empresas existe y está correctamente configurada';
    END IF;
END $$;

-- 4. Actualizar todos los clientes para asignarles la empresa actual
DO $$
DECLARE
    empresa_id integer;
BEGIN
    -- Obtener el ID de la primera empresa (la predeterminada si se acaba de crear)
    SELECT "Id" INTO empresa_id FROM "Empresas" ORDER BY "Id" LIMIT 1;
    
    IF empresa_id IS NOT NULL THEN
        -- Actualizar todos los clientes que no tengan EmpresaId asignado
        UPDATE "Clientes" SET "EmpresaId" = empresa_id WHERE "EmpresaId" IS NULL;
        RAISE NOTICE 'Clientes actualizados con el ID de empresa %', empresa_id;
    ELSE
        RAISE EXCEPTION 'No se encontró ninguna empresa para asociar con los clientes';
    END IF;
END $$;

-- 5. Verificar la integridad de las tablas principales relacionadas con la sesión y Empresas
DO $$
BEGIN
    -- Verificar TiposIdentificacion
    IF NOT EXISTS (SELECT 1 FROM "TiposIdentificacion" LIMIT 1) THEN
        RAISE NOTICE 'No hay datos en la tabla TiposIdentificacion. Insertando datos predeterminados...';
        
        INSERT INTO "TiposIdentificacion" ("Id", "Nombre", "Descripcion")
        VALUES 
            (1, 'Cédula', 'Cédula de identidad y electoral'),
            (2, 'RNC', 'Registro Nacional del Contribuyente'),
            (3, 'Pasaporte', 'Pasaporte');
            
        RAISE NOTICE 'Datos predeterminados insertados en TiposIdentificacion';
    END IF;
    
    -- Verificar Provincias
    IF NOT EXISTS (SELECT 1 FROM "Provincias" LIMIT 1) THEN
        RAISE NOTICE 'No hay datos en la tabla Provincias. Insertando datos predeterminados...';
        
        INSERT INTO "Provincias" ("Id", "Nombre")
        VALUES 
            (1, 'Santo Domingo'),
            (2, 'Santiago'),
            (3, 'La Vega');
            
        RAISE NOTICE 'Datos predeterminados insertados en Provincias';
    END IF;
    
    -- Verificar Municipios
    IF NOT EXISTS (SELECT 1 FROM "Municipios" LIMIT 1) THEN
        RAISE NOTICE 'No hay datos en la tabla Municipios. Insertando datos predeterminados...';
        
        INSERT INTO "Municipios" ("Id", "Nombre", "ProvinciaId")
        VALUES 
            (1, 'Santo Domingo Este', 1),
            (2, 'Santo Domingo Norte', 1),
            (3, 'Santiago', 2);
            
        RAISE NOTICE 'Datos predeterminados insertados en Municipios';
    END IF;
END $$;

-- 6. Verificar y corregir la configuración de EmpresaService en la aplicación
SELECT 'Importante: Modificar el archivo EmpresaService.cs para eliminar el return 4 hardcoded y descomentar el código original' AS mensaje;

-- 7. Verificar la configuración de la sesión en la aplicación
SELECT 'Verificar que Program.cs tenga configurada la sesión correctamente y que app.UseSession() esté presente antes de mapear las rutas' AS mensaje;

-- 8. Verificar tabla de CuentasContables
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "CuentasContables" LIMIT 1) THEN
        RAISE NOTICE 'No hay datos en la tabla CuentasContables. Asegúrese de importar el catálogo de cuentas desde la aplicación.';
        SELECT 'No hay cuentas contables en la base de datos. Se recomienda importar el catálogo desde la aplicación.' AS mensaje;
    END IF;
END $$;

-- Finalizar con un mensaje de éxito
SELECT 'Script de corrección ejecutado correctamente. Reinicie la aplicación para verificar si el error 404 ha sido resuelto.' AS mensaje_final;