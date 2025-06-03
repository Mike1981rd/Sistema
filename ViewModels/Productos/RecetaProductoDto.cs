using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels.Productos
{
    public class RecetaProductoDto
    {
        public int ProductoId { get; set; }
        
        [StringLength(1000)]
        public string? NotasReceta { get; set; }
        
        [Range(0, 100)]
        public decimal? MargenGanancia { get; set; }
        
        public List<RecetaIngredienteDto> Ingredientes { get; set; } = new List<RecetaIngredienteDto>();
    }

    public class RecetaIngredienteDto
    {
        public int? Id { get; set; }
        
        [Required]
        public int ItemId { get; set; }
        
        [Required]
        public int ItemContenedorId { get; set; }
        
        [Required]
        [Range(0.001, double.MaxValue)]
        public decimal Cantidad { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal CostoUnitario { get; set; }
        
        // Información adicional para mostrar
        public string? NombreItem { get; set; }
        public string? MarcaNombre { get; set; }
        public string? UnidadMedidaNombre { get; set; }
    }
}