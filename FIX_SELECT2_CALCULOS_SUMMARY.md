# Corrección de Select2 y Cálculos en Segundo Nivel de Precios

## 🐛 Problema Identificado
El Select2 y los cálculos automáticos no funcionaban en el segundo nivel de precios debido a:

**Error de consola:**
```
Uncaught ReferenceError: updateDeleteButtons is not defined
    at Object.success (gestionar-producto.js:2220:17)
```

## 🔍 Causa Raíz
Las funciones necesarias estaban definidas dentro del scope de `initPricingSystem()`, pero se intentaban llamar desde funciones externas:

1. **`updateDeleteButtons`** - Definida localmente pero llamada globalmente
2. **`addPriceRow`** - Definida localmente pero referenciada globalmente  
3. **`initializePriceRow`** - Definida localmente pero necesaria para nuevas filas

## ✅ Soluciones Implementadas

### 1. **Funciones Accesibles Globalmente**
- ✅ Agregadas al objeto `window` al final de `initPricingSystem()`
- ✅ Variables globales declaradas para scope compartido

```javascript
// Variables globales para el sistema de precios
let priceRowsContainer;
let btnAddPriceRow;
let priceRowIndex = 0;
let impuestosDataGlobal = {};

// Al final de initPricingSystem():
window.updateDeleteButtons = updateDeleteButtons;
window.addPriceRow = addPriceRow;
window.initializePriceRow = initializePriceRow;
```

### 2. **Función agregarFilaPrecio Mejorada**
- ✅ Prioriza usar `window.addPriceRow` si está disponible
- ✅ Fallback completo con template actualizado
- ✅ Inicialización automática con `window.initializePriceRow`
- ✅ Logs de debug para troubleshooting

```javascript
function agregarFilaPrecio() {
    // Usar función global si está disponible
    if (typeof window.addPriceRow === 'function') {
        window.addPriceRow();
        return $('#priceRowsContainer .price-row').last();
    }
    
    // Fallback con template completo e inicialización
}
```

### 3. **Verificación de Funciones**
- ✅ Comprobación de existencia antes de llamar funciones
- ✅ Mensajes de warning si funciones no están disponibles
- ✅ Manejo robusto de errores

```javascript
if (typeof window.updateDeleteButtons === 'function') {
    window.updateDeleteButtons();
} else {
    console.warn('window.updateDeleteButtons no está disponible');
}
```

### 4. **Template Actualizado en Fallback**
- ✅ Incluye campo `NombreNivel` 
- ✅ Estructura de columnas corregida (col-md-2, col-md-3, etc.)
- ✅ Nombres de atributos consistentes
- ✅ Atributos `data-original-name` para binding

## 📋 Archivos Modificados

### JavaScript Principal
- ✅ **`wwwroot/js/productos/gestionar-producto.js`**
  - Variables globales declaradas al inicio
  - Funciones exportadas a `window` object
  - Función `agregarFilaPrecio()` completamente reescrita
  - Verificaciones de existencia agregadas
  - Logs de debug agregados

## 🧪 Cómo Probar la Corrección

### 1. **Test Básico de Segundo Nivel**
1. Ir a crear/editar producto
2. Hacer clic en "Agregar Nivel de Precio" 
3. **Verificar que NO aparece error en consola**
4. **Verificar que Select2 de impuestos funciona en segunda fila**
5. **Verificar que cálculos automáticos funcionan**

### 2. **Test de Cálculos Automáticos**
```
Fila 1: Precio Base = 100, Seleccionar ITBIS (18%)
→ Debe calcular automáticamente Precio Total = 118.00

Fila 2: Precio Base = 85, Seleccionar Solo Propina (10%)  
→ Debe calcular automáticamente Precio Total = 93.50
```

### 3. **Test de Guardado**
1. Configurar múltiples niveles con diferentes precios base
2. Guardar producto
3. Recargar página
4. **Verificar que precios base se mantienen correctos**

### 4. **Verificación en Consola (F12)**
Los logs deben mostrar:
```
[DEBUG] agregarFilaPrecio llamada
[DEBUG] Usando window.addPriceRow
[DEBUG] Inicializando fila con window.initializePriceRow  
[DEBUG] Actualizando botones con window.updateDeleteButtons
```

## 🔄 Flujo de Funcionamiento

```mermaid
graph TD
    A[Usuario hace clic 'Agregar Nivel'] --> B[agregarFilaPrecio()]
    B --> C{window.addPriceRow existe?}
    C -->|Sí| D[window.addPriceRow()]
    C -->|No| E[Crear template manual]
    D --> F[window.initializePriceRow()]
    E --> F
    F --> G[Inicializar Select2]
    G --> H[Configurar eventos de cálculo]
    H --> I[window.updateDeleteButtons()]
    I --> J[Segunda fila completamente funcional]
```

## ⚡ Próximos Pasos

1. **Probar exhaustivamente** los cálculos en múltiples niveles
2. **Verificar guardado** de precios base en base de datos
3. **Remover logs de debug** una vez confirmado que funciona
4. **Validar compatibilidad** con edición de productos existentes

## 🎯 Resultado Esperado

- ✅ **Sin errores de consola** al agregar niveles
- ✅ **Select2 funcional** en todos los niveles
- ✅ **Cálculos automáticos** en todos los niveles  
- ✅ **Guardado correcto** de precios base
- ✅ **Experiencia fluida** para el usuario