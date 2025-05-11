using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class Impresora
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Modelo { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? RutasFisicas { get; set; }

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