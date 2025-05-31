-- Script para corregir clientes sin EmpresaId asignado
UPDATE "Clientes" 
SET "EmpresaId" = 1 
WHERE "EmpresaId" IS NULL;

-- Verifica que todos los clientes tengan una empresa asignada
SELECT COUNT(*) AS "ClientesSinEmpresa" FROM "Clientes" WHERE "EmpresaId" IS NULL;

-- Asegura que la relación con Empresa sea válida y apunte a empresas existentes
UPDATE "Clientes" c
SET "EmpresaId" = 1
FROM (
    SELECT c."Id"
    FROM "Clientes" c
    LEFT JOIN "Empresas" e ON c."EmpresaId" = e."Id"
    WHERE c."EmpresaId" IS NOT NULL 
    AND e."Id" IS NULL
) AS invalid_relations
WHERE c."Id" = invalid_relations."Id";