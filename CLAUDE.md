# Documentación del Proyecto - Sistema Contable Aurora

## Dashboard: Guía de Manipulación y Diseño

### Archivos Principales del Dashboard

#### **Archivo Vista Principal**
- **Ubicación**: `/Views/Home/Index.cshtml`
- **Función**: Contiene todo el markup HTML/Razor del dashboard
- **Estructura**: 
  - Sección `@section Styles` con CSS inline
  - Sección `@section Scripts` con JavaScript de Chart.js
  - HTML organizado en filas (rows) con sistema Bootstrap

#### **Archivos de Layout**
- **Layout principal**: `/Views/Shared/_Layout.cshtml`
- **Secciones requeridas**:
  - `@RenderSection("Styles", required: false)`
  - `@RenderSection("VendorStyles", required: false)`
  - `@RenderSection("Scripts", required: false)`
  - `@RenderSection("VendorScripts", required: false)`
  - `@RenderSection("PageScripts", required: false)`

### Estructura Actual del Dashboard

#### **Fila 1: Métricas Principales**
```html
<div class="row">
  <!-- Card grande Ventas (col-xl-4) -->
  <!-- 3 Cards pequeños (col-lg-4 cada uno) -->
</div>
```
- **Card de Ventas**: Estilo gradient púrpura, botón "Ver Detalles"
- **3 Cards estadísticas**: Impuestos de ventas, Customers, Products

#### **Fila 2: Inventario y Compras**
```html
<div class="row">
  <!-- Card Inventario (col-md-6) - estilo "Popular Products" -->
  <!-- Card Compras (col-md-6) - estilo "Orders by Countries" -->
</div>
```

#### **Fila 3: Espacio Futuro**
```html
<div class="row mt-4">
  <!-- Placeholder para futuros widgets -->
</div>
```

### Guía de Estilos CSS

#### **Clases Base de Cards**
```css
.vuexy-card {
  background: #fff;
  border-radius: 0.5rem;
  box-shadow: 0 0.25rem 1.125rem rgba(75, 70, 92, 0.1);
  transition: all 0.3s ease-in-out;
  height: 100%;
}
```

#### **Colores del Sistema Vuexy**
- **Púrpura principal**: `#7367f0`
- **Verde éxito**: `#28c76f`
- **Naranja warning**: `#ff9f43`
- **Azul info**: `#00cfe8`
- **Rojo danger**: `#ea5455`
- **Grises**: `#5e5873`, `#6e6b7b`
- **Fondo**: `#f5f5f9`

#### **Clases de Iconos**
```css
.stats-icon-wrapper {
  width: 42px;
  height: 42px;
  border-radius: 0.375rem;
  font-size: 1.25rem;
}
.stats-icon-primary { background: rgba(115, 103, 240, 0.12); color: #7367f0; }
.stats-icon-success { background: rgba(40, 199, 111, 0.12); color: #28c76f; }
.stats-icon-warning { background: rgba(255, 159, 67, 0.12); color: #ff9f43; }
.stats-icon-info { background: rgba(0, 207, 232, 0.12); color: #00cfe8; }
```

### Tipos de Cards Implementados

#### **1. Card de Ventas (Congratulations Style)**
- **Clase**: `.congratulations-card`
- **Características**: Gradient púrpura, texto blanco, botón translúcido
- **Uso**: Métricas principales destacadas

#### **2. Cards de Estadísticas Simples**
- **Clase**: `.stats-card`
- **Características**: Número grande, título, icono en esquina
- **Layout**: Flexbox con `justify-content-between`

#### **3. Card de Inventario (Popular Products Style)**
- **Características**: 
  - Header con título y dropdown
  - Lista de items con avatares circulares
  - Colores de fondo para avatares
  - Formato de moneda en valores

#### **4. Card de Compras (Orders by Countries Style)**
- **Características**:
  - Tabs de navegación (`.compras-tabs`)
  - Estados con badges circulares
  - Información de suplidor y fechas
  - Iconos de estado (check, clock, truck)

### Manipulación de Gráficos

#### **Chart.js Configuración**
- **CDN**: `https://cdn.jsdelivr.net/npm/chart.js@4.4.0/dist/chart.umd.min.js`
- **Configuración global**: Font family, colores base
- **Tipos implementados**: Line charts (mini), Bar charts, Radial SVG

#### **Gráficos SVG Radiales**
```html
<svg width="100" height="100">
  <circle cx="50" cy="50" r="40" fill="none" stroke="#f0f0f0" stroke-width="8"/>
  <circle cx="50" cy="50" r="40" fill="none" stroke="#color" stroke-width="8"
          stroke-dasharray="226" stroke-dashoffset="valor" stroke-linecap="round"/>
</svg>
```

### Proceso para Agregar Nuevos Cards

#### **1. Planificación**
- Definir tipo de card (estadística, lista, gráfico)
- Identificar fila de destino
- Determinar ancho de columna Bootstrap

#### **2. HTML Structure**
```html
<div class="col-md-X mb-4">
  <div class="card vuexy-card [clase-especifica]">
    <div class="card-header"> <!-- Opcional -->
      <!-- Título y dropdown -->
    </div>
    <div class="card-body">
      <!-- Contenido del card -->
    </div>
  </div>
</div>
```

#### **3. CSS Específico**
- Agregar clases en la sección `@section Styles`
- Seguir naming convention: `.nombre-card`
- Usar colores del sistema Vuexy
- Mantener responsive design

#### **4. JavaScript (si requiere gráficos)**
- Agregar en la sección `@section Scripts`
- Usar Chart.js para gráficos complejos
- SVG inline para gráficos radiales simples

### Mejores Prácticas

