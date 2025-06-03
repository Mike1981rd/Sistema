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

        // Cambio: ahora apunta a Item en lugar de ProductoVenta
        [Required]
        public int ItemId { get; set; }
        
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }

        // Nuevo: referencia al contenedor/unidad de medida específica
        [Required]
        public int ItemContenedorId { get; set; }
        
        [ForeignKey("ItemContenedorId")]
        public virtual ItemContenedor? ItemContenedor { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cantidad { get; set; }

        // Nuevo: costo unitario al momento de crear la receta
        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal CostoUnitario { get; set; }

        // Calculado: Cantidad * CostoUnitario
        [Column(TypeName = "decimal(18,4)")]
        public decimal CostoTotal { get; set; }

        // Auditoría
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
}