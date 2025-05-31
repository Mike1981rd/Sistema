-- Script para crear datos iniciales esenciales
-- Ejecutar este script para resolver problemas con datos faltantes que causan errores 404

-- 1. Verificar y crear la empresa predeterminada si no existe
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "Empresas" LIMIT 1) THEN
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
END $$;

-- 2. Mostrar las empresas existentes para verificación
SELECT * FROM "Empresas";

-- 3. Asegurarse de que haya al menos una cuenta contable básica
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "CuentasContables" LIMIT 1) THEN
        -- Obtener el ID de la primera empresa
        DECLARE empresa_id INT;
        SELECT "Id" INTO empresa_id FROM "Empresas" ORDER BY "Id" LIMIT 1;
        
        IF empresa_id IS NOT NULL THEN
            -- Insertar cuenta contable raíz
            INSERT INTO "CuentasContables" (
                "Nombre", "Codigo", "Categoria", "TipoCuenta", 
                "Naturaleza", "Nivel", "Orden", 
                "EsCuentaSistema", "FechaCreacion", "EmpresaId", "Activo"
            ) VALUES (
                'Plan Contable General', '1', 'ACTIVOS', 'BALANCE',
                'DEBITO', 1, 1,
                TRUE, NOW(), empresa_id, TRUE
            );
            
            -- Insertar cuenta de Activos
            INSERT INTO "CuentasContables" (
                "Nombre", "Codigo", "Categoria", "TipoCuenta", 
                "Naturaleza", "Nivel", "Orden", 
                "EsCuentaSistema", "FechaCreacion", "EmpresaId", 
                "CuentaPadreId", "Activo"
            ) VALUES (
                'Activos', '10', 'ACTIVOS', 'BALANCE',
                'DEBITO', 2, 2,
                TRUE, NOW(), empresa_id,
                (SELECT "Id" FROM "CuentasContables" WHERE "Codigo" = '1' AND "EmpresaId" = empresa_id LIMIT 1),
                TRUE
            );
            
            -- Insertar cuenta de Pasivos
            INSERT INTO "CuentasContables" (
                "Nombre", "Codigo", "Categoria", "TipoCuenta", 
                "Naturaleza", "Nivel", "Orden", 
                "EsCuentaSistema", "FechaCreacion", "EmpresaId", 
                "CuentaPadreId", "Activo"
            ) VALUES (
                'Pasivos', '20', 'PASIVOS', 'BALANCE',
                'CREDITO', 2, 3,
                TRUE, NOW(), empresa_id,
                (SELECT "Id" FROM "CuentasContables" WHERE "Codigo" = '1' AND "EmpresaId" = empresa_id LIMIT 1),
                TRUE
            );
            
            -- Insertar cuenta de Patrimonio
            INSERT INTO "CuentasContables" (
                "Nombre", "Codigo", "Categoria", "TipoCuenta", 
                "Naturaleza", "Nivel", "Orden", 
                "EsCuentaSistema", "FechaCreacion", "EmpresaId", 
                "CuentaPadreId", "Activo"
            ) VALUES (
                'Patrimonio', '30', 'PATRIMONIO', 'BALANCE',
                'CREDITO', 2, 4,
                TRUE, NOW(), empresa_id,
                (SELECT "Id" FROM "CuentasContables" WHERE "Codigo" = '1' AND "EmpresaId" = empresa_id LIMIT 1),
                TRUE
            );
            
            -- Insertar cuenta de Ingresos
            INSERT INTO "CuentasContables" (
                "Nombre", "Codigo", "Categoria", "TipoCuenta", 
                "Naturaleza", "Nivel", "Orden", 
                "EsCuentaSistema", "FechaCreacion", "EmpresaId", 
                "CuentaPadreId", "Activo"
            ) VALUES (
                'Ingresos', '40', 'INGRESOS', 'RESULTADOS',
                'CREDITO', 2, 5,
                TRUE, NOW(), empresa_id,
                (SELECT "Id" FROM "CuentasContables" WHERE "Codigo" = '1' AND "EmpresaId" = empresa_id LIMIT 1),
                TRUE
            );
            
            -- Insertar cuenta de Gastos
            INSERT INTO "CuentasContables" (
                "Nombre", "Codigo", "Categoria", "TipoCuenta", 
                "Naturaleza", "Nivel", "Orden", 
                "EsCuentaSistema", "FechaCreacion", "EmpresaId", 
                "CuentaPadreId", "Activo"
            ) VALUES (
                'Gastos', '50', 'GASTOS', 'RESULTADOS',
                'DEBITO', 2, 6,
                TRUE, NOW(), empresa_id,
                (SELECT "Id" FROM "CuentasContables" WHERE "Codigo" = '1' AND "EmpresaId" = empresa_id LIMIT 1),
                TRUE
            );
            
            RAISE NOTICE 'Cuentas contables básicas creadas';
        ELSE
            RAISE NOTICE 'No se pudo obtener un ID de empresa válido';
        END IF;
    ELSE
        RAISE NOTICE 'Ya existen cuentas contables';
    END IF;