#### **Responsividad**
- Usar clases Bootstrap: `col-xl-4 col-lg-6 col-md-6`
- Probar en diferentes tamaños de pantalla
- Ajustar padding/margin para móvil

#### **Performance**
- CSS inline en `@section Styles` (evita archivos externos)
- JavaScript al final en `@section Scripts`
- Lazy load para gráficos complejos

#### **Mantenimiento**
- Documentar cambios en CLAUDE.md
- Usar nombres de clases descriptivos
- Mantener consistencia con colores Vuexy
- Comentar secciones complejas

#### **Datos**
- Usar formato de moneda: `$XXX,XXX.XX`
- Fechas en formato contable: `DD Mmm YYYY`
- Números con separadores de miles: `X,XXX`
- Porcentajes: `XX.X%`

### Comandos Útiles para Testing

#### **Verificar Compilación**
```bash
# Si hay errores de Razor, revisar:
# - Escape de @ en CSS: @@media
# - Comillas en atributos HTML
# - Secciones cerradas correctamente
```

#### **Debugging de Layout**
- Usar DevTools para inspeccionar grid Bootstrap
- Verificar clases CSS aplicadas
- Comprobar responsive breakpoints

## Arquitectura General

### Patrón y Estructura
- **Arquitectura**: MVC tradicional de ASP.NET Core con elementos del patrón Repository
- **Base de Datos**: PostgreSQL con Entity Framework Core
- **Frontend**: Razor Views con jQuery, Bootstrap 5 y Select2

### Estructura de Carpetas
```
/Controllers    - Controladores MVC (26+ controladores)
/Models         - Modelos de dominio y ViewModels
/Views          - Vistas Razor organizadas por controlador
/Services       - Servicios de negocio (EmpresaService, ImpuestoService, etc.)
/Repositories   - Implementación del patrón Repository (selectivo)
/Data           - ApplicationDbContext y migraciones (70+ archivos)
/wwwroot        - Recursos estáticos organizados por módulo
```

## Manejo de Multi-Empresa (EmpresaId) - ACTUALIZADO

### Implementación Actual (Enero 2025)
- **Servicio Centralizado**: `IEmpresaService` / `EmpresaService`
- **Estado Actual**: Lee desde `appsettings.json` → `AppSettings:EmpresaUnicaId`
- **Configuración**: `/appsettings.json` contiene `"EmpresaUnicaId": 4`

### Archivo de Configuración
```json
{
  "AppSettings": {
    "EmpresaUnicaId": 4
  }
}
```

### Implementación del Servicio
```csharp
// Services/EmpresaService.cs
public class EmpresaService : IEmpresaService
{
    private readonly int _empresaActualId;
    
    public EmpresaService(IConfiguration configuration)
    {
        _empresaActualId = configuration.GetValue<int>("AppSettings:EmpresaUnicaId");
        
        if (_empresaActualId == 0)
        {
            throw new InvalidOperationException("EmpresaUnicaId requerido en appsettings.json");
        }
    }
    
    public async Task<int> ObtenerEmpresaActualId()
    {
        return await Task.FromResult(_empresaActualId);
    }
    
    public int ObtenerEmpresaActualIdSincrono()
    {
        return _empresaActualId;
    }
}
```

### Patrón de Uso en Controladores
```csharp
// 1. Inyección de dependencias
private readonly IEmpresaService _empresaService;

// 2. En cada método
var empresaId = await _empresaService.ObtenerEmpresaActualId();

// 3. Filtrado en consultas
.Where(x => x.EmpresaId == empresaId)

// 4. Asignación en creates
entidad.EmpresaId = empresaId;
```

### Ventajas del Nuevo Sistema
- ✅ **Configurable**: Se puede cambiar sin recompilar
- ✅ **Validación**: Falla rápido si no está configurado
- ✅ **Logging**: Registra el EmpresaId en uso
- ✅ **Mantenible**: Centralizado en un solo lugar
- ✅ **Preparado**: Para futuro multi-empresa

## Módulos Principales Funcionando

### 1. Catálogo de Cuentas
- **Controller**: CatalogoController, CatalogoCuentasController
- **Características**: Manejo jerárquico, Repository pattern
- **Filtrado correcto por empresaId**

### 2. Proveedores
- **Controller**: ProveedoresController
- **Características**: Extensión del modelo Cliente con flag EsProveedor
- **Validación robusta de empresaId**

### 3. Productos
- **Controller**: ProductosController
- **Características**: Herencia de datos desde categorías
- **Select2 implementado con endpoints AJAX**

## Configuración Select2

### Patrón JavaScript Genérico
```javascript
$('.select2-selector').select2({
    theme: 'bootstrap-5',
    width: '100%',
    ajax: {
        url: '/Controller/BuscarDatos',
        dataType: 'json',
        delay: 250
    }
});
```

### Endpoint en Controlador Genérico
```csharp
[HttpGet]
public async Task<IActionResult> BuscarDatos(string term)
{
    var empresaId = await _empresaService.ObtenerEmpresaActualId();
    // Filtrar por empresaId y term
    return Json(new { results = datos });
}
```

## Implementación Select2 para Cuentas Contables (PATRÓN ESTÁNDAR)

Este es el patrón más utilizado en el sistema para seleccionar cuentas contables. Se implementa en múltiples módulos como Familia, EntradaDiario, Productos, etc.

### 1. HTML en la Vista (Razor)
```html
<div class="form-group">
    <label asp-for="CuentaVentasId" class="control-label">Cuenta de Ventas</label>
    <select asp-for="CuentaVentasId" class="form-control select-cuenta">
        <option value="">Seleccione una cuenta</option>
        @if (Model.CuentaVentasId.HasValue)
        {
            <!-- Opción preseleccionada para Edit -->
            <option value="@Model.CuentaVentasId" selected>Cargando...</option>
        }
    </select>
    <span asp-validation-for="CuentaVentasId" class="text-danger"></span>
</div>
```

