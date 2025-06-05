# Fix: Popup "Agregar Item" Solo en Pestaña de Recetas

## Fecha: Enero 2025

## Problema
El popup de "Agregar Item" aparecía incorrectamente al editar cualquier artículo, cuando debería aparecer únicamente en la pestaña de recetas al hacer clic en el botón "Agregar Item".

## Causa del Problema
La función `initRecetasFunctionality()` se ejecutaba inmediatamente al cargar la página, agregando event listeners a botones que podrían no estar en el contexto correcto.

## Solución Implementada

### 1. **Inicialización Diferida**

**Archivo:** `/Views/Productos/GestionarProducto.cshtml`

#### Cambio en la carga inicial (líneas 781-788):
```javascript
// ANTES: Inicialización inmediata
initRecetasFunctionality();

// DESPUÉS: Inicialización solo al activar pestaña
let recetasInitialized = false;
document.getElementById('inventario-tab')?.addEventListener('shown.bs.tab', function() {
    if (!recetasInitialized) {
        initRecetasFunctionality();
        recetasInitialized = true;
    }
});
```

### 2. **Validación en agregarItem()**

#### Verificación de pestaña activa (líneas 868-873):
```javascript
// ANTES: Sin validación
function agregarItem() {
    // Código del popup...
}

// DESPUÉS: Con validación de pestaña
function agregarItem() {
    // Verificar que estamos en la pestaña de recetas
    const inventarioTab = document.getElementById('inventario');
    if (!inventarioTab || !inventarioTab.classList.contains('active')) {
        console.log('[DEBUG] agregarItem() llamado fuera de la pestaña de recetas. Ignorando.');
        return;
    }
    
    // Resto del código...
}
```

## Cómo Funciona la Solución

### ✅ **Inicialización Controlada:**
- Los event listeners de recetas se agregan solo cuando el usuario hace clic en la pestaña "Recetas"
- Usa el evento `shown.bs.tab` de Bootstrap para detectar la activación de la pestaña
- Flag `recetasInitialized` previene múltiples inicializaciones

### ✅ **Doble Validación:**
- **Primera barrera**: Los event listeners solo existen cuando la pestaña ha sido activada
- **Segunda barrera**: La función `agregarItem()` verifica que la pestaña esté actualmente activa

### ✅ **Compatibilidad:**
- Usa `?.` (optional chaining) para evitar errores si los elementos no existen
- Compatible con el sistema de pestañas de Bootstrap 5
- No afecta otras funcionalidades existentes

## Flujo de Funcionamiento

### **Escenario 1: Usuario edita artículo (no va a recetas)**
```
1. Página carga → initRecetasFunctionality() NO se ejecuta
2. Event listeners de recetas NO existen
3. agregarItem() nunca se llama
4. ✅ No hay popup
```

### **Escenario 2: Usuario va a pestaña de recetas**
```
1. Usuario hace clic en pestaña "Recetas"
2. Evento 'shown.bs.tab' se dispara
3. initRecetasFunctionality() se ejecuta una vez
4. Event listeners se agregan a botones de recetas
5. Usuario hace clic en "Agregar Item"
6. agregarItem() verifica que está en pestaña activa
7. ✅ Popup se muestra correctamente
```

### **Escenario 3: Usuario cambia de pestaña después de estar en recetas**
```
1. Usuario sale de pestaña de recetas
2. Pestaña pierde clase 'active'
3. Si agregarItem() se llama por error, la validación lo bloquea
4. ✅ No hay popup fuera de contexto
```

## Elementos de Seguridad

### 🛡️ **Event Listener Protegido:**
```javascript
document.getElementById('inventario-tab')?.addEventListener('shown.bs.tab', ...)
```

### 🛡️ **Validación de Estado:**
```javascript
if (!inventarioTab || !inventarioTab.classList.contains('active')) {
    return;
}
```

### 🛡️ **Flag de Control:**
```javascript
let recetasInitialized = false;
if (!recetasInitialized) { ... }
```

### 🛡️ **Log de Debug:**
```javascript
console.log('[DEBUG] agregarItem() llamado fuera de la pestaña de recetas. Ignorando.');
```

## Estado Final

✅ **Problema resuelto:**
- Popup ya no aparece al editar artículos
- Popup solo aparece en pestaña de recetas
- Funcionalidad de recetas intacta

✅ **Sin efectos secundarios:**
- Otras pestañas funcionan normalmente
- Performance mejorada (menos event listeners al inicio)
- Compatibilidad mantenida

**El popup de "Agregar Item" ahora funciona exclusivamente en el contexto correcto.**