using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaContable.ViewModels
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [DisplayName("Nombre de la categoría")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "La nota no puede exceder los 500 caracteres")]
        [DisplayName("Nota")]
        public string? Nota { get; set; }

        [Required]
        [DisplayName("Estado")]
        public bool Estado { get; set; } = true;

        // Familia (Grupo en la UI)
        [Required(ErrorMessage = "Debe seleccionar una familia")]
        [DisplayName("Familia")]
        public int FamiliaId { get; set; }
        public SelectList? FamiliasDisponibles { get; set; }

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

        // Impuestos
        [DisplayName("Impuesto")]
        public int? ImpuestoId { get; set; }
        public SelectList? ImpuestosDisponibles { get; set; }

        // Propina
        [DisplayName("Propina")]
        public int? PropinaImpuestoId { get; set; }
        public SelectList? PropinasDisponibles { get; set; }

        // Ruta de impresora
        [DisplayName("Ruta de Impresión")]
        public int? RutaImpresoraId { get; set; }
        public SelectList? RutasImpresoraDisponibles { get; set; }

        /// <summary>
        /// Obsoleta: Use RutaImpresoraId en su lugar
        /// </summary>
        [Obsolete("Use RutaImpresoraId instead", true)]
        public string CanalesImpresora 
        { 
            get => RutaImpresoraId?.ToString(); 
            set 
            {
                if (int.TryParse(value, out int id))
                    RutaImpresoraId = id;
            }
        }

        // Campos requeridos por la base de datos
        [Required]
        public int EmpresaId { get; set; }
    }
} 