### 2. JavaScript Select2 con AJAX
```javascript
@section Scripts {
    <script>
        $(document).ready(function() {
            // Inicializar Select2 para cuentas contables
            $('.select-cuenta').select2({
                theme: 'bootstrap-5',
                placeholder: 'Buscar cuenta contable...',
                allowClear: true,
                ajax: {
                    url: '/EntradaDiario/BuscarCuentasContables',
                    dataType: 'json',
                    delay: 250,
                    data: function(params) {
                        return { 
                            term: params.term || ''
                        };
                    },
                    processResults: function(data) {
                        return data; // El endpoint ya devuelve { results: [...] }
                    },
                    cache: true
                },
                minimumInputLength: 1
            });

            // Para Edit: Cargar las opciones preseleccionadas
            @if (Model.CuentaVentasId.HasValue)
            {
                <text>
                // Cargar cuenta preseleccionada
                $.ajax({
                    url: '/EntradaDiario/BuscarCuentasContables',
                    data: { term: '' },
                    success: function(data) {
                        var cuenta = data.results.find(c => c.id == @Model.CuentaVentasId);
                        if (cuenta) {
                            var option = new Option(cuenta.text, cuenta.id, true, true);
                            $('#CuentaVentasId').append(option).trigger('change');
                        }
                    }
                });
                </text>
            }
        });
    </script>
}
```

### 3. Endpoint en EntradaDiarioController
```csharp
// GET: EntradaDiario/BuscarCuentasContables
[HttpGet]
public async Task<IActionResult> BuscarCuentasContables(string term)
{
    try
    {
        term = term ?? string.Empty;
        
        if (string.IsNullOrEmpty(term))
        {
            return Json(new { results = new List<object>() });
        }
        
        // Usar el repositorio que NO filtra por EmpresaId
        var cuentas = await _cuentaContableRepository.BuscarPorNombreOCodigoAsync(term);
        
        var results = cuentas.Select(c => new
        {
            id = c.Id,
            text = $"{c.Codigo} - {c.Nombre}",
            codigo = c.Codigo,
            nombre = c.Nombre
        }).ToList();
        
        return Json(new { results = results });
    }
    catch (Exception ex)
    {
        return Json(new { results = new List<object>() });
    }
}
```

### 4. Repositorio CuentaContableRepository
```csharp
public async Task<IEnumerable<CuentaContable>> BuscarPorNombreOCodigoAsync(string termino)
{
    termino = termino?.Trim().ToLower() ?? "";
    
    var resultados = await _context.CuentasContables
        .Where(c => EF.Functions.Like(c.Codigo.ToLower(), $"%{termino}%") || 
                    EF.Functions.Like(c.Nombre.ToLower(), $"%{termino}%"))
        .OrderBy(c => c.Codigo)
        .Take(20)
        .ToListAsync();
    
    return resultados;
}
```

### 5. Alternativa: Select2 con Datos Precargados (Para Create)

Si prefieres cargar todas las cuentas de una vez (útil cuando no hay muchas):

#### En el Controlador:
```csharp
private async Task CargarCuentasContablesDisponibles(FamiliaViewModel viewModel)
{
    var cuentasContables = await _context.CuentasContables
        .Where(c => c.Activo && c.EmpresaId == viewModel.EmpresaId)
        .OrderBy(c => c.Codigo)
        .Select(c => new { 
            Id = c.Id, 
            Descripcion = $"{c.Codigo} - {c.Descripcion ?? c.Nombre}" 
        })
        .ToListAsync();

    viewModel.CuentasVentasDisponibles = new SelectList(
        cuentasContables, "Id", "Descripcion", viewModel.CuentaVentasId);
}
```

#### En la Vista:
```html
<select asp-for="CuentaVentasId" asp-items="Model.CuentasVentasDisponibles" 
        class="form-control select-cuenta">
    <option value="">Seleccione una cuenta</option>
</select>
```

#### JavaScript más simple:
```javascript
$('.select-cuenta').select2({
    theme: 'bootstrap-5',
    placeholder: 'Buscar cuenta contable...',
    allowClear: true,
    minimumInputLength: 1
});
```

### 6. Consideraciones Importantes

1. **Filtrado por EmpresaId**: 
   - El endpoint AJAX actual NO filtra por empresa
   - La versión precargada SÍ filtra por empresa
   - Decidir según requerimientos de seguridad

2. **Performance**:
   - AJAX es mejor para muchas cuentas (carga bajo demanda)
   - Precargado es más rápido para pocas cuentas

3. **Formato de Display**:
   - Estándar: `{Codigo} - {Nombre}`
   - Alternativa: `{Codigo} - {Descripción ?? Nombre}`

4. **Validación**:
   - Siempre incluir `asp-validation-for`
   - El campo puede ser nullable (`int?`)

5. **Múltiples Select2 en la misma vista**:
   - Usar la misma clase `.select-cuenta` para todos
   - Se inicializan todos con una sola llamada jQuery

### 7. Troubleshooting Común

1. **Select2 no se inicializa**: Verificar que jQuery y Select2 estén cargados antes
2. **No aparecen resultados**: Verificar que el endpoint devuelve formato `{ results: [...] }`
3. **Valor preseleccionado no carga**: Asegurar que el Option existe en el HTML antes de Select2
4. **Error 500 en búsqueda**: El endpoint debe devolver array vacío, no error

