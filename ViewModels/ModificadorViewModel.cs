using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaContable.ViewModels
{
    public class ModificadorViewModel
    {
        public int Id { get; set; }
        public int GrupoModificadoresId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Precio Adicional")]
        [DataType(DataType.Currency)]
        public decimal PrecioAdicional { get; set; } = 0;

        [Display(Name = "Control de Stock")]
        public bool StockControl { get; set; } = false;

        [Display(Name = "Producto Consumido")]
        public int? ProductoConsumidoId { get; set; }

        [Display(Name = "Cantidad Consumida")]
        public decimal CantidadConsumida { get; set; } = 1;

        [Display(Name = "Orden")]
        public int OrdenClasificacion { get; set; } = 0;

        [Display(Name = "Activo")]
        public bool EsActivo { get; set; } = true;

        // Para el dropdown de productos
        public SelectList? ProductosDisponibles { get; set; }
    }
}