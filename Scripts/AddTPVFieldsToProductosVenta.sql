-- Script para agregar las nuevas columnas a ProductosVenta para TPV
-- Ejecutar este script antes de aplicar la migración

-- Agregar columnas nuevas a la tabla ProductosVenta
ALTER TABLE "ProductosVenta" 
ADD COLUMN IF NOT EXISTS "NombreCortoTPV" VARCHAR(20) NULL,
ADD COLUMN IF NOT EXISTS "Descripcion" VARCHAR(500) NULL,
ADD COLUMN IF NOT EXISTS "PLU" VARCHAR(50) NULL,
ADD COLUMN IF NOT EXISTS "ColorBotonTPV" VARCHAR(7) NULL,
ADD COLUMN IF NOT EXISTS "OrdenClasificacion" INT NOT NULL DEFAULT 0,
ADD COLUMN IF NOT EXISTS "EsActivo" BOOLEAN NOT NULL DEFAULT TRUE,
ADD COLUMN IF NOT EXISTS "PermiteModificadores" BOOLEAN NOT NULL DEFAULT TRUE,
ADD COLUMN IF NOT EXISTS "RequierePuntoCoccion" BOOLEAN NOT NULL DEFAULT FALSE,
ADD COLUMN IF NOT EXISTS "CategoriaId" INT NULL,
ADD COLUMN IF NOT EXISTS "RutaImpresoraId" INT NULL;

-- Modificar columna Costo para que no sea nullable
ALTER TABLE "ProductosVenta" 
ALTER COLUMN "Costo" SET NOT NULL,
ALTER COLUMN "Costo" SET DEFAULT 0;

-- Agregar foreign keys
ALTER TABLE "ProductosVenta" 
ADD CONSTRAINT "FK_ProductosVenta_Categorias_CategoriaId" 
FOREIGN KEY ("CategoriaId") REFERENCES "Categorias"("Id") ON DELETE RESTRICT;

ALTER TABLE "ProductosVenta" 
ADD CONSTRAINT "FK_ProductosVenta_RutasImpresora_RutaImpresoraId" 
FOREIGN KEY ("RutaImpresoraId") REFERENCES "RutasImpresora"("Id") ON DELETE RESTRICT;

-- Agregar índice único a PLU si existe la columna
CREATE UNIQUE INDEX IF NOT EXISTS "IX_ProductosVenta_PLU" 
ON "ProductosVenta"("PLU") 
WHERE "PLU" IS NOT NULL;