## 🚨 FIX CRÍTICO: Problema en Vistas Edit con Select2 Cuentas Contables

### **Problema Identificado en Enero 2025**
En las vistas **Edit**, los Select2 de cuentas contables no cargan los valores preseleccionados correctamente. Los síntomas son:
- ✅ **Create**: Funciona correctamente
- ❌ **Edit**: Muestra "Cargando..." y no selecciona la cuenta
- ❌ **Edit**: Al elegir una cuenta, no se selecciona

### **Causa Raíz**
El endpoint `/EntradaDiario/BuscarCuentasContables` devuelve **lista vacía** cuando `term` está vacío:

```csharp
// ❌ PROBLEMA: Este código devuelve lista vacía cuando term = ""
if (string.IsNullOrEmpty(term))
{
    return Json(new { results = new List<object>() });
}
```

**JavaScript problemático en Edit:**
```javascript
// ❌ ESTO NO FUNCIONA en Edit
$.ajax({
    url: '/EntradaDiario/BuscarCuentasContables',
    data: { term: '' }, // Devuelve lista vacía
    success: function(data) {
        var cuenta = data.results.find(c => c.id == @Model.CuentaId); // No encuentra nada
    }
});
```

### **Solución Implementada**

**1. Nuevo Endpoint Específico para Obtener por ID:**
```csharp
// ✅ SOLUCIÓN: Nuevo endpoint en EntradaDiarioController
[HttpGet]
public async Task<IActionResult> ObtenerCuentaContable(int id)
{
    try
    {
        if (id <= 0)
        {
            return Json(new { success = false, message = "ID inválido" });
        }

        var cuenta = await _dbContext.CuentasContables
            .Where(c => c.Id == id)
            .Select(c => new
            {
                id = c.Id,
                text = $"{c.Codigo} - {c.Nombre}",
                codigo = c.Codigo,
                nombre = c.Nombre
            })
            .FirstOrDefaultAsync();

        if (cuenta == null)
        {
            return Json(new { success = false, message = "Cuenta no encontrada" });
        }

        return Json(new { success = true, cuenta = cuenta });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = "Error interno del servidor" });
    }
}
```

**2. JavaScript Corregido en Edit:**
```javascript
// ✅ SOLUCIÓN: Usar el nuevo endpoint específico
@if (Model.CuentaContableVentasId > 0)
{
    <text>
    $.ajax({
        url: '/EntradaDiario/ObtenerCuentaContable', // ✅ Endpoint específico
        data: { id: @Model.CuentaContableVentasId }, // ✅ Buscar por ID específico
        success: function(response) {
            if (response.success && response.cuenta) {
                // Limpiar opciones existentes primero
                $('#CuentaContableVentasId').empty();
                // Agregar opción vacía
                $('#CuentaContableVentasId').append(new Option('Seleccione una cuenta', '', false, false));
                // Agregar y seleccionar la cuenta actual
                var newOption = new Option(response.cuenta.text, response.cuenta.id, true, true);
                $('#CuentaContableVentasId').append(newOption).trigger('change');
            }
        }
    });
    </text>
}
```

**3. Configuración Select2 Mejorada:**
```javascript
$('.select-cuenta').select2({
    // ... configuración AJAX normal ...
    templateResult: function(item) {
        if (item.loading) return item.text;
        return item.text || item.codigo + ' - ' + item.nombre;
    },
    templateSelection: function(item) {
        return item.text || item.codigo + ' - ' + item.nombre;
    }
});
```

### **Implementación Estándar Final**

**HTML en Edit:**
```html
<select asp-for="CuentaContableVentasId" class="form-select select-cuenta">
    <option value="">Seleccione una cuenta</option>
    @if (Model.CuentaContableVentasId > 0)
    {
        <option value="@Model.CuentaContableVentasId" selected>Cargando...</option>
    }
</select>
```

### **Patrón a Seguir en Futuros Módulos**

**Para Create:**
- ✅ Usar AJAX con `/EntradaDiario/BuscarCuentasContables`
- ✅ `minimumInputLength: 1`

**Para Edit:**
- ✅ Usar AJAX con `/EntradaDiario/ObtenerCuentaContable` para cargar preseleccionados
- ✅ Usar AJAX con `/EntradaDiario/BuscarCuentasContables` para búsqueda normal
- ✅ Verificar con `> 0` en lugar de `.HasValue` para campos `int`

**Archivos Afectados:**
- `/Controllers/EntradaDiarioController.cs` - Agregado método `ObtenerCuentaContable`
- `/Views/Impuestos/Edit.cshtml` - JavaScript actualizado
- `/Views/Impuestos/Create.cshtml` - Mejoras aplicadas

**⚠️ IMPORTANTE**: Este fix es **CRÍTICO** y debe aplicarse a cualquier módulo que use Select2 con cuentas contables en vistas Edit.

## Patrones de Desarrollo

### Modelo Base
```csharp
public class Entidad : BaseEntity
{
    public int EmpresaId { get; set; }
    public virtual Empresa? Empresa { get; set; }
}
```

### Patrón CRUD Estándar
1. **Index**: Filtrar por empresaId
2. **Create**: Asignar empresaId y FechaCreacion
3. **Edit**: Mantener empresaId original
4. **Delete**: Soft delete con campo Activo

### Manejo de Errores
```csharp
try
{
    await _context.SaveChangesAsync();
    return Json(new { success = true });
}
catch (Exception ex)
{
    return Json(new { success = false, message = ex.Message });
}
```

## Consideraciones Importantes

### ⚠️ Aspectos Críticos
1. **NO hay autenticación** implementada actualmente
2. **EmpresaId hardcodeado** a 4 en EmpresaService
3. **Todas las fechas** deben usar DateTime.UtcNow
4. **Soft delete** preferido sobre eliminación física

