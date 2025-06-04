-- Script para debuggear específicamente el guardado de impuestos múltiples
-- Ejecutar después de guardar un producto con múltiples niveles de precio e impuestos

-- Ver el último producto creado/modificado con todos sus precios e impuestos
WITH UltimoProducto AS (
    SELECT Id, FechaModificacion
    FROM ProductosVenta 
    WHERE EmpresaId = 4 
    ORDER BY COALESCE(FechaModificacion, FechaCreacion) DESC 
    LIMIT 1
)
SELECT 
    'DETALLE COMPLETO' as Seccion,
    pv.Id as ProductoId,
    pv.Nombre as ProductoNombre,
    pvp.Id as PrecioId,
    pvp.NombreNivel,
    pvp.PrecioBase,
    pvp.PrecioTotal,
    pvp.Orden as OrdenPrecio,
    pvp.EsPrincipal,
    pvpi.Id as ImpuestoPrecioId,
    pvpi.ImpuestoId,
    i.Nombre as ImpuestoNombre,
    i.Porcentaje as ImpuestoPorcentaje,
    pvpi.Orden as OrdenImpuesto,
    pvpi.FechaCreacion as FechaCreacionImpuesto
FROM UltimoProducto up
INNER JOIN ProductosVenta pv ON up.Id = pv.Id
INNER JOIN ProductoVentaPrecios pvp ON pv.Id = pvp.ProductoVentaId
LEFT JOIN ProductoVentaPrecioImpuestos pvpi ON pvp.Id = pvpi.ProductoVentaPrecioId
LEFT JOIN Impuestos i ON pvpi.ImpuestoId = i.Id
ORDER BY pvp.Orden, pvpi.Orden;

-- Resumen por nivel de precio
WITH UltimoProducto AS (
    SELECT Id 
    FROM ProductosVenta 
    WHERE EmpresaId = 4 
    ORDER BY COALESCE(FechaModificacion, FechaCreacion) DESC 
    LIMIT 1
)
SELECT 
    'RESUMEN POR NIVEL' as Seccion,
    pvp.Id as PrecioId,
    pvp.NombreNivel,
    pvp.Orden,
    COUNT(pvpi.Id) as CantidadImpuestos,
    STRING_AGG(
        i.Nombre || ' (' || i.Porcentaje || '%)', 
        ', ' 
        ORDER BY pvpi.Orden
    ) as ImpuestosDetalle,
    STRING_AGG(
        pvpi.ImpuestoId::text, 
        ',' 
        ORDER BY pvpi.Orden
    ) as ImpuestosIds
FROM UltimoProducto up
INNER JOIN ProductosVenta pv ON up.Id = pv.Id
INNER JOIN ProductoVentaPrecios pvp ON pv.Id = pvp.ProductoVentaId
LEFT JOIN ProductoVentaPrecioImpuestos pvpi ON pvp.Id = pvpi.ProductoVentaPrecioId
LEFT JOIN Impuestos i ON pvpi.ImpuestoId = i.Id
GROUP BY pvp.Id, pvp.NombreNivel, pvp.Orden
ORDER BY pvp.Orden;

-- Ver solo registros de impuestos en tabla de relación
SELECT 
    'TABLA IMPUESTOS PRECIO' as Seccion,
    pvpi.Id,
    pvpi.ProductoVentaPrecioId,
    pvpi.ImpuestoId,
    pvpi.Orden,
    pvpi.FechaCreacion,
    i.Nombre as ImpuestoNombre,
    pvp.NombreNivel as NivelPrecio
FROM ProductoVentaPrecioImpuestos pvpi
INNER JOIN ProductoVentaPrecios pvp ON pvpi.ProductoVentaPrecioId = pvp.Id
INNER JOIN ProductosVenta pv ON pvp.ProductoVentaId = pv.Id
LEFT JOIN Impuestos i ON pvpi.ImpuestoId = i.Id
WHERE pv.EmpresaId = 4
ORDER BY pvpi.FechaCreacion DESC, pvpi.ProductoVentaPrecioId, pvpi.Orden
LIMIT 20;

-- Verificar si hay duplicados o problemas de integridad
SELECT 
    'VERIFICACION DUPLICADOS' as Seccion,
    ProductoVentaPrecioId,
    ImpuestoId,
    COUNT(*) as Cantidad
FROM ProductoVentaPrecioImpuestos
GROUP BY ProductoVentaPrecioId, ImpuestoId
HAVING COUNT(*) > 1;