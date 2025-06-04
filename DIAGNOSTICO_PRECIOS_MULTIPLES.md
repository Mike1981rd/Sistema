# Diagnóstico: Múltiples Precios No Se Guardan

## 🐛 Problemas Identificados

1. **❌ Formato inconsistente**: Primera fila vs segunda fila tienen diferentes estructuras
2. **❌ Precio base no se guarda**: Solo guarda precio total
3. **❌ Impuestos del segundo nivel**: No guarda todos los impuestos seleccionados
4. **❌ Botón eliminar mal posicionado**: UI inconsistente

## ✅ Correcciones Aplicadas

### 1. **HTML Unificado** 
- ✅ Primera fila actualizada para incluir campo "Nombre del nivel"
- ✅ Estructura de columnas unificada: `col-md-2` + `col-md-2` + `col-md-3` + `col-md-2`
- ✅ Clases CSS consistentes entre primera fila y filas adicionales
- ✅ Botón eliminar correctamente posicionado

### 2. **Logs de Debug Completos**
- ✅ **Frontend**: Logs en `recopilarDatosPrecios()` y `guardarProductoSinImagen()`
- ✅ **Backend**: Logs en `ActualizarProducto()` y `ProcesarPreciosProducto()`

```javascript
// Frontend - logs JSON exacto enviado
console.log('JSON a enviar:', jsonToSend);

// Backend - logs datos recibidos
Console.WriteLine($"[DEBUG] Precio: {precio.NombreNivel} - Base: {precio.PrecioBase}");
```

### 3. **Verificaciones de Integridad**
- ✅ DTO `ProductoActualizarDto` tiene propiedad `Precios` ✓
- ✅ Modelo `ProductoPrecioDto` correcto ✓
- ✅ Función `recopilarDatosPrecios()` lee campos correctos ✓

## 🧪 Plan de Diagnóstico

### Paso 1: Verificar Frontend
1. Abrir F12 → Console
2. Crear/editar producto con 2 niveles de precio:
   ```
   Nivel 1: "Precio Regular" - Base: 100.00 - ITBIS + Propina
   Nivel 2: "Precio VIP" - Base: 85.00 - Solo Propina  
   ```
3. **Verificar logs esperados:**
   ```
   [DEBUG] Iniciando recopilarDatosPrecios...
   [DEBUG] Fila 0: nombre=Precio Regular, base=100, total=128, impuestos=1,2
   [DEBUG] Fila 1: nombre=Precio VIP, base=85, total=93.5, impuestos=2
   [DEBUG] Precio 1: {NombreNivel: "Precio Regular", PrecioBase: 100, ...}
   [DEBUG] Precio 2: {NombreNivel: "Precio VIP", PrecioBase: 85, ...}
   JSON a enviar: {"Precios":[{"NombreNivel":"Precio Regular",...}]}
   ```

### Paso 2: Verificar Backend  
4. **En consola del servidor buscar:**
   ```
   [DEBUG] ActualizarProducto - ID: X
   [DEBUG] Precios recibidos: 2
   [DEBUG] Precio: Precio Regular - Base: 100 - Total: 128 - Impuestos: 1,2
   [DEBUG] Precio: Precio VIP - Base: 85 - Total: 93.5 - Impuestos: 2
   [DEBUG] ProcesarPreciosProducto - ProductoId: X, Precios recibidos: 2
   ```

### Paso 3: Verificar Base de Datos
5. **Query para verificar guardado:**
   ```sql
   SELECT * FROM ProductoVentaPrecios WHERE ProductoVentaId = X;
   SELECT * FROM ProductoVentaPrecioImpuestos WHERE ProductoVentaPrecioId IN (...);
   ```

## 🔍 Posibles Puntos de Falla

### Frontend
- ❓ **Select2 no inicializado**: Segundo nivel devuelve impuestos vacíos
- ❓ **Nombres de campos incorrectos**: `name` attributes no coinciden
- ❓ **jQuery selectors**: No encuentra elementos correctos

### Backend  
- ❓ **Deserialización JSON**: `List<ProductoPrecioDto>` no se mapea
- ❓ **Validación ModelState**: Rechaza datos antes de procesarlos
- ❓ **EntityFramework**: Problema al guardar relaciones

### Base de Datos
- ❓ **Constraints**: Restricciones de FK o validaciones
- ❓ **Transacciones**: Rollback por errores no detectados

## 🎯 Siguientes Pasos

1. **Probar con logs habilitados** y documentar salida exacta
2. **Identificar punto de falla** basado en logs
3. **Aplicar corrección específica** según diagnóstico  
4. **Remover logs de debug** una vez funcional

## 📋 Checklist de Verificación

- [ ] Primera fila tiene campo "Nombre del nivel"
- [ ] Segunda fila tiene mismo formato que primera
- [ ] Select2 funciona en ambas filas
- [ ] Cálculos automáticos funcionan en ambas filas
- [ ] Logs frontend muestran datos correctos
- [ ] Logs backend reciben datos correctos
- [ ] Base de datos guarda precios base y totales
- [ ] Base de datos guarda todos los impuestos por nivel