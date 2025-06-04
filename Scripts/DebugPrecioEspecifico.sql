-- Script para debuggear el producto específico de las imágenes
-- Basado en las imágenes: ProductoVentaPrecioId 15 y 17

-- Encontrar el producto que tiene los precios 15 y 17
SELECT 
    'PRODUCTO PRINCIPAL' as Seccion,
    pv.Id as ProductoId,
    pv.Nombre as ProductoNombre
FROM ProductosVenta pv
INNER JOIN ProductoVentaPrecios pvp ON pv.Id = pvp.ProductoVentaId
WHERE pvp.Id IN (15, 17)
GROUP BY pv.Id, pv.Nombre;

-- Ver los precios específicos (15 y 17) con sus impuestos
SELECT 
    'PRECIOS CON IMPUESTOS' as Seccion,
    pvp.Id as PrecioId,
    pvp.NombreNivel,
    pvp.PrecioBase,
    pvp.PrecioTotal,
    pvp.Orden,
    pvp.EsPrincipal,
    STRING_AGG(
        pvpi.ImpuestoId::text || ':' || COALESCE(i.Nombre, 'SIN_NOMBRE') || '(' || COALESCE(i.Porcentaje::text, '0') || '%)', 
        ',' 
        ORDER BY pvpi.Orden
    ) as ImpuestosDetalle,
    ARRAY_AGG(pvpi.ImpuestoId ORDER BY pvpi.Orden) as ImpuestoIds
FROM ProductoVentaPrecios pvp
LEFT JOIN ProductoVentaPrecioImpuestos pvpi ON pvp.Id = pvpi.ProductoVentaPrecioId
LEFT JOIN Impuestos i ON pvpi.ImpuestoId = i.Id
WHERE pvp.Id IN (15, 17)
GROUP BY pvp.Id, pvp.NombreNivel, pvp.PrecioBase, pvp.PrecioTotal, pvp.Orden, pvp.EsPrincipal
ORDER BY pvp.Orden;

-- Ver exactamente lo que debería devolver el API
SELECT 
    'FORMATO API' as Seccion,
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
FROM ProductoVentaPrecios pvp
LEFT JOIN ProductoVentaPrecioImpuestos pvpi ON pvp.Id = pvpi.ProductoVentaPrecioId
WHERE pvp.Id IN (15, 17)
GROUP BY pvp.Id, pvp.NombreNivel, pvp.PrecioBase, pvp.PrecioTotal, pvp.Orden, pvp.EsPrincipal, pvp.Activo
ORDER BY pvp.Orden;

-- Verificar que los impuestos 1 y 3 existen y sus detalles
SELECT 
    'IMPUESTOS INVOLUCRADOS' as Seccion,
    i.Id,
    i.Nombre,
    i.Porcentaje,
    i.Activo
FROM Impuestos i
WHERE i.Id IN (1, 3)
ORDER BY i.Id;