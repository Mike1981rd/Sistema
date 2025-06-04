# Fix: Cálculos No Funcionan Al Cargar Precios Existentes

## Fecha: Enero 2025

## Problema Identificado

Cuando se edita un producto y se va a la pestaña de precios, los valores se cargan correctamente (precio base + impuestos), pero los cálculos automáticos no se ejecutan, mostrando el precio total incorrecto en la interfaz.

**Síntomas:**
- Los precios base e impuestos se cargan desde la BD
- Los impuestos aparecen seleccionados en el Select2
- Los cálculos manuales funcionan (cambiar precio base → calcula total)
- **PERO** al cargar, el precio total no se actualiza automáticamente

## Root Cause Identificado

**Problema Principal:** Cuando se cargan impuestos existentes, el porcentaje de cada impuesto no se guardaba en `window.impuestosDataGlobal`, que es necesario para los cálculos.

**Secuencia problemática:**
1. Se cargan los precios desde `/api/productos/{id}/precios`
2. Se cargan los impuestos desde `/api/impuestos` 
3. Se seleccionan los impuestos en el Select2: `$impuestoSelect.trigger('change')`
4. **El cálculo falla** porque `window.impuestosDataGlobal[impuestoId]` = `undefined`

## Solución Implementada

### 1. Guardar Porcentajes al Cargar Impuestos

**Archivo:** `/wwwroot/js/productos/gestionar-producto.js`
**Función:** `cargarImpuestosEnFila()`

```javascript
}).then(function(data) {
    if (data && data.length > 0) {
        const impuesto = data[0];
        
        // Guardar el porcentaje en el store global - CRÍTICO
        if (!window.impuestosDataGlobal) {
            window.impuestosDataGlobal = {};
        }
        window.impuestosDataGlobal[impuesto.id] = parseFloat(impuesto.porcentaje);
        
        // Agregar opción al Select2...
    }
});
```

### 2. Forzar Recálculo Después de Cargar

```javascript
// Esperar a que todos los impuestos se carguen y luego seleccionarlos
Promise.all(promesasImpuestos).then(() => {
    const idsParaSeleccionar = impuestoIds.map(id => id.toString());
    $impuestoSelect.val(idsParaSeleccionar).trigger('change');
    
    // Forzar recálculo después de un pequeño delay
    setTimeout(() => {
        const $precioBase = $row.find('.precio-base');
        if ($precioBase.length && $precioBase.val()) {
            $precioBase.trigger('input'); // Dispara calcularPrecioTotal()
        }
    }, 500);
});
```

## Por Qué Esta Solución Funciona

1. **Se guardan los porcentajes:** Ahora `window.impuestosDataGlobal[impuestoId]` tiene el valor correcto
2. **Se fuerza el recálculo:** Después de cargar todo, se simula que el usuario cambió el precio base
3. **Timing correcto:** El delay de 500ms asegura que todos los datos estén disponibles
4. **Logs para debug:** Se puede verificar en consola que todo funciona

## Cómo Probar

1. **Editar un producto existente con múltiples precios e impuestos**
2. **Ir a la pestaña "Precios e Impuestos"**
3. **Verificar en consola del navegador:**
   ```
   [DEBUG] Cargando impuesto 1: ITBIS (18%)
   [DEBUG] Guardado en window.impuestosDataGlobal[1] = 18
   [DEBUG] Forzando recálculo después de cargar impuestos
   [DEBUG] Disparando recálculo en fila 0 basado en precio base: 100.00
   ```
4. **El precio total debería actualizarse automáticamente**

## Estado

✅ Los porcentajes de impuestos se guardan correctamente al cargar
✅ Se fuerza el recálculo después de cargar precios existentes  
✅ Los cálculos funcionan correctamente en todos los niveles
✅ Funciona tanto para el primer nivel como niveles adicionales