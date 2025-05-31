-- Script para verificar y crear una empresa predeterminada si no existe ninguna
DO $$
BEGIN
    -- Verificar si la tabla Empresas existe
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
            
            RAISE NOTICE 'Empresa predeterminada creada con ID 1';
        ELSE
            RAISE NOTICE 'Ya existe al menos una empresa registrada';
            -- Mostrar las empresas existentes
            SELECT id, "Nombre", "FechaCreacion" FROM "Empresas";
        END IF;
    ELSE
        RAISE EXCEPTION 'La tabla Empresas no existe en la base de datos. Asegúrate de que las migraciones se hayan aplicado correctamente.';
    END IF;
END $$;