END $$;

-- 4. Mostrar las cuentas contables existentes para verificación
SELECT * FROM "CuentasContables" ORDER BY "Nivel", "Orden";

-- 5. Asegurarse de que haya al menos una categoría
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "Categorias" LIMIT 1) THEN
        -- Obtener el ID de la primera empresa
        DECLARE empresa_id INT;
        SELECT "Id" INTO empresa_id FROM "Empresas" ORDER BY "Id" LIMIT 1;
        
        -- Intentar obtener familia existente o crear una si no existe
        DECLARE familia_id INT;
        
        IF NOT EXISTS (SELECT 1 FROM "Familias" LIMIT 1) THEN
            -- Crear una familia general
            INSERT INTO "Familias" ("Nombre", "EmpresaId", "FechaCreacion", "Activo")
            VALUES ('General', empresa_id, NOW(), TRUE)
            RETURNING "Id" INTO familia_id;
            RAISE NOTICE 'Familia predeterminada creada con ID %', familia_id;
        ELSE
            -- Usar la primera familia disponible
            SELECT "Id" INTO familia_id FROM "Familias" WHERE "EmpresaId" = empresa_id LIMIT 1;
            
            -- Si no hay familia para esta empresa pero hay otras, crear una
            IF familia_id IS NULL THEN
                INSERT INTO "Familias" ("Nombre", "EmpresaId", "FechaCreacion", "Activo")
                VALUES ('General', empresa_id, NOW(), TRUE)
                RETURNING "Id" INTO familia_id;
                RAISE NOTICE 'Familia predeterminada creada con ID %', familia_id;
            END IF;
        END IF;
        
        IF empresa_id IS NOT NULL AND familia_id IS NOT NULL THEN
            -- Insertar categoría predeterminada
            INSERT INTO "Categorias" (
                "Nombre", "FamiliaId", "EmpresaId", "FechaCreacion", "Activo"
            ) VALUES (
                'General', familia_id, empresa_id, NOW(), TRUE
            );
            
            -- Categoría adicional para productos
            INSERT INTO "Categorias" (
                "Nombre", "FamiliaId", "EmpresaId", "FechaCreacion", "Activo"
            ) VALUES (
                'Productos', familia_id, empresa_id, NOW(), TRUE
            );
            
            -- Categoría adicional para servicios
            INSERT INTO "Categorias" (
                "Nombre", "FamiliaId", "EmpresaId", "FechaCreacion", "Activo"
            ) VALUES (
                'Servicios', familia_id, empresa_id, NOW(), TRUE
            );
            
            RAISE NOTICE 'Categorías predeterminadas creadas';
        ELSE
            RAISE NOTICE 'No se pudo obtener un ID de empresa o familia válido';
        END IF;
    ELSE
        RAISE NOTICE 'Ya existen categorías';
    END IF;
END $$;

-- 6. Mostrar las categorías existentes para verificación
SELECT cat.*, fam."Nombre" as "NombreFamilia"
FROM "Categorias" cat
LEFT JOIN "Familias" fam ON cat."FamiliaId" = fam."Id"
ORDER BY cat."Id";

-- 7. Asegurarse de que existan tipos de entrada de diario predeterminados
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "TiposEntradaDiario" LIMIT 1) THEN
        -- Insertar tipos de entrada predeterminados si no existen
        INSERT INTO "TiposEntradaDiario" ("Codigo", "Nombre")
        VALUES 
            ('AC', 'Ajuste contable'),
            ('CA', 'Cierre de periodos contables'),
            ('CPC', 'Cuentas por cobrar'),
            ('CPP', 'Cuentas por pagar'),
            ('D', 'Depreciaciones'),
            ('IMP', 'Impuestos');
            
        RAISE NOTICE 'Tipos de entrada de diario predeterminados creados';
    ELSE
        RAISE NOTICE 'Ya existen tipos de entrada de diario';
    END IF;
END $$;

-- 8. Mostrar los tipos de entrada de diario existentes para verificación
SELECT * FROM "TiposEntradaDiario";

