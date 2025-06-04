# Cambios: Costo % en Pestaña de Recetas

## Fecha: Enero 2025

## Resumen de Cambios
Se ha implementado la funcionalidad de "Costo %" en la pestaña de recetas según los requerimientos especificados.

## Cambios Realizados

### 1. **Vista HTML - GestionarProducto.cshtml**

**Archivo:** `/Views/Productos/GestionarProducto.cshtml`

#### Cambio en líneas 396-404:
```html
<!-- ANTES -->
<div class="cost-info-row">
    <span>Costo Unitario:</span>
    <span id="costoUnitarioProducto">$0.00</span>
</div>

<!-- DESPUÉS -->
<div class="cost-info-row" style="display: none;">
    <span>Costo Unitario:</span>
    <span id="costoUnitarioProducto">$0.00</span>
</div>

<div class="cost-info-row">
    <span>Costo %:</span>
    <span id="costoPorcentajeProducto">0.00%</span>
</div>
```

**Resultado:**
- ✅ "Costo Unitario" oculto (no eliminado)
- ✅ "Costo %" agregado en su lugar

#### Cambio en función calcularTotales (líneas 1091-1101):
```javascript
// Recalcular el Costo % cuando cambie el total de ingredientes
const primeraFila = document.querySelector('#priceRowsContainer .price-row');
if (primeraFila) {
    const precioBaseInput = primeraFila.querySelector('.precio-base');
    if (precioBaseInput) {
        const precioBase = parseFloat(precioBaseInput.value) || 0;
        if (typeof calcularCostoPorcentaje === 'function') {
            calcularCostoPorcentaje(precioBase);
        }
    }
}
```

### 2. **JavaScript - gestionar-producto.js**

**Archivo:** `/wwwroot/js/productos/gestionar-producto.js`

#### Corrección en actualizarPrecioEnRecetas (líneas 1381-1394):
```javascript
// ANTES: Tomaba precio TOTAL
const precioTotalInput = primeraFila.querySelector('.precio-total');
const precioTotal = parseFloat(precioTotalInput.value) || 0;

// DESPUÉS: Toma precio BASE
const precioBaseInput = primeraFila.querySelector('.precio-base');
const precioBase = parseFloat(precioBaseInput.value) || 0;
```

**Resultado:**
- ✅ Ahora usa precio **base** del nivel 1, no precio **total**

#### Nueva función calcularCostoPorcentaje (líneas 1424-1445):
```javascript
function calcularCostoPorcentaje(precioBase) {
    // Obtener el costo total de ingredientes desde la pestaña de recetas
    const totalRecetaElement = document.getElementById('totalReceta');
    const costoTotalIngredientes = totalRecetaElement ? 
        parseFloat(totalRecetaElement.textContent.replace('$', '')) || 0 : 0;
    
    // Calcular el porcentaje
    let costoPorcentaje = 0;
    if (precioBase > 0) {
        costoPorcentaje = (costoTotalIngredientes / precioBase) * 100;
    }
    
    // Actualizar el elemento en la interfaz
    const costoPorcentajeElement = document.getElementById('costoPorcentajeProducto');
    if (costoPorcentajeElement) {
        costoPorcentajeElement.textContent = costoPorcentaje.toFixed(2) + '%';
    }
}
```

## Fórmula Implementada

### **Costo % = (Costo Total Ingredientes / Precio Base Nivel 1) × 100**

**Donde:**
- **Costo Total Ingredientes**: Suma de todos los ingredientes en la receta
- **Precio Base Nivel 1**: Precio base (sin impuestos) del primer nivel de precios
- **Resultado**: Porcentaje que representa el costo de ingredientes respecto al precio base

## Casos de Actualización

El "Costo %" se recalcula automáticamente cuando:

1. **Cambia el precio base del nivel 1** → Via `actualizarPrecioEnRecetas()`
2. **Cambian los ingredientes de la receta** → Via `calcularTotales()`
3. **Se agregan/eliminan ingredientes** → Via `calcularTotales()`
4. **Cambian las cantidades de ingredientes** → Via `calcularTotales()`

## Ejemplo de Funcionamiento

```
Costo Total Ingredientes: $45.00
Precio Base Nivel 1: $100.00
Costo % = (45.00 / 100.00) × 100 = 45.00%
```

## Estado Final

✅ **Completado:**
- Costo Unitario oculto (mantenido para compatibilidad)
- Costo % implementado con fórmula correcta
- Precio Venta Producto usa precio BASE (no total) del nivel 1
- Actualización automática en tiempo real
- Logs de debug incluidos

**La funcionalidad está lista para usar en la pestaña de recetas.**