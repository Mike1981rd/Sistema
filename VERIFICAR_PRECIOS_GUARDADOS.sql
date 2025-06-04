-- Script para verificar precios múltiples guardados
-- Reemplaza el ProductoId = 7 con el ID de tu producto

-- 1. Ver el producto principal
SELECT Id, Nombre, PrecioVenta 
FROM ProductosVenta 
WHERE Id = 7;

-- 2. Ver los precios múltiples guardados
SELECT 
    Id,
    ProductoVentaId,
    NombreNivel,
    PrecioBase,
    PrecioTotal,
    Orden,
    EsPrincipal,
    Activo,
    FechaCreacion
FROM ProductoVentaPrecios 
WHERE ProductoVentaId = 7 
ORDER BY Orden;

-- 3. Ver los impuestos por cada precio
SELECT 
    pvp.Id AS PrecioId,
    pvp.NombreNivel,
    pvp.PrecioBase,
    pvp.PrecioTotal,
    pvpi.ImpuestoId,
    i.Nombre AS NombreImpuesto,
    i.Porcentaje,
    pvpi.Orden AS OrdenImpuesto
FROM ProductoVentaPrecios pvp
LEFT JOIN ProductoVentaPrecioImpuestos pvpi ON pvp.Id = pvpi.ProductoVentaPrecioId
LEFT JOIN Impuestos i ON pvpi.ImpuestoId = i.Id
WHERE pvp.ProductoVentaId = 7
ORDER BY pvp.Orden, pvpi.Orden;

-- 4. Contar impuestos por precio (debe mostrar múltiples si está funcionando)
SELECT 
    pvp.NombreNivel,
    COUNT(pvpi.ImpuestoId) AS TotalImpuestos,
    STRING_AGG(i.Nombre, ', ') AS NombresImpuestos
FROM ProductoVentaPrecios pvp
LEFT JOIN ProductoVentaPrecioImpuestos pvpi ON pvp.Id = pvpi.ProductoVentaPrecioId
LEFT JOIN Impuestos i ON pvpi.ImpuestoId = i.Id
WHERE pvp.ProductoVentaId = 7
GROUP BY pvp.Id, pvp.NombreNivel, pvp.Orden
ORDER BY pvp.Orden;