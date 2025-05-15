using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class Modificador : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GrupoModificadoresId { get; set; }
        
        [ForeignKey("GrupoModificadoresId")]
        public virtual GrupoModificadores? GrupoModificadores { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioAdicional { get; set; } = 0;

        public bool StockControl { get; set; } = false;

        public int? ProductoConsumidoId { get; set; }
        
        [ForeignKey("ProductoConsumidoId")]
        public virtual ProductoVenta? ProductoConsumido { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal CantidadConsumida { get; set; } = 1;

        public int OrdenClasificacion { get; set; } = 0;

        public bool EsActivo { get; set; } = true;

        // Auditoría
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
}