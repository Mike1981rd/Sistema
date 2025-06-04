-- Script para verificar el guardado de precios múltiples y sus impuestos
-- Ejecutar este script después de guardar/actualizar un producto para verificar los datos

-- Ver los últimos 5 productos con sus precios
SELECT 
    pv.Id AS ProductoId,
    pv.Nombre AS ProductoNombre,
    pv.PrecioVenta AS PrecioVentaProducto,
    pvp.Id AS PrecioId,
    pvp.NombreNivel,
    pvp.PrecioBase,
    pvp.PrecioTotal,
    pvp.Orden,
    pvp.EsPrincipal,
    pvp.Activo AS PrecioActivo,
    pvp.FechaCreacion
FROM ProductosVenta pv
LEFT JOIN ProductoVentaPrecios pvp ON pv.Id = pvp.ProductoVentaId
WHERE pv.EmpresaId = 4
ORDER BY pv.Id DESC, pvp.Orden
LIMIT 20;

-- Ver los impuestos asociados a cada precio
SELECT 
    pv.Id AS ProductoId,
    pv.Nombre AS ProductoNombre,
    pvp.Id AS PrecioId,
    pvp.NombreNivel,
    pvp.PrecioBase,
    pvp.PrecioTotal,
    i.Id AS ImpuestoId,
    i.Nombre AS ImpuestoNombre,
    i.Porcentaje,
    pvpi.Orden AS OrdenImpuesto
FROM ProductosVenta pv
INNER JOIN ProductoVentaPrecios pvp ON pv.Id = pvp.ProductoVentaId
LEFT JOIN ProductoVentaPrecioImpuestos pvpi ON pvp.Id = pvpi.ProductoVentaPrecioId
LEFT JOIN Impuestos i ON pvpi.ImpuestoId = i.Id
WHERE pv.EmpresaId = 4
ORDER BY pv.Id DESC, pvp.Orden, pvpi.Orden
LIMIT 30;

-- Contar impuestos por precio para los últimos productos
SELECT 
    pv.Id AS ProductoId,
    pv.Nombre AS ProductoNombre,
    pvp.Id AS PrecioId,
    pvp.NombreNivel,
    COUNT(pvpi.Id) AS CantidadImpuestos
FROM ProductosVenta pv
INNER JOIN ProductoVentaPrecios pvp ON pv.Id = pvp.ProductoVentaId
LEFT JOIN ProductoVentaPrecioImpuestos pvpi ON pvp.Id = pvpi.ProductoVentaPrecioId
WHERE pv.EmpresaId = 4
GROUP BY pv.Id, pv.Nombre, pvp.Id, pvp.NombreNivel
ORDER BY pv.Id DESC
LIMIT 20;

-- Ver específicamente el producto más reciente con todos sus detalles
WITH UltimoProducto AS (
    SELECT Id 
    FROM ProductosVenta 
    WHERE EmpresaId = 4 
    ORDER BY Id DESC 
    LIMIT 1
)
SELECT 
    'PRODUCTO' as Tipo,
    pv.Id,
    pv.Nombre,
    pv.PrecioVenta,
    pv.FechaCreacion,
    NULL as PrecioId,
    NULL as NombreNivel,
    NULL as PrecioBase,
    NULL as PrecioTotal,
    NULL as ImpuestoId,
    NULL as ImpuestoNombre
FROM ProductosVenta pv
WHERE pv.Id = (SELECT Id FROM UltimoProducto)
UNION ALL
SELECT 
    'PRECIO' as Tipo,
    pv.Id,
    pv.Nombre,
    pv.PrecioVenta,
    pv.FechaCreacion,
    pvp.Id as PrecioId,
    pvp.NombreNivel,
    pvp.PrecioBase,
    pvp.PrecioTotal,
    NULL as ImpuestoId,
    NULL as ImpuestoNombre
FROM ProductosVenta pv
INNER JOIN ProductoVentaPrecios pvp ON pv.Id = pvp.ProductoVentaId
WHERE pv.Id = (SELECT Id FROM UltimoProducto)
UNION ALL
SELECT 
    'IMPUESTO' as Tipo,
    pv.Id,
    pv.Nombre,
    pv.PrecioVenta,
    pv.FechaCreacion,
    pvp.Id as PrecioId,
    pvp.NombreNivel,
    pvp.PrecioBase,
    pvp.PrecioTotal,
    i.Id as ImpuestoId,
    i.Nombre as ImpuestoNombre
FROM ProductosVenta pv
INNER JOIN ProductoVentaPrecios pvp ON pv.Id = pvp.ProductoVentaId
INNER JOIN ProductoVentaPrecioImpuestos pvpi ON pvp.Id = pvpi.ProductoVentaPrecioId
INNER JOIN Impuestos i ON pvpi.ImpuestoId = i.Id
WHERE pv.Id = (SELECT Id FROM UltimoProducto)
ORDER BY Tipo, PrecioId, ImpuestoId;