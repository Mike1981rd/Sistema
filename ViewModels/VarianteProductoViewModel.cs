using System.ComponentModel.DataAnnotations;
using SistemaContable.Models.Enums;

namespace SistemaContable.ViewModels
{
    public class VarianteProductoViewModel
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50)]
        [Display(Name = "Nombre de la Variante")]
        public string Nombre { get; set; }

        [StringLength(50)]
        [Display(Name = "PLU Variante")]
        public string? PLUVariante { get; set; }

        [Display(Name = "Precio")]
        [DataType(DataType.Currency)]
        public decimal PrecioAdicionalOAbsoluto { get; set; }

        [Display(Name = "Tipo de Ajuste")]
        public AjustePrecioTipo AjustePrecioTipo { get; set; }

        [Display(Name = "Stock")]
        public int? Stock { get; set; }

        [Display(Name = "Orden")]
        public int OrdenClasificacion { get; set; } = 0;

        [Display(Name = "Activo")]
        public bool EsActivo { get; set; } = true;

        // Para mostrar el precio final calculado
        public decimal PrecioFinal { get; set; }
    }
}