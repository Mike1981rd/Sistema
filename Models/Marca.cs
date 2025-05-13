using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.Models
{
    public class Marca : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public bool Estado { get; set; } = true;

        // Auditoría
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
}
