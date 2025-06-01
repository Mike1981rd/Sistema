# Sistema Contable Aurora - Documentación

## 🏗️ Arquitectura
- **Stack**: ASP.NET Core MVC + PostgreSQL + Entity Framework Core
- **Frontend**: Razor Views + jQuery + Bootstrap 5 + Select2
- **Patrón**: MVC con Repository (selectivo)
- **Autenticación**: No implementada
- **Multi-Empresa**: EmpresaId = 4 (hardcoded en appsettings.json)

## 📁 Estructura Principal
```
/Controllers    - Controladores MVC
/Models         - Modelos de dominio y ViewModels
/Views          - Vistas Razor por controlador
/Services       - Lógica de negocio (EmpresaService)
/Repositories   - Repository pattern (solo algunos módulos)
/Data           - ApplicationDbContext y migraciones
/wwwroot        - Recursos estáticos por módulo
```

## 🔑 Manejo de EmpresaId

### Configuración (appsettings.json)
```json
{
  "AppSettings": {
    "EmpresaUnicaId": 4
  }
}
```

### Uso en Controladores
```csharp
// Inyectar servicio
private readonly IEmpresaService _empresaService;

// Obtener empresaId
var empresaId = await _empresaService.ObtenerEmpresaActualId();

// Filtrar queries
.Where(x => x.EmpresaId == empresaId)

// Asignar en creates
entidad.EmpresaId = empresaId;
```

## 🎨 Dashboard
- **Vista**: `/Views/Home/Index.cshtml`
- **Layout**: `/Views/Shared/_Layout.cshtml`
- **Colores Vuexy**: #7367f0 (púrpura), #28c76f (verde), #ff9f43 (naranja), #00cfe8 (azul), #ea5455 (rojo)
- **Cards**: Stats cards, Congratulations card, Popular products, Orders by countries
- **Gráficos**: Chart.js + SVG radiales

## 🔧 Select2 para Cuentas Contables

### ⚠️ FIX CRÍTICO para Edit (Enero 2025)
El endpoint `/EntradaDiario/BuscarCuentasContables` devuelve lista vacía cuando term="". 

**Solución**: Crear endpoint específico para obtener por ID:

```csharp
// En EntradaDiarioController.cs
[HttpGet]
public async Task<IActionResult> ObtenerCuentaContable(int id)
{
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

    return Json(new { success = true, cuenta = cuenta });
}
```

### JavaScript para Edit
```javascript
// Cargar cuenta preseleccionada
$.ajax({
    url: '/EntradaDiario/ObtenerCuentaContable',
    data: { id: @Model.CuentaId },
    success: function(response) {
        if (response.success && response.cuenta) {
            var option = new Option(response.cuenta.text, response.cuenta.id, true, true);
            $('#CuentaId').append(option).trigger('change');
        }
    }
});
```

## 📋 Estándar de Vistas Index

### Estructura Base
- Container-fluid p-0
- Breadcrumbs
- Pestañas Activos/Inactivos
- Alertas dismissibles
- Barra de acciones (Exportar + Nuevo)
- DataTable responsive

### Colores Corporativos
- **Azul primario**: #0A1172 (botones principales)
- **Verde confirmar**: #007848
- **Gris cancelar**: #787276

### Iconos Estándar
- Nuevo: `fas fa-plus`
- Editar: `fas fa-edit`
- Desactivar: `fas fa-ban`
- Activar: `fas fa-check-circle`

### CSS Críticos
```css
/* Fix selector DataTables */
.dataTables_length label {
    display: flex !important;
    align-items: center !important;
    gap: 0.5rem !important;
}

/* Fix espaciado iconos (solución agresiva) */
.btn-group.no-gap {
    display: inline-block !important;
    font-size: 0 !important;
}
.btn-group.no-gap .btn + .btn {
    margin-left: -5px !important;
}
```

### DataTables - Idioma Español Local
```javascript
language: {
    "processing": "Procesando...",
    "lengthMenu": "Mostrar _MENU_ registros",
    "search": "Buscar:",
    // ... (definir todo localmente, NO usar CDN)
}
```

## ✅ Checklist Nuevos Módulos
1. Modelo hereda BaseEntity con EmpresaId
2. DbSet en ApplicationDbContext
3. Migración EF
4. Controlador inyecta IEmpresaService
5. Vistas en /Views/[Controlador]/
6. JS en /wwwroot/js/[modulo]/
7. Filtrar SIEMPRE por empresaId
8. DateTime.UtcNow para fechas
9. Soft delete con campo Activo
10. Try-catch en operaciones DB

## ⚠️ Importante
- NO crear archivos innecesarios
- Preferir editar sobre crear
- Seguir patrones existentes
- Validar empresaId != 0
- No hay autenticación implementada