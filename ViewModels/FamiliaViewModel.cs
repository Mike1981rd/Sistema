using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaContable.ViewModels
{
    public class FamiliaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "La nota no puede exceder los 500 caracteres")]
        [DisplayName("Nota")]
        public string? Nota { get; set; }

        [Required]
        [DisplayName("Estado")]
        public bool Estado { get; set; } = true;

        // Cuentas Contables
        [DisplayName("Cuenta de Ventas")]
        public int? CuentaVentasId { get; set; }
        public SelectList? CuentasVentasDisponibles { get; set; }

        [DisplayName("Cuenta de Compras/Inventarios")]
        public int? CuentaComprasInventariosId { get; set; }
        public SelectList? CuentasComprasInventariosDisponibles { get; set; }

        [DisplayName("Cuenta de Costo de Ventas/Gastos")]
        public int? CuentaCostoVentasGastosId { get; set; }
        public SelectList? CuentasCostoVentasGastosDisponibles { get; set; }

        [DisplayName("Cuenta de Descuentos")]
        public int? CuentaDescuentosId { get; set; }
        public SelectList? CuentasDescuentosDisponibles { get; set; }

        [DisplayName("Cuenta de Devoluciones")]
        public int? CuentaDevolucionesId { get; set; }
        public SelectList? CuentasDevolucionesDisponibles { get; set; }

        [DisplayName("Cuenta de Ajustes")]
        public int? CuentaAjustesId { get; set; }
        public SelectList? CuentasAjustesDisponibles { get; set; }

        [DisplayName("Cuenta de Costo de Materia Prima")]
        public int? CuentaCostoMateriaPrimaId { get; set; }
        public SelectList? CuentasCostoMateriaPrimaDisponibles { get; set; }

        // Campos requeridos por la base de datos
        [Required]
        public int EmpresaId { get; set; }
    }
} 