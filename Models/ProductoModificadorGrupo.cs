using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class ProductoModificadorGrupo
    {
        public int ProductoId { get; set; }
        
        [ForeignKey("ProductoId")]
        public virtual ProductoVenta? Producto { get; set; }

        public int GrupoModificadoresId { get; set; }
        
        [ForeignKey("GrupoModificadoresId")]
        public virtual GrupoModificadores? GrupoModificadores { get; set; }

        public int OrdenEspecificoProducto { get; set; } = 0;

        // Auditoría
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }
        
        // Campos de auditoría heredados de BaseEntity
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
}