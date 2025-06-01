# Sistema de Permisos por Roles - Instrucciones de Uso

## 🎯 Objetivo
Sistema que filtra dinámicamente el menú lateral según los permisos asignados al rol del usuario.

## 🔧 ¿Cómo Funciona?

### 1. **Configuración de Roles**
1. Ve a **Configuración > Roles**
2. Crear o editar un rol
3. En la sección de **Permisos**, usa los toggles para activar/desactivar permisos:
   - **Toggle de categoría**: Activa/desactiva todos los permisos de una sección
   - **Toggle individual**: Activa/desactiva permisos específicos

### 2. **Asignación de Usuarios**
1. Ve a **Configuración > Usuarios**
2. Asigna el rol correspondiente al usuario
3. El usuario hereda automáticamente los permisos del rol

### 3. **Filtrado del Menú**
Al hacer login, el menú lateral se filtra automáticamente:
- Solo aparecen las secciones con permisos activos
- Dentro de cada sección, solo los elementos permitidos

## 📋 Estructura de Permisos

### Dashboard
- `dashboard.ver` → Ver Dashboard

### Configuración
- `configuracion.roles` → Roles
- `configuracion.usuarios` → Usuarios  
- `configuracion.empresa` → Empresa
- `configuracion.impuestos` → Impuestos
- `configuracion.familias` → Familias
- `configuracion.categorias` → Categorías
- `configuracion.plazos_pago` → Plazos de Pago
- `configuracion.retenciones` → Retenciones
- `configuracion.comprobantes_fiscales` → Comprobantes Fiscales

### Ventas
- `ventas.facturas` → Facturas
- `ventas.clientes` → Clientes
- `ventas.cotizaciones` → Cotizaciones
- `ventas.devoluciones` → Devoluciones

### Compras
- `compras.facturas` → Facturas
- `compras.proveedores` → Proveedores
- `compras.gastos` → Gastos
- `compras.devoluciones` → Devoluciones

### Inventario
- `inventario.almacenes` → Almacenes
- `inventario.items` → Items

### Punto de Venta
- `pos.tamanos` → Tamaños
- `pos.productos` → Productos
- `pos.menu` → Menu
- `pos.areas` → Areas
- `pos.promociones` → Promociones
- `pos.terminales` → Terminales
- `pos.descuentos` → Descuentos
- `pos.kitchen_display` → Kitchen Display
- `pos.motivos_anulacion` → Motivos de Anulación
- `pos.reservaciones` → Reservaciones
- `pos.tipos_ordenes` → Tipos de Ordenes
- `pos.repartidores` → Repartidores
- `pos.zonas_domicilio` → Zonas de Domicilio
- `pos.impresoras` → Impresoras
- `pos.rutas_impresora` → Rutas de Impresora

### Bancos
- `bancos.cuentas` → Cuentas
- `bancos.transacciones` → Transacciones
- `bancos.conciliacion` → Conciliación

### Contabilidad
- `contabilidad.catalogo` → Catálogo de cuentas
- `contabilidad.entradas_diario` → Entradas de diario
- `contabilidad.libro_diario` → Libro diario
- `contabilidad.libro_mayor` → Libro mayor

### Reportes
- `reportes.estado_resultados` → Estado de resultados
- `reportes.balance_general` → Balance general
- `reportes.impuestos` → Impuestos
- `reportes.ventas` → Reportes de ventas

## 📝 Ejemplo Práctico

### Caso: Rol "Asistente"
**Configuración del rol:**
- ✅ `dashboard.ver`
- ✅ `configuracion.impuestos`
- ✅ `ventas.clientes`
- ❌ `configuracion.roles` (desactivado)
- ❌ `contabilidad.*` (toda la sección desactivada)

**Resultado en el menú:**
- ✅ Dashboard
- ✅ Configuración (pero solo Impuestos visible)
- ✅ Ventas (pero solo Clientes visible)
- ❌ Contabilidad (sección completa oculta)

### Caso: Rol "Administrador"
**Configuración del rol:**
- ✅ Todos los permisos activados

**Resultado en el menú:**
- ✅ Todas las secciones visibles
- ✅ Todos los elementos dentro de cada sección

## 🔧 Archivos Modificados

### Nuevos Servicios
- `Services/IPermisosService.cs` - Interface del servicio
- `Services/PermisosService.cs` - Implementación del servicio
- `Helpers/PermisosViewHelper.cs` - Helper para vistas

### Archivos Actualizados
- `Program.cs` - Registro del servicio de permisos
- `Views/_ViewImports.cshtml` - Import del helper
- `Views/Shared/_Layout.cshtml` - Filtrado dinámico del menú

### Base de Datos
- El modelo `Rol` ya tenía la propiedad `Permisos` como JSON
- El modelo `PermisosSistema` ya contenía la estructura completa
- No se requieren migraciones adicionales

## ⚠️ Importante

1. **Sin usuario logueado**: El menú muestra todas las opciones (fallback)
2. **Usuario sin rol**: No se muestra ninguna opción del menú
3. **Rol sin permisos**: No se muestra ninguna opción del menú
4. **Performance**: Los permisos se verifican una sola vez al cargar la página

## 🚀 ¿Cómo Probar?

1. Crear un rol de prueba (ej: "Asistente")
2. Activar solo algunos permisos específicos
3. Crear un usuario y asignarle ese rol
4. Hacer login con ese usuario
5. Verificar que el menú solo muestre las opciones permitidas

¡El sistema está listo para usar! 🎉