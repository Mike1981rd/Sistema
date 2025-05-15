using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class RecetaIngrediente : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductoCompuestoId { get; set; }
        
        [ForeignKey("ProductoCompuestoId")]
        public virtual ProductoVenta? ProductoCompuesto { get; set; }

        [Required]
        public int IngredienteProductoId { get; set; }
        
        [ForeignKey("IngredienteProductoId")]
        public virtual ProductoVenta? IngredienteProducto { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cantidad { get; set; }

        [StringLength(20)]
        public string? UnidadMedida { get; set; }

        // Auditoría
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
}