-- 9. Asegurarse de que existan numeraciones de entrada de diario predeterminadas
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "NumeracionesEntradaDiario" LIMIT 1) THEN
        -- Insertar numeraciones predeterminadas si no existen
        INSERT INTO "NumeracionesEntradaDiario" (
            "TipoEntradaDiarioId", "Nombre", "Prefijo", "NumeroActual", "EsPreferida"
        )
        SELECT 
            "Id", "Nombre", "Codigo", 1, TRUE
        FROM "TiposEntradaDiario";
            
        RAISE NOTICE 'Numeraciones de entrada de diario predeterminadas creadas';
    ELSE
        RAISE NOTICE 'Ya existen numeraciones de entrada de diario';
    END IF;
END $$;

-- 10. Mostrar las numeraciones de entrada de diario existentes para verificación
SELECT num.*, tipo."Nombre" as "NombreTipo"
FROM "NumeracionesEntradaDiario" num
JOIN "TiposEntradaDiario" tipo ON num."TipoEntradaDiarioId" = tipo."Id"
ORDER BY num."Id";

-- 11. Asegurarse de que existan impuestos predeterminados
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "Impuestos" LIMIT 1) THEN
        -- Obtener el ID de la primera empresa
        DECLARE empresa_id INT;
        SELECT "Id" INTO empresa_id FROM "Empresas" ORDER BY "Id" LIMIT 1;
        
        -- Obtener cuentas contables para impuestos
        DECLARE cuenta_ventas_id INT;
        DECLARE cuenta_compras_id INT;
        
        -- Usar cuentas existentes o las cuentas básicas creadas anteriormente
        SELECT "Id" INTO cuenta_ventas_id 
        FROM "CuentasContables" 
        WHERE "Codigo" LIKE '40%' AND "EmpresaId" = empresa_id 
        ORDER BY "Id" LIMIT 1;
        
        SELECT "Id" INTO cuenta_compras_id 
        FROM "CuentasContables" 
        WHERE "Codigo" LIKE '50%' AND "EmpresaId" = empresa_id 
        ORDER BY "Id" LIMIT 1;
        
        IF empresa_id IS NOT NULL THEN
            -- Insertar impuesto predeterminado
            INSERT INTO "Impuestos" (
                "Nombre", "Descripcion", "Porcentaje", "TipoImpuesto",
                "CuentaContableVentasId", "CuentaContableComprasId",
                "EmpresaId", "Activo", "FechaCreacion"
            ) VALUES (
                'IVA 18%', 'Impuesto al Valor Agregado', 18.00, 'IVA',
                cuenta_ventas_id, cuenta_compras_id,
                empresa_id, TRUE, NOW()
            );
            
            -- Insertar otro impuesto común
            INSERT INTO "Impuestos" (
                "Nombre", "Descripcion", "Porcentaje", "TipoImpuesto",
                "CuentaContableVentasId", "CuentaContableComprasId",
                "EmpresaId", "Activo", "FechaCreacion"
            ) VALUES (
                'Exento', 'Exento de impuestos', 0.00, 'EXENTO',
                cuenta_ventas_id, cuenta_compras_id,
                empresa_id, TRUE, NOW()
            );
            
            RAISE NOTICE 'Impuestos predeterminados creados';
        ELSE
            RAISE NOTICE 'No se pudo obtener un ID de empresa válido';
        END IF;
    ELSE
        RAISE NOTICE 'Ya existen impuestos';
    END IF;
END $$;

-- 12. Mostrar los impuestos existentes para verificación
SELECT * FROM "Impuestos";

-- 13. Crear un almacén predeterminado si no existe
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "Almacenes" LIMIT 1) THEN
        -- Obtener el ID de la primera empresa
        DECLARE empresa_id INT;
        SELECT "Id" INTO empresa_id FROM "Empresas" ORDER BY "Id" LIMIT 1;
        
        IF empresa_id IS NOT NULL THEN
            -- Insertar almacén predeterminado
            INSERT INTO "Almacenes" (
                "Nombre", "Descripcion", "Direccion", "EmpresaId", 
                "FechaCreacion", "Activo"
            ) VALUES (
                'Almacén Principal', 'Almacén principal de la empresa', 
                'Dirección del almacén', empresa_id, 
                NOW(), TRUE
            );
            
            RAISE NOTICE 'Almacén predeterminado creado';
        ELSE
            RAISE NOTICE 'No se pudo obtener un ID de empresa válido';
        END IF;
    ELSE
        RAISE NOTICE 'Ya existen almacenes';
    END IF;
END $$;

-- 14. Mostrar los almacenes existentes para verificación
SELECT * FROM "Almacenes";

-- Script completado
SELECT 'Datos iniciales creados correctamente. Verifica la salida para confirmar.' AS Resultado;