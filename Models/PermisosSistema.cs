using System.Collections.Generic;

namespace SistemaContable.Models
{
    public static class PermisosSistema
    {
        public static readonly Dictionary<string, Dictionary<string, string>> EstructuraPermisos = new Dictionary<string, Dictionary<string, string>>
        {
            ["Dashboard"] = new Dictionary<string, string>
            {
                ["dashboard.ver"] = "Ver Dashboard"
            },
            ["Configuración"] = new Dictionary<string, string>
            {
                ["configuracion.roles"] = "Roles",
                ["configuracion.usuarios"] = "Usuarios",
                ["configuracion.empresa"] = "Empresa",
                ["configuracion.impuestos"] = "Impuestos",
                ["configuracion.familias"] = "Familias",
                ["configuracion.categorias"] = "Categorías",
                ["configuracion.plazos_pago"] = "Plazos de Pago",
                ["configuracion.retenciones"] = "Retenciones",
                ["configuracion.comprobantes_fiscales"] = "Comprobantes Fiscales"
            },
            ["Ventas"] = new Dictionary<string, string>
            {
                ["ventas.facturas"] = "Facturas",
                ["ventas.clientes"] = "Clientes",
                ["ventas.cotizaciones"] = "Cotizaciones",
                ["ventas.devoluciones"] = "Devoluciones"
            },
            ["Compras"] = new Dictionary<string, string>
            {
                ["compras.facturas"] = "Facturas",
                ["compras.proveedores"] = "Proveedores",
                ["compras.gastos"] = "Gastos",
                ["compras.devoluciones"] = "Devoluciones"
            },
            ["Inventario"] = new Dictionary<string, string>
            {
                ["inventario.almacenes"] = "Almacenes",
                ["inventario.items"] = "Items"
            },
            ["Punto de Venta"] = new Dictionary<string, string>
            {
                ["pos.tamanos"] = "Tamaños",
                ["pos.productos"] = "Productos",
                ["pos.menu"] = "Menu",
                ["pos.areas"] = "Areas",
                ["pos.promociones"] = "Promociones",
                ["pos.terminales"] = "Terminales",
                ["pos.descuentos"] = "Descuentos",
                ["pos.kitchen_display"] = "Kitchen Display",
                ["pos.motivos_anulacion"] = "Motivos de Anulación",
                ["pos.reservaciones"] = "Reservaciones",
                ["pos.tipos_ordenes"] = "Tipos de Ordenes",
                ["pos.repartidores"] = "Repartidores",
                ["pos.zonas_domicilio"] = "Zonas de Domicilio",
                ["pos.impresoras"] = "Impresoras",
                ["pos.rutas_impresora"] = "Rutas de Impresora"
            },
            ["Bancos"] = new Dictionary<string, string>
            {
                ["bancos.cuentas"] = "Cuentas",
                ["bancos.transacciones"] = "Transacciones",
                ["bancos.conciliacion"] = "Conciliación"
            },
            ["Contabilidad"] = new Dictionary<string, string>
            {
                ["contabilidad.catalogo"] = "Catálogo de cuentas",
                ["contabilidad.entradas_diario"] = "Entradas de diario",
                ["contabilidad.libro_diario"] = "Libro diario",
                ["contabilidad.libro_mayor"] = "Libro mayor"
            },
            ["Reportes"] = new Dictionary<string, string>
            {
                ["reportes.estado_resultados"] = "Estado de resultados",
                ["reportes.balance_general"] = "Balance general",
                ["reportes.impuestos"] = "Impuestos",
                ["reportes.ventas"] = "Reportes de ventas"
            }
        };

        // Método helper para obtener todos los permisos como lista plana
        public static List<string> ObtenerTodosLosPermisos()
        {
            var permisos = new List<string>();
            foreach (var categoria in EstructuraPermisos.Values)
            {
                permisos.AddRange(categoria.Keys);
            }
            return permisos;
        }
    }
}