### ✅ Buenas Prácticas del Proyecto
1. **SIEMPRE** filtrar por empresaId en queries
2. **SIEMPRE** validar empresaId != 0 antes de operaciones
3. **INCLUIR** relaciones necesarias con .Include()
4. **Organizar JS** por módulo en /wwwroot/js/[modulo]/
5. **Usar Select2** para búsquedas complejas con endpoints AJAX

### 📋 Checklist para Nuevos Módulos
1. Modelo hereda de BaseEntity con EmpresaId
2. Agregar DbSet en ApplicationDbContext
3. Crear migración
4. Controlador inyecta ApplicationDbContext y IEmpresaService
5. Crear carpeta de vistas en /Views/[Controlador]/
6. Crear archivos JS en /wwwroot/js/[modulo]/
7. Implementar filtrado por empresaId en TODAS las queries
8. Asignar empresaId en TODOS los creates
9. Usar DataAnnotations para validaciones
10. Implementar manejo de errores con try-catch

## Tecnologías y Versiones
- ASP.NET Core MVC
- Entity Framework Core con PostgreSQL
- Bootstrap 5
- jQuery
- Select2
- Sin sistema de autenticación actualmente

## Estándar de Diseño para Vistas Index (PATRÓN COMPROBANTES)

Este es el diseño estándar para la mayoría de las vistas Index en el sistema. Basado en el módulo de Comprobantes Fiscales.

### 1. Estructura HTML Base
```html
<div class="container-fluid p-0">
    <!-- Encabezado con título y breadcrumb -->
    <div class="row mb-4">
        <div class="col-12">
            <h1 class="h4">[Título del Módulo]</h1>
            <nav class="breadcrumb-container d-none d-sm-block d-lg-inline-block">
                <ol class="breadcrumb pt-0">
                    <li class="breadcrumb-item">
                        <a asp-controller="Home" asp-action="Index">Dashboard</a>
                    </li>
                    <li class="breadcrumb-item active">[Módulo Actual]</li>
                </ol>
            </nav>
        </div>
    </div>

    <!-- Card con pestañas Activos/Inactivos -->
    <div class="card mb-4">
        <div class="card-header">
            <ul class="nav border-0">
                <li class="nav-item">
                    <a class="nav-link @(ViewBag.Tab == "Activos" || ViewBag.Tab == null ? "active" : "")" 
                       asp-action="Index" asp-route-tab="Activos">
                        Activos
                        @if (ViewBag.Tab == "Activos" || ViewBag.Tab == null)
                        {
                            <span class="position-absolute" style="bottom: 0; left: 0; right: 0; height: 2px; background-color: #007bff;"></span>
                        }
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link @(ViewBag.Tab == "Inactivos" ? "active" : "")" 
                       asp-action="Index" asp-route-tab="Inactivos">
                        Inactivos
                        @if (ViewBag.Tab == "Inactivos")
                        {
                            <span class="position-absolute" style="bottom: 0; left: 0; right: 0; height: 2px; background-color: #007bff;"></span>
                        }
                    </a>
                </li>
            </ul>
        </div>
    </div>

    <!-- Alertas de éxito/error -->
    @if (TempData["SuccessMessage"] != null)
    {
        <div class="alert alert-success alert-dismissible fade show">
            <i class="fas fa-check-circle me-2"></i> @TempData["SuccessMessage"]
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    }

    <!-- Barra de acciones -->
    <div class="row mb-3">
        <div class="col-md-4">
            <!-- Espacio para búsqueda futura -->
        </div>
        <div class="col-md-8 text-end">
            <!-- Dropdown Exportar -->
            <div class="dropdown d-inline-block me-2">
                <button type="button" class="btn btn-sm btn-outline-secondary dropdown-toggle" 
                        data-bs-toggle="dropdown">
                    <i class="fas fa-file-export me-1"></i> Exportar
                </button>
                <ul class="dropdown-menu dropdown-menu-end">
                    <li><button class="dropdown-item" id="export-excel">
                        <i class="fas fa-file-excel me-2 text-success"></i>Excel
                    </button></li>
                    <li><button class="dropdown-item" id="export-pdf">
                        <i class="fas fa-file-pdf me-2 text-danger"></i>PDF
                    </button></li>
                    <li><button class="dropdown-item" id="export-csv">
                        <i class="fas fa-file-csv me-2 text-primary"></i>CSV
                    </button></li>
                </ul>
            </div>
            <!-- Botón Nuevo -->
            <a asp-action="Create" class="btn btn-primary btn-sm px-3">
                <i class="fas fa-plus me-1"></i> Nuevo [elemento]
            </a>
        </div>
    </div>

    <!-- Tabla principal -->
    <div class="card">
        <div class="card-body">
            <div class="table-responsive">
                <table id="[modulo]Table" class="table table-striped table-hover">
                    <thead>
                        <tr>
                            <th>Columnas...</th>
                            <th class="text-center">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        <!-- Contenido -->
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>
```

