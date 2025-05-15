using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SistemaContable.Models.Enums;

namespace SistemaContable.Models
{
    public class VarianteProducto : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductoId { get; set; }
        
        [ForeignKey("ProductoId")]
        public virtual ProductoVenta? ProductoPadre { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string? PLUVariante { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioAdicionalOAbsoluto { get; set; }

        public AjustePrecioTipo AjustePrecioTipo { get; set; }

        public int? Stock { get; set; }

        public int OrdenClasificacion { get; set; } = 0;

        public bool EsActivo { get; set; } = true;

        // Auditoría
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
}