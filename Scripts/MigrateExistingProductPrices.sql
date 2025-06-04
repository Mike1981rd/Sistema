-- Script para migrar precios existentes al nuevo sistema de múltiples precios
-- Este script crea un precio base para cada producto existente

-- Insertar precios base para productos existentes que no tienen niveles de precio
INSERT INTO "ProductoVentaPrecios" (
    "ProductoVentaId",
    "NombreNivel",
    "PrecioBase",
    "PrecioTotal",
    "Orden",
    "EsPrincipal",
    "Activo",
    "FechaCreacion",
    "EmpresaId"
)
SELECT 
    pv."Id" as "ProductoVentaId",
    'Precio Base' as "NombreNivel",
    pv."PrecioVenta" as "PrecioBase",
    pv."PrecioVenta" as "PrecioTotal", -- Se recalculará después con impuestos
    0 as "Orden",
    true as "EsPrincipal",
    true as "Activo",
    NOW() as "FechaCreacion",
    pv."EmpresaId"
FROM "ProductosVenta" pv
WHERE pv."PrecioVenta" > 0
  AND NOT EXISTS (
      SELECT 1 FROM "ProductoVentaPrecios" pvp 
      WHERE pvp."ProductoVentaId" = pv."Id"
  );

-- Copiar los impuestos del producto a su precio principal
-- Esto mantiene la compatibilidad con el sistema actual
INSERT INTO "ProductoVentaPrecioImpuestos" (
    "ProductoVentaPrecioId",
    "ImpuestoId",
    "Orden",
    "Activo",
    "FechaCreacion",
    "EmpresaId"
)
SELECT 
    pvp."Id" as "ProductoVentaPrecioId",
    pvi."ImpuestoId",
    pvi."Orden",
    true as "Activo",
    NOW() as "FechaCreacion",
    pvi."EmpresaId"
FROM "ProductoVentaPrecios" pvp
INNER JOIN "ProductosVenta" pv ON pvp."ProductoVentaId" = pv."Id"
INNER JOIN "ProductoVentaImpuestos" pvi ON pvi."ProductoVentaId" = pv."Id"
WHERE pvp."EsPrincipal" = true
  AND NOT EXISTS (
      SELECT 1 FROM "ProductoVentaPrecioImpuestos" pvpi 
      WHERE pvpi."ProductoVentaPrecioId" = pvp."Id" 
        AND pvpi."ImpuestoId" = pvi."ImpuestoId"
  );

-- Recalcular el PrecioTotal de los precios base con sus impuestos
UPDATE "ProductoVentaPrecios" 
SET "PrecioTotal" = "PrecioBase" * (1 + COALESCE(impuestos_total.total_porcentaje, 0))
FROM (
    SELECT 
        pvp."Id" as precio_id,
        SUM(COALESCE(i."Porcentaje", 0) / 100.0) as total_porcentaje
    FROM "ProductoVentaPrecios" pvp
    LEFT JOIN "ProductoVentaPrecioImpuestos" pvpi ON pvpi."ProductoVentaPrecioId" = pvp."Id"
    LEFT JOIN "Impuestos" i ON i."Id" = pvpi."ImpuestoId" AND i."Activo" = true
    WHERE pvp."EsPrincipal" = true
    GROUP BY pvp."Id"
) impuestos_total
WHERE "ProductoVentaPrecios"."Id" = impuestos_total.precio_id;

-- Verificar resultados
SELECT 
    COUNT(*) as total_productos,
    COUNT(CASE WHEN pvp."Id" IS NOT NULL THEN 1 END) as productos_con_precios
FROM "ProductosVenta" pv
LEFT JOIN "ProductoVentaPrecios" pvp ON pvp."ProductoVentaId" = pv."Id" AND pvp."EsPrincipal" = true
WHERE pv."PrecioVenta" > 0;