### 2. Estilos CSS Estándar
```css
@section Styles {
    <style>
        /* Botón primario rectangular */
        .btn-primary.btn-sm {
            background-color: #0A1172 !important;
            border-color: #0A1172 !important;
            border-radius: 4px !important;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.15) !important;
        }

        /* Botones de acción sin fondo */
        .btn-group {
            display: inline-flex !important;
            justify-content: center !important;
            gap: 0 !important;
        }

        .btn-group .btn {
            padding: 0.25rem 0.5rem !important;
            margin: 0 !important;
            border: none;
            background: transparent;
            transition: transform 0.2s, color 0.2s;
        }
        
        /* Forzar que no haya espacio entre botones */
        .btn-group > .btn:not(:first-child) {
            margin-left: -1px !important;
        }

        .btn-group .btn:hover {
            transform: scale(1.2);
        }

        /* Hover en filas */
        .table-hover tbody tr:hover {
            background-color: rgba(0, 123, 255, 0.05);
            cursor: pointer;
        }

        /* Pestañas activas */
        .nav-link.active {
            border-bottom: 2px solid #007bff !important;
            font-weight: 500;
        }

        /* Fondo principal */
        .aurora-main {
            background-color: #f5f7fa;
            min-height: calc(100vh - 60px);
            padding: 20px;
        }
    </style>
}
```

### 3. Botones de Acción Estándar
```html
<td class="text-center">
    <div class="btn-group">
        <!-- Editar (siempre visible) -->
        <a asp-action="Edit" asp-route-id="@item.Id" class="btn btn-sm text-primary" title="Editar">
            <i class="fas fa-edit"></i>
        </a>
        
        <!-- Para elementos activos -->
        @if (item.Activo)
        {
            <button class="btn btn-sm text-danger" onclick="confirmarCambioEstado(@item.Id, '@item.Nombre', true)" title="Desactivar">
                <i class="fas fa-ban"></i>
            </button>
        }
        else
        {
            <button class="btn btn-sm text-success" onclick="confirmarCambioEstado(@item.Id, '@item.Nombre', false)" title="Activar">
                <i class="fas fa-check-circle"></i>
            </button>
        }
    </div>
</td>
```

### 4. Modal de Confirmación Estándar
```html
<div class="modal fade" id="modalConfirmacion" tabindex="-1">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Confirmar acción</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body">
                <p id="mensajeConfirmacion"></p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn" data-bs-dismiss="modal" 
                        style="background:#787276;color:#FFFFFF;">Cancelar</button>
                <form asp-action="ToggleEstado" method="post" id="formToggleEstado">
                    <input type="hidden" id="elementoId" name="id" />
                    <button type="submit" class="btn" 
                            style="background:#007848;color:#FFFFFF;">Confirmar</button>
                </form>
            </div>
        </div>
    </div>
</div>
```

### 5. JavaScript con DataTables
```javascript
@section Scripts {
    <script>
        $(document).ready(function () {
            $('#[modulo]Table').DataTable({
                language: {
                    url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json'
                },
                dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>' +
                     '<"row"<"col-sm-12"tr>>' +
                     '<"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
                buttons: [
                    {
                        extend: 'excel',
                        exportOptions: { columns: ':not(:last-child)' }
                    },
                    {
                        extend: 'pdf',
                        exportOptions: { columns: ':not(:last-child)' }
                    },
                    {
                        extend: 'csv',
                        exportOptions: { columns: ':not(:last-child)' }
                    }
                ]
            });

            // Conectar botones de exportación
            $('#export-excel').on('click', function() {
                $('#[modulo]Table').DataTable().button(0).trigger();
            });
            $('#export-pdf').on('click', function() {
                $('#[modulo]Table').DataTable().button(1).trigger();
            });
            $('#export-csv').on('click', function() {
                $('#[modulo]Table').DataTable().button(2).trigger();
            });
        });

        function confirmarCambioEstado(id, nombre, estaActivo) {
            document.getElementById('elementoId').value = id;
            var accion = estaActivo ? 'desactivar' : 'activar';
            document.getElementById('mensajeConfirmacion').textContent = 
                `¿Está seguro que desea ${accion} "${nombre}"?`;
            var modal = new bootstrap.Modal(document.getElementById('modalConfirmacion'));
            modal.show();
        }
    </script>
}
```

### 6. Colores y Estilos Corporativos
- **Azul primario**: `#0A1172` (botones principales)
- **Verde confirmación**: `#007848` (botón confirmar)
- **Gris cancelar**: `#787276` (botón cancelar)
- **Fondo general**: `#f5f7fa`
- **Hover filas**: `rgba(0, 123, 255, 0.05)`

### 7. Iconos FontAwesome Estándar
- **Nuevo**: `fas fa-plus`
- **Editar**: `fas fa-edit` (azul)
- **Desactivar**: `fas fa-ban` (rojo)
- **Activar**: `fas fa-check-circle` (verde)
- **Exportar**: `fas fa-file-export`
- **Excel**: `fas fa-file-excel` (verde)
- **PDF**: `fas fa-file-pdf` (rojo)
- **CSV**: `fas fa-file-csv` (azul)
- **Success**: `fas fa-check-circle`
- **Error**: `fas fa-exclamation-circle`

### 8. Características Importantes
1. **Pestañas Activos/Inactivos** con línea azul inferior
2. **Breadcrumb** para navegación
3. **Alertas dismissibles** con iconos
4. **DataTables** en español con exportación
5. **Modal Bootstrap 5** para confirmaciones
6. **Botones sin bordes** con efecto hover scale
7. **Responsive** con table-responsive
8. **Container-fluid** sin padding

### 9. Controlador - Patrón para Index
```csharp
public async Task<IActionResult> Index(string tab)
{
    var empresaId = await _empresaService.ObtenerEmpresaActualId();
    bool activos = string.IsNullOrEmpty(tab) || tab == "Activos";
    
    ViewBag.Tab = activos ? "Activos" : "Inactivos";
    
    var elementos = await _context.Elementos
        .Where(e => e.EmpresaId == empresaId && e.Activo == activos)
        .ToListAsync();
        
    return View(elementos);
}

[HttpPost]
public async Task<IActionResult> ToggleEstado(int id)
{
    var elemento = await _context.Elementos.FindAsync(id);
    if (elemento != null)
    {
        elemento.Activo = !elemento.Activo;
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Estado actualizado correctamente";
    }
    
    return RedirectToAction(nameof(Index), new { tab = elemento.Activo ? "Activos" : "Inactivos" });
}
```

