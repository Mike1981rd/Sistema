-- Script para verificar el impuesto de propina que no se encuentra

-- Ver todos los impuestos de la empresa 4
SELECT 
    'TODOS LOS IMPUESTOS EMPRESA 4' as Seccion,
    i.Id,
    i.Nombre,
    i.Porcentaje,
    i.Estado as Activo,
    i.EmpresaId,
    i.FechaCreacion
FROM Impuestos i
WHERE i.EmpresaId = 4
ORDER BY i.Id;

-- Ver específicamente los impuestos 1 y 3
SELECT 
    'IMPUESTOS 1 Y 3 ESPECÍFICOS' as Seccion,
    i.Id,
    i.Nombre,
    i.Porcentaje,
    i.Estado as Activo,
    i.EmpresaId,
    CASE 
        WHEN i.Estado = true THEN 'Activo' 
        ELSE 'Inactivo' 
    END as EstadoTexto
FROM Impuestos i
WHERE i.Id IN (1, 3)
ORDER BY i.Id;

-- Ver qué impuestos devolvería el query del controlador para empresa 4
SELECT 
    'QUERY DEL CONTROLADOR - TODOS' as Seccion,
    i.Id,
    i.Nombre,
    i.Porcentaje,
    i.Estado,
    CONCAT(i.Nombre, ' ', COALESCE(i.Porcentaje::text, ''), '%') as TextoFormateado
FROM Impuestos i
WHERE i.EmpresaId = 4 AND i.Estado = true
ORDER BY i.Nombre;

-- Ver qué devolvería para búsqueda exacta por ID
SELECT 
    'BÚSQUEDA EXACTA ID=1' as Seccion,
    i.Id,
    i.Nombre,
    i.Porcentaje,
    i.Estado,
    CONCAT(i.Nombre, ' ', COALESCE(i.Porcentaje::text, ''), '%') as TextoFormateado
FROM Impuestos i
WHERE i.EmpresaId = 4 AND i.Estado = true AND i.Id = 1;

SELECT 
    'BÚSQUEDA EXACTA ID=3' as Seccion,
    i.Id,
    i.Nombre,
    i.Porcentaje,
    i.Estado,
    CONCAT(i.Nombre, ' ', COALESCE(i.Porcentaje::text, ''), '%') as TextoFormateado
FROM Impuestos i
WHERE i.EmpresaId = 4 AND i.Estado = true AND i.Id = 3;

-- Ver todos los impuestos sin filtro de empresa para comparar
SELECT 
    'TODOS SIN FILTRO EMPRESA' as Seccion,
    i.Id,
    i.Nombre,
    i.Porcentaje,
    i.Estado,
    i.EmpresaId
FROM Impuestos i
WHERE i.Id IN (1, 3)
ORDER BY i.Id;