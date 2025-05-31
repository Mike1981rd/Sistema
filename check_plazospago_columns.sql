-- Script para verificar columnas existentes en PlazosPago
SELECT 
    column_name, 
    data_type, 
    is_nullable,
    character_maximum_length
FROM 
    information_schema.columns
WHERE 
    table_name = 'PlazosPago'
    AND column_name IN ('Descripcion', 'Estado')
ORDER BY 
    ordinal_position;