Este estándar asegura consistencia visual y funcional en todo el sistema.

## Guía de Implementación Completa - Lecciones del Módulo Impuestos

Esta sección documenta todos los problemas encontrados y sus soluciones al implementar el estándar de Comprobantes en el módulo de Impuestos.

### 1. Problema del Selector de Registros (DataTables)

**Problema**: La flecha del dropdown del selector "Mostrar X registros" tapa el número.

**Solución**: Agregar estos estilos CSS específicos en la sección `@section Styles`:

```css
/* Fix para el selector de registros - evitar que la flecha tape el número */
.dataTables_length label {
    display: flex !important;
    align-items: center !important;
    gap: 0.5rem !important;
}

.dataTables_length select {
    display: inline-block !important;
    width: auto !important;
    padding-right: 30px !important;
}
```

### 2. Problema de Espaciado de Iconos de Acción

**Problema**: Los iconos de editar y activar/desactivar tenían demasiado espacio entre ellos. Además, el área clickeable del botón de editar se extendía al espacio vacío.

**Solución**: CSS que controla exactamente el área clickeable de cada botón:

```css
/* Ajuste FORZADO de espaciado de iconos */
.btn-group {
    display: flex !important;
    width: auto !important;
    justify-content: center !important;
    gap: 0 !important;
}

.btn-group .btn {
    padding: 0.25rem !important;
    margin: 0 !important;
    border: none !important;
    background: transparent !important;
    width: auto !important;
    min-width: auto !important;
    display: inline-flex !important;
    align-items: center !important;
    justify-content: center !important;
}

.btn-group .btn i {
    margin: 0 !important;
    padding: 0 !important;
}
```

**Importante**: 
- `width: auto` y `min-width: auto` controlan el área clickeable exacta
- `display: inline-flex` con `align-items: center` centra perfectamente los iconos
- CSS específico para los iconos (`i`) elimina spacing interno
- Evita que el área clickeable se extienda más allá del icono visible

**⚠️ PROBLEMA COMÚN: Espaciado Inconsistente por Botones Faltantes**

Si los iconos siguen teniendo espaciado incorrecto después de aplicar el CSS, verificar que el HTML tenga la **misma estructura** que el módulo Comprobantes:

```html
<!-- ✅ ESTRUCTURA CORRECTA (3 botones, 1 oculto) -->
<div class="btn-group">
    <a><!-- Editar --></a>
    <button class="view-details d-none"><!-- Botón oculto --></button>
    <button><!-- Activar/Desactivar --></button>
</div>
```

**CSS requerido para ocultar el botón del medio:**
```css
.view-details {
    display: none !important;
    width: 0 !important;
    height: 0 !important;
    padding: 0 !important;
    margin: 0 !important;
    border: 0 !important;
    position: absolute !important;
    overflow: hidden !important;
    clip: rect(0 0 0 0) !important;
}
```

**Módulos afectados por este problema:**
- ✅ **Comprobantes**: Tiene el botón oculto desde el inicio
- ✅ **Impuestos**: Agregado el botón oculto para consistencia

### 3. Problema de Idioma de DataTables (Error CORS)

**Problema**: DataTables no puede cargar el archivo de idioma español desde CDN debido a política CORS.

**Error en consola**:
```
Access to XMLHttpRequest at 'http://cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json' 
from origin 'http://localhost:5089' has been blocked by CORS policy
```

**Solución**: Reemplazar la URL del CDN con definición local del idioma:

```javascript
$('#tablaElement').DataTable({
    language: {
        "processing": "Procesando...",
        "lengthMenu": "Mostrar _MENU_ registros",
        "zeroRecords": "No se encontraron resultados",
        "emptyTable": "Ningún dato disponible en esta tabla",
        "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
        "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
        "infoFiltered": "(filtrado de un total de _MAX_ registros)",
        "search": "Buscar:",
        "infoThousands": ",",
        "loadingRecords": "Cargando...",
        "paginate": {
            "first": "Primero",
            "last": "Último",
            "next": "Siguiente",
            "previous": "Anterior"
        },
        "aria": {
            "sortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    },
    // resto de configuración...
});
```

**NUNCA usar**: `url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json'`

### 4. Estructura Completa de la Vista Index

```html
@section Styles {
    <style>
        /* TODOS los estilos necesarios */
        
        /* Botón primario rectangular */
        .btn-primary.btn-sm {
            background-color: #0A1172 !important;
            border-color: #0A1172 !important;
            border-radius: 4px !important;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.15) !important;
        }

        /* Botones de acción sin fondo */
        .btn-group {
            display: inline-flex !important;
            justify-content: center !important;
            gap: 0 !important;
        }

        .btn-group .btn {
            padding: 0.25rem 0.5rem !important;
            margin: 0 !important;
            border: none;
            background: transparent;
            transition: transform 0.2s, color 0.2s;
        }
        
        /* Forzar que no haya espacio entre botones */
        .btn-group > .btn:not(:first-child) {
            margin-left: -1px !important;
        }

        .btn-group .btn:hover {
            transform: scale(1.2);
        }

        /* Hover en filas */
        .table-hover tbody tr:hover {
            background-color: rgba(0, 123, 255, 0.05);
            cursor: pointer;
        }

        /* Pestañas activas */
        .nav-link.active {
            border-bottom: 2px solid #007bff !important;
            font-weight: 500;
        }

        .nav-link {
            padding-bottom: 10px;
            margin-bottom: -1px;
        }

        /* Botón de exportar */
        .dropdown-toggle::after {
            display: inline-block;
            margin-left: 0.5em;
            vertical-align: 0.15em;
            content: "";
            border-top: 0.3em solid;
            border-right: 0.3em solid transparent;
            border-bottom: 0;
            border-left: 0.3em solid transparent;
        }

        .btn-outline-secondary {
            border-color: #e3e6f0;
            color: #6c757d;
        }

        .btn-outline-secondary:hover {
            background-color: #f8f9fa;
            color: #444;
            border-color: #dde0e6;
        }

        /* DataTables */
        .dataTables_length {
            width: 100%;
            margin-bottom: 15px;
            padding-left: 0;
        }

        .dataTables_length select {
            min-width: 60px;
        }

        div.dataTables_wrapper div.dataTables_length {
            float: left;
            padding-top: 0.5em;
        }
        
        /* Fix para el selector de registros */
        .dataTables_length label {
            display: flex !important;
            align-items: center !important;
            gap: 0.5rem !important;
        }
        
        .dataTables_length select {
            display: inline-block !important;
            width: auto !important;
            padding-right: 30px !important;
        }
    </style>
}
```

