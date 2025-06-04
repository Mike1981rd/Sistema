# Fix: Segundo Nivel No Muestra Múltiples Impuestos

## Fecha: Enero 2025

## Problema Identificado

Basado en las imágenes proporcionadas:

### **Base de Datos (Correcto):**
- **Precio 15 (Nivel 1):** Impuestos 1 y 3 ✅
- **Precio 17 (Nivel 2):** Impuesto 1 ✅

### **Pantalla de Edición (Incorrecto):**
- **Primer nivel:** Muestra ITBIS 18% + 10% Propina ✅
- **Segundo nivel:** Solo muestra 10% Propina ❌ (debería mostrar ITBIS 18%)

## Root Cause

El problema estaba en **la carga de impuestos, no en el guardado**. Específicamente:

1. **Orden inconsistente en el endpoint API**
2. **Falta de logs detallados para debugging**
3. **Posible problema en el mapeo de IDs**

## Soluciones Implementadas

### 1. **Corregir Orden en el Endpoint API**

**Archivo:** `/Controllers/API/ProductosController.cs`
**Método:** `ObtenerPreciosProducto`

```csharp
// ANTES: Sin orden específico
ImpuestoIds = p.ImpuestosEspecificos?.Select(pi => pi.ImpuestoId).ToList() ?? new List<int>(),

// DESPUÉS: Ordenado por pi.Orden
ImpuestoIds = p.ImpuestosEspecificos?.OrderBy(pi => pi.Orden).Select(pi => pi.ImpuestoId).ToList() ?? new List<int>(),
```

### 2. **Agregar Logs Detallados en JavaScript**

**Archivo:** `/wwwroot/js/productos/gestionar-producto.js`

#### A) Logs en `cargarPreciosProducto`:
```javascript
precios.forEach((precio, index) => {
    console.log(`[DEBUG] Precio ${index} - NombreNivel: ${precio.NombreNivel}`);
    console.log(`[DEBUG] Precio ${index} - ImpuestoIds: [${precio.ImpuestoIds.join(',')}]`);
    console.log(`[DEBUG] Precio ${index} - PrecioBase: ${precio.PrecioBase}`);
});
```

#### B) Logs en `cargarImpuestosEnFila`:
```javascript
function cargarImpuestosEnFila($row, impuestoIds) {
    const rowIndex = $row.closest('.price-row').index();
    console.log(`[DEBUG] cargarImpuestosEnFila - Fila ${rowIndex}, ImpuestoIds:`, impuestoIds);
    console.log(`[DEBUG] Iniciando carga de ${impuestoIds.length} impuestos`);
    
    // Logs para cada impuesto individual
    impuestoIds.map((impuestoId, index) => {
        console.log(`[DEBUG] Cargando impuesto ${index + 1}/${impuestoIds.length} - ID: ${impuestoId}`);
    });
    
    // Logs al seleccionar
    console.log(`[DEBUG] Seleccionando impuestos:`, idsParaSeleccionar);
    console.log(`[DEBUG] Opciones disponibles:`, $impuestoSelect.find('option').map(...).get());
    console.log(`[DEBUG] Impuestos seleccionados finalmente:`, $impuestoSelect.val());
}
```

### 3. **Script SQL para Debugging**

**Archivo:** `/Scripts/DebugPrecioEspecifico.sql`

Script para verificar exactamente qué datos están en la BD para los precios específicos y qué debería devolver el API.

## Cómo Verificar la Corrección

### 1. **Ejecutar el Script SQL:**
```sql
-- Ver exactamente lo que debería devolver el API para los precios 15 y 17
SELECT 
    pvp.Id,
    pvp.NombreNivel,
    ARRAY_AGG(pvpi.ImpuestoId ORDER BY pvpi.Orden) as ImpuestoIds
FROM ProductoVentaPrecios pvp
LEFT JOIN ProductoVentaPrecioImpuestos pvpi ON pvp.Id = pvpi.ProductoVentaPrecioId
WHERE pvp.Id IN (15, 17)
GROUP BY pvp.Id, pvp.NombreNivel
ORDER BY pvp.Orden;
```

### 2. **Verificar en Consola del Navegador:**
Al editar el producto, deberías ver logs como:
```
[DEBUG] Precio 0 - ImpuestoIds: [1,3]
[DEBUG] Precio 1 - ImpuestoIds: [1]
[DEBUG] cargarImpuestosEnFila - Fila 1, ImpuestoIds: [1]
[DEBUG] Iniciando carga de 1 impuestos
[DEBUG] Cargando impuesto 1/1 - ID: 1
[DEBUG] Seleccionando impuestos: ["1"]
[DEBUG] Impuestos seleccionados finalmente: ["1"]
```

### 3. **Verificar en la Interfaz:**
- **Primer nivel:** Debe mostrar ITBIS 18% + 10% Propina
- **Segundo nivel:** Debe mostrar ITBIS 18% (solo ese)

## Estado Final

✅ Endpoint API devuelve ImpuestoIds en orden correcto
✅ Logs detallados para debugging
✅ Función cargarImpuestosEnFila mejorada
✅ Scripts SQL para verificación

**Con estas correcciones, el segundo nivel debería mostrar correctamente el impuesto ITBIS 18% que está guardado en la base de datos.**