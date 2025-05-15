using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SistemaContable.Models.Enums;

namespace SistemaContable.Models
{
    public class GrupoModificadores : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        public bool EsForzado { get; set; } = false;

        public int MinSeleccion { get; set; } = 0;

        public int MaxSeleccion { get; set; } = 1;

        public TipoVisualizacionTPV TipoVisualizacionTPV { get; set; } = TipoVisualizacionTPV.Lista;

        // Colecciones de navegación
        public virtual ICollection<Modificador>? Modificadores { get; set; }
        public virtual ICollection<ProductoModificadorGrupo>? ProductoModificadorGrupos { get; set; }

        // Auditoría
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
}