### 3. Botones en Vistas Create/Edit

**IMPORTANTE**: Los botones en Create/Edit usan los estilos estándar de Bootstrap, NO colores personalizados.

```html
<!-- Estructura correcta de botones -->
<div class="d-flex justify-content-end mt-4">
    <a asp-action="Index" class="btn btn-secondary me-2">Cancelar</a>
    <button type="submit" class="btn btn-primary">Guardar</button>
</div>
```

**NO usar**:
- Colores personalizados como #007848 (verde) o #787276 (gris)
- Clases personalizadas como .btn-guardar o .btn-cancelar

### 4. Breadcrumbs en Create/Edit

Siempre incluir breadcrumbs completos:

```html
<div class="container-fluid p-0">
    <div class="row mb-4">
        <div class="col-12">
            <h1 class="h4">Crear Nuevo [Elemento]</h1>
            <nav class="breadcrumb-container d-none d-sm-block d-lg-inline-block" aria-label="breadcrumb">
                <ol class="breadcrumb pt-0">
                    <li class="breadcrumb-item">
                        <a asp-controller="Home" asp-action="Index">Dashboard</a>
                    </li>
                    <li class="breadcrumb-item">
                        <a asp-controller="[Controlador]" asp-action="Index">[Módulo]</a>
                    </li>
                    <li class="breadcrumb-item active">Crear Nuevo</li>
                </ol>
            </nav>
        </div>
    </div>
```

### 5. Controlador - Implementación Completa

```csharp
// GET: Index con pestañas
public async Task<IActionResult> Index(string tab)
{
    var empresaId = await _empresaService.ObtenerEmpresaActualId();
    if (empresaId <= 0)
    {
        return RedirectToAction("Index", "Empresas");
    }
    
    bool activos = string.IsNullOrEmpty(tab) || tab == "Activos";
    ViewBag.Tab = activos ? "Activos" : "Inactivos";
    
    var elementos = await _context.Elementos
        .Where(e => e.EmpresaId == empresaId && e.Estado == activos)
        .OrderBy(e => e.Nombre)
        .ToListAsync();
        
    return View(elementos);
}

// POST: ToggleEstado
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ToggleEstado(int id)
{
    var empresaId = await _empresaService.ObtenerEmpresaActualId();
    var elemento = await _context.Elementos
        .FirstOrDefaultAsync(e => e.Id == id && e.EmpresaId == empresaId);
        
    if (elemento != null)
    {
        elemento.Estado = !elemento.Estado;
        elemento.FechaModificacion = DateTime.UtcNow;
        
        _context.Update(elemento);
        await _context.SaveChangesAsync();
        
        TempData["SuccessMessage"] = $"El {elemento.Nombre} ha sido {(elemento.Estado ? "activado" : "desactivado")} correctamente";
    }
    
    return RedirectToAction(nameof(Index), new { tab = elemento?.Estado == true ? "Activos" : "Inactivos" });
}
```

### 6. Espaciado de Iconos de Acción

El espaciado correcto ya está definido con `gap: 0` en `.btn-group`. Los iconos deben aparecer juntos sin separación.

### 7. Tamaños de Fuente

No se requieren estilos personalizados de tamaño de fuente. El sistema usa:
- Base móvil: 14px
- Base desktop: 16px
- Badges: 0.85rem

Todo se hereda de `site.css` y los estilos de DataTables.

### 8. Checklist de Implementación

Al implementar el estándar en un nuevo módulo:

- [ ] Copiar TODA la sección de estilos CSS (incluyendo el fix del selector)
- [ ] Usar estructura HTML exacta con container-fluid p-0
- [ ] Incluir breadcrumbs en todas las vistas
- [ ] Implementar pestañas Activos/Inactivos
- [ ] Usar btn-secondary y btn-primary para botones (NO colores custom)
- [ ] Agregar modal de confirmación estándar
- [ ] Implementar método ToggleEstado en el controlador
- [ ] Usar ViewBag.Tab (NO ViewBag.MostrarEstados)
- [ ] Configurar DataTables con idioma español
- [ ] Conectar botones de exportación con jQuery

### 9. Errores Comunes a Evitar

1. **NO** olvidar el CSS del selector de registros
2. **NO** usar colores personalizados en botones de formulario
3. **NO** omitir breadcrumbs
4. **NO** usar nombres de ViewBag diferentes
5. **NO** separar los iconos de acción con espacios adicionales

Esta guía garantiza una implementación 100% consistente con el módulo de Comprobantes.