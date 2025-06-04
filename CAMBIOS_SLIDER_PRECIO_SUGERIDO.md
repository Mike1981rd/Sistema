# Cambios: Slider de Precio Sugerido en Recetas

## Fecha: Enero 2025

## Resumen de Cambios
Se ha actualizado la funcionalidad del slider para calcular el **Precio Sugerido** basado en el **porcentaje de costo deseado**.

## Cambios Realizados

### 1. **Campos Ocultos en Leyenda de Recetas**

**Archivo:** `/Views/Productos/GestionarProducto.cshtml`

#### Campos ocultados (líneas 406-414):
```html
<!-- OCULTOS -->
<div class="cost-info-row" style="display: none;">
    <span>Cantidad Base:</span>
    <span id="cantidadBaseProducto">1.00</span>
</div>

<div class="cost-info-row" style="display: none;">
    <span>Costo Total Producto:</span>
    <span id="costoTotalProducto">$0.00</span>
</div>
```

#### Campo visible:
```html
<!-- VISIBLE -->
<div class="cost-info-row highlighted">
    <span>Precio de Venta Sugerido:</span>
    <span id="precioVentaSugerido">$0.00</span>
</div>
```

### 2. **Slider Actualizado**

#### Nuevas etiquetas (líneas 423-437):
```html
<!-- ANTES -->
<label class="form-label mb-3">Margen de Ganancia</label>
<span class="text-muted">0%</span>
<span class="text-muted">100%</span>
min="0" max="100"

<!-- DESPUÉS -->
<label class="form-label mb-3">Porcentaje de Costo Deseado</label>
<span class="text-muted">Bajo Costo</span>
<span class="text-muted">Alto Costo</span>
min="1" max="99"
```

### 3. **Nueva Lógica de Cálculo**

#### Función calcularCostos() actualizada (líneas 1107-1123):
```javascript
// ANTES: Fórmula de margen tradicional
const precioSugerido = costoBase * (1 + margen / 100);

// DESPUÉS: Fórmula de porcentaje de costo deseado
const porcentajeCostoDeseado = parseFloat(document.getElementById('margenSlider').value) || 50;
let precioSugerido = 0;
if (porcentajeCostoDeseado > 0) {
    precioSugerido = costoTotalIngredientes / (porcentajeCostoDeseado / 100);
}
```

## Fórmula Implementada

### **Precio Sugerido = Costo Total Ingredientes / (Porcentaje Slider / 100)**

## Ejemplos de Funcionamiento

### Ejemplo 1:
```
Costo Total Ingredientes: $30
Slider: 60% (queremos que el costo sea 60% del precio final)
Precio Sugerido = 30 / (60/100) = 30 / 0.6 = $50.00
Verificación: $30 es el 60% de $50 ✓
```

### Ejemplo 2:
```
Costo Total Ingredientes: $25
Slider: 40% (queremos que el costo sea 40% del precio final)  
Precio Sugerido = 25 / (40/100) = 25 / 0.4 = $62.50
Verificación: $25 es el 40% de $62.50 ✓
```

### Ejemplo 3:
```
Costo Total Ingredientes: $15
Slider: 25% (queremos que el costo sea 25% del precio final)
Precio Sugerido = 15 / (25/100) = 15 / 0.25 = $60.00
Verificación: $15 es el 25% de $60 ✓
```

## Interpretación del Slider

- **Slider bajo (20-30%)**: Precio alto, margen alto (precio premium)
- **Slider medio (40-60%)**: Precio equilibrado  
- **Slider alto (70-90%)**: Precio bajo, margen bajo (precio competitivo)

## Casos de Actualización

El "Precio Sugerido" se recalcula automáticamente cuando:
1. **Se mueve el slider** → Via evento `input`
2. **Cambian los ingredientes** → Via `calcularTotales()`
3. **Se modifican cantidades** → Via `calcularTotales()`

## Estado Final

✅ **Campos ocultados:**
- Cantidad Base
- Costo Total Producto

✅ **Campo visible:**
- Precio de Venta Sugerido (dinámico con slider)

✅ **Nueva funcionalidad:**
- Slider representa porcentaje de costo deseado
- Cálculo inverso para obtener precio sugerido
- Etiquetas actualizadas para mayor claridad
- Rango seguro (1%-99%) para evitar divisiones por cero

**La pestaña de recetas ahora calcula el precio sugerido correctamente basado en el porcentaje de costo deseado.**