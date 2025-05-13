using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaContable.Models.ViewModels
{
    public class ItemContabilidadViewModel
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        [Display(Name = "Cuenta de Ventas")]
        public int? CuentaVentasId { get; set; }
        public string? CuentaVentasNombre { get; set; }

        [Display(Name = "Cuenta de Compras/Inventarios")]
        public int? CuentaComprasInventariosId { get; set; }
        public string? CuentaComprasInventariosNombre { get; set; }

        [Display(Name = "Cuenta de Costo de Ventas")]
        public int? CuentaCostoVentasGastosId { get; set; }
        public string? CuentaCostoVentasGastosNombre { get; set; }

        [Display(Name = "Cuenta de Descuentos")]
        public int? CuentaDescuentosId { get; set; }
        public string? CuentaDescuentosNombre { get; set; }

        [Display(Name = "Cuenta de Devoluciones")]
        public int? CuentaDevolucionesId { get; set; }
        public string? CuentaDevolucionesNombre { get; set; }

        [Display(Name = "Cuenta de Ajustes")]
        public int? CuentaAjustesId { get; set; }
        public string? CuentaAjustesNombre { get; set; }

        [Display(Name = "Impuesto")]
        public int? ImpuestoId { get; set; }
        public string? ImpuestoNombre { get; set; }

        // Para los dropdowns de selección de cuentas contables
        public SelectList? CuentasVentasDisponibles { get; set; }
        public SelectList? CuentasComprasInventariosDisponibles { get; set; }
        public SelectList? CuentasCostoVentasGastosDisponibles { get; set; }
        public SelectList? CuentasDescuentosDisponibles { get; set; }
        public SelectList? CuentasDevolucionesDisponibles { get; set; }
        public SelectList? CuentasAjustesDisponibles { get; set; }
        public SelectList? ImpuestosDisponibles { get; set; }

        // Propiedades para mostrar información de la categoría
        public int? CategoriaId { get; set; }
        public string? CategoriaNombre { get; set; }

        // Flags para indicar qué cuentas fueron heredadas de la categoría
        public bool CuentaVentasHeredada { get; set; }
        public bool CuentaComprasInventariosHeredada { get; set; }
        public bool CuentaCostoVentasGastosHeredada { get; set; }
        public bool CuentaDescuentosHeredada { get; set; }
        public bool CuentaDevolucionesHeredada { get; set; }
        public bool CuentaAjustesHeredada { get; set; }
        public bool ImpuestoHeredado { get; set; }
    }
}