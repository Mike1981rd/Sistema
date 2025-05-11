using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class RutaImpresora
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public bool Default { get; set; } = false;

        [Required]
        public bool Estado { get; set; } = true; // true = Activo, false = Inactivo

        // --- CAMPOS REQUERIDOS POR LA BASE DE DATOS ---
        [Required]
        public int EmpresaId { get; set; }
        public virtual Empresa? Empresa { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
    }
} 