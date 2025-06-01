using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del rol es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [Display(Name = "Nombre del Rol")]
        public string Nombre { get; set; } = string.Empty;
        
        // Propiedades adicionales requeridas
        public int EmpresaId { get; set; }
        public bool Activo { get; set; } = true;

        [Display(Name = "Prioridad")]
        [Range(0, 100, ErrorMessage = "La prioridad debe estar entre 0 y 100")]
        public int Prioridad { get; set; } = 0;

        [Display(Name = "Descripción")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }

        // Almacenamiento de permisos como JSON
        [Column(TypeName = "jsonb")]
        public List<string> Permisos { get; set; } = new List<string>();

        // Navegación
        public virtual Empresa? Empresa { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        
        // Fechas de auditoría
        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }

        // Propiedad calculada para mostrar cantidad de permisos
        [NotMapped]
        public int CantidadPermisos => Permisos?.Count ?? 0;

        // Método para verificar si tiene un permiso específico
        public bool TienePermiso(string permiso)
        {
            return Permisos != null && Permisos.Contains(permiso);
        }
    }
}