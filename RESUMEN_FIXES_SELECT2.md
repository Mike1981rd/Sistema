# Fix para Select2 en Item Create/Edit

## Problema Inicial
- Los controles Select2 no se inicializaban correctamente
- Aparecían duplicados visuales
- Las características previamente funcionales se rompieron:
  - Funcionalidad de agregar/editar para categorías y marcas
  - Herencia de valores de impuestos y cuentas contables desde categorías
  - Generación automática de código (campo readonly)
  - Generación de código de barras

## Análisis del Problema
1. Se descubrió que múltiples scripts intentaban inicializar los mismos elementos Select2
2. Los endpoints API en los scripts no coincidían con los controladores reales
3. El archivo `contenedores.js` ya estaba inicializando algunos Select2, causando conflictos

## Solución Implementada

### 1. Examinación de Controladores
Revisé `ItemController.cs` para encontrar los endpoints correctos:
- `/Item/GenerarCodigoAutomatico` - para generar códigos automáticos
- `/Item/GenerarCodigoBarras` - para generar códigos de barras (método POST)
- `/Item/ObtenerDatosCategoria` - para obtener datos de categoría incluyendo impuestos y cuentas

### 2. Creación de Scripts Corregidos

#### item-create-fixed.js
- Inicialización correcta de Select2 para categorías, marcas, impuestos y cuentas contables
- Endpoints corregidos para todas las llamadas AJAX
- Generación automática de código implementada
- Generación de código de barras funcionando
- Herencia de categoría para impuestos y cuentas contables
- Funcionalidad de agregar/editar categorías y marcas

#### item-edit-fixed.js
- Versión adaptada para la vista de edición
- Mantiene la funcionalidad existente de los datos
- Mismas correcciones de endpoints
- Maneja la visualización de códigos de barras existentes

### 3. Actualización de Vistas
- `Create.cshtml`: Actualizado para usar `item-create-fixed.js`
- `Edit.cshtml`: Actualizado para usar `item-edit-fixed.js`
- Eliminados scripts conflictivos

## Pruebas Recomendadas
1. **Vista Create**:
   - Verificar generación automática de código
   - Probar generación de código de barras
   - Confirmar herencia de categoría (impuestos y cuentas)
   - Probar creación de nuevas categorías/marcas

2. **Vista Edit**:
   - Verificar que los datos existentes se cargan correctamente
   - Confirmar que los cambios de categoría actualizan los campos heredados
   - Probar modificación de código de barras

## Archivos Modificados
1. `/wwwroot/js/items/item-create-fixed.js` (nuevo)
2. `/wwwroot/js/items/item-edit-fixed.js` (nuevo)
3. `/Views/Item/Create.cshtml` (actualizado)
4. `/Views/Item/Edit.cshtml` (actualizado)

## Endpoints API Utilizados
- `/api/CuentasContables/GetCategorias`
- `/api/CuentasContables/GetMarcas`
- `/api/CuentasContables/GetCuentas`
- `/api/Impuestos/GetImpuestos`
- `/Item/GenerarCodigoAutomatico`
- `/Item/GenerarCodigoBarras`
- `/Item/ObtenerDatosCategoria`