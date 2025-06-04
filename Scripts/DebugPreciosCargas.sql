-- Script para debuggear la carga de precios específico
-- Ejecutar este script para ver exactamente qué datos están guardados

-- Seleccionar un producto específico (cambiar el ID según necesidad)
-- Para ver todos los productos disponibles:
SELECT Id, Nombre, PrecioVenta FROM ProductosVenta WHERE EmpresaId = 4 ORDER BY Id DESC LIMIT 10;

-- Ver los precios del producto más reciente con detalles completos
WITH UltimoProducto AS (
    SELECT Id 
    FROM ProductosVenta 
    WHERE EmpresaId = 4 
    ORDER BY Id DESC 
    LIMIT 1
)
SELECT 
    'PRODUCTO PRINCIPAL' as Tipo,
    pv.Id as ProductoId,
    pv.Nombre as ProductoNombre,
    pv.PrecioVenta as PrecioVentaProducto,
    NULL::integer as PrecioId,
    NULL as NombreNivel,
    NULL::decimal as PrecioBase,
    NULL::decimal as PrecioTotal,
    NULL::boolean as EsPrincipal,
    NULL::integer as Orden,
    NULL::boolean as PrecioActivo,
    pv.FechaCreacion as Fecha
FROM ProductosVenta pv
WHERE pv.Id = (SELECT Id FROM UltimoProducto)
UNION ALL
SELECT 
    'PRECIO INDIVIDUAL' as Tipo,
    pv.Id as ProductoId,
    pv.Nombre as ProductoNombre,
    pv.PrecioVenta as PrecioVentaProducto,
    pvp.Id as PrecioId,
    pvp.NombreNivel,
    pvp.PrecioBase,
    pvp.PrecioTotal,
    pvp.EsPrincipal,
    pvp.Orden,
    pvp.Activo as PrecioActivo,
    pvp.FechaCreacion as Fecha
FROM ProductosVenta pv
INNER JOIN ProductoVentaPrecios pvp ON pv.Id = pvp.ProductoVentaId
WHERE pv.Id = (SELECT Id FROM UltimoProducto)
ORDER BY Tipo, Orden;

-- Ver los impuestos de cada precio
WITH UltimoProducto AS (
    SELECT Id 
    FROM ProductosVenta 
    WHERE EmpresaId = 4 
    ORDER BY Id DESC 
    LIMIT 1
)
SELECT 
    pv.Id as ProductoId,
    pv.Nombre as ProductoNombre,
    pvp.Id as PrecioId,
    pvp.NombreNivel,
    pvp.PrecioBase,
    pvp.PrecioTotal,
    COUNT(pvpi.Id) as CantidadImpuestos,
    STRING_AGG(
        CASE 
            WHEN i.Nombre IS NOT NULL 
            THEN i.Nombre || ' (' || i.Porcentaje || '%)'
            ELSE 'Impuesto ID: ' || pvpi.ImpuestoId
        END, 
        ', ' 
        ORDER BY pvpi.Orden
    ) as ImpuestosDetalle
FROM ProductosVenta pv
INNER JOIN ProductoVentaPrecios pvp ON pv.Id = pvp.ProductoVentaId
LEFT JOIN ProductoVentaPrecioImpuestos pvpi ON pvp.Id = pvpi.ProductoVentaPrecioId
LEFT JOIN Impuestos i ON pvpi.ImpuestoId = i.Id
WHERE pv.Id = (SELECT Id FROM UltimoProducto)
GROUP BY pv.Id, pv.Nombre, pvp.Id, pvp.NombreNivel, pvp.PrecioBase, pvp.PrecioTotal, pvp.Orden
ORDER BY pvp.Orden;

-- Verificar formato JSON como lo devolvería el API
WITH UltimoProducto AS (
    SELECT Id 
    FROM ProductosVenta 
    WHERE EmpresaId = 4 
    ORDER BY Id DESC 
    LIMIT 1
)
SELECT 
    pvp.Id,
    pvp.NombreNivel,
    pvp.PrecioBase,
    pvp.PrecioTotal,
    pvp.Orden,
    pvp.EsPrincipal,
    pvp.Activo,
    COALESCE(
        JSON_AGG(
            pvpi.ImpuestoId 
            ORDER BY pvpi.Orden
        ) FILTER (WHERE pvpi.ImpuestoId IS NOT NULL), 
        '[]'::json
    ) as ImpuestoIds
FROM ProductosVenta pv
INNER JOIN ProductoVentaPrecios pvp ON pv.Id = pvp.ProductoVentaId
LEFT JOIN ProductoVentaPrecioImpuestos pvpi ON pvp.Id = pvpi.ProductoVentaPrecioId
WHERE pv.Id = (SELECT Id FROM UltimoProducto)
GROUP BY pvp.Id, pvp.NombreNivel, pvp.PrecioBase, pvp.PrecioTotal, pvp.Orden, pvp.EsPrincipal, pvp.Activo
ORDER BY pvp.Orden;