# Fix Crítico: Endpoint Incorrecto para Cargar Impuestos

## Fecha: Enero 2025

## Problema Root Cause Identificado

**El JavaScript estaba llamando a un endpoint que NO EXISTE:**

```javascript
// ❌ INCORRECTO - Este endpoint no existe
url: '/api/impuestos'
data: { search: '', exactId: impuestoId }

// ✅ CORRECTO - Este es el endpoint real
url: '/Impuestos/Buscar'
data: { term: impuestoId.toString(), exactId: true }
```

## Evidencia del Problema

**Del log proporcionado:**
```
[DEBUG] Cargando impuesto 3: 10 % Propina (10%)  // ✅ Se carga
[DEBUG] Cargando impuesto 3: 10 % Propina (10%)  // ❌ Se repite 3 veces!
[DEBUG] Cargando impuesto 3: 10 % Propina (10%)  // ❌ Nunca carga el impuesto 1
```

**Análisis:**
- Se intentan cargar ImpuestoIds `[3, 1]` 
- Solo se encuentra el impuesto 3, repetido para ambas peticiones
- El impuesto 1 (ITBIS 18%) nunca se encuentra

## Correcciones Implementadas

### 1. **Función cargarImpuestosEnFila**
```javascript
// ANTES
$.ajax({
    url: '/api/impuestos',
    data: { search: '', exactId: impuestoId },
    type: 'GET'
})

// DESPUÉS  
$.ajax({
    url: '/Impuestos/Buscar',
    data: { term: impuestoId.toString(), exactId: true },
    type: 'GET'
})
```

### 2. **Select2 Ajax Configuration**
```javascript
// ANTES
ajax: {
    url: '/api/impuestos',
    data: function (params) {
        return {
            search: params.term,
            page: params.page || 1
        };
    }
}

// DESPUÉS
ajax: {
    url: '/Impuestos/Buscar',
    data: function (params) {
        return {
            term: params.term || '',
            exactId: false
        };
    }
}
```

### 3. **Manejo de Respuesta**
```javascript
// ANTES - Asumía estructura incorrecta
if (data && data.length > 0) {
    const impuesto = data[0];
    console.log(`Cargando impuesto ${impuesto.id}: ${impuesto.nombre}`);
}

// DESPUÉS - Maneja estructura correcta del endpoint real
.then(function(response) {
    const data = response.results || response;
    if (data && data.length > 0) {
        const impuesto = data[0];
        console.log(`Cargando impuesto ${impuesto.id}: ${impuesto.text}`);
    }
})
```

## Endpoint Real (ImpuestosController.cs)

**Método:** `Buscar(string term, bool exactId = false)`
**URL:** `/Impuestos/Buscar`
**Parámetros:**
- `term`: ID del impuesto (cuando exactId=true) o término de búsqueda
- `exactId`: true para búsqueda por ID exacto

**Respuesta:**
```json
{
  "results": [
    {
      "id": 1,
      "text": "ITBIS 18%",
      "porcentaje": 18
    }
  ]
}
```

## Resultado Esperado

Con esta corrección:

1. **Para ImpuestoIds `[3, 1]`:**
   - Petición 1: `/Impuestos/Buscar?term=3&exactId=true` → Devuelve impuesto 3 (10% Propina)
   - Petición 2: `/Impuestos/Buscar?term=1&exactId=true` → Devuelve impuesto 1 (ITBIS 18%)

2. **En la interfaz:**
   - Segundo nivel debería mostrar ambos impuestos: 10% Propina + ITBIS 18%
   - Los cálculos deberían funcionar correctamente

## Estado

✅ URLs corregidas en todas las referencias
✅ Parámetros corregidos (term en lugar de search)
✅ Estructura de respuesta manejada correctamente  
✅ Logs detallados para verificar funcionamiento

**Esta corrección debería resolver completamente el problema del segundo nivel que no muestra múltiples impuestos.**