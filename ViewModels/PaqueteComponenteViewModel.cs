using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaContable.ViewModels
{
    public class PaqueteComponenteViewModel
    {
        public int Id { get; set; }
        public int ProductoPaqueteId { get; set; }

        [Required(ErrorMessage = "El componente es requerido")]
        [Display(Name = "Componente")]
        public int ComponenteProductoId { get; set; }

        [Display(Name = "Cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; } = 1;

        [StringLength(50)]
        [Display(Name = "Grupo de Elección")]
        public string? GrupoEleccion { get; set; }

        [Display(Name = "Es Opcional")]
        public bool EsOpcional { get; set; } = false;

        [Display(Name = "Precio en Paquete")]
        [DataType(DataType.Currency)]
        public decimal? PrecioComponenteEnPaquete { get; set; }

        // Para mostrar información
        public string? NombreComponente { get; set; }
        public decimal? PrecioOriginalComponente { get; set; }

        // Para el dropdown de productos
        public SelectList? ProductosDisponibles { get; set; }
    }
}