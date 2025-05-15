using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class PaqueteComponente : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductoPaqueteId { get; set; }
        
        [ForeignKey("ProductoPaqueteId")]
        public virtual ProductoVenta? ProductoPaquete { get; set; }

        [Required]
        public int ComponenteProductoId { get; set; }
        
        [ForeignKey("ComponenteProductoId")]
        public virtual ProductoVenta? ComponenteProducto { get; set; }

        public int Cantidad { get; set; } = 1;

        [StringLength(50)]
        public string? GrupoEleccion { get; set; }

        public bool EsOpcional { get; set; } = false;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrecioComponenteEnPaquete { get; set; }

        // Auditoría
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
}