using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class PlazoPago
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
        
        [Display(Name = "Días")]
        public int? Dias { get; set; }
        
        [Display(Name = "Es el predeterminado")]
        public bool EsPredeterminado { get; set; }
        
        // Indica si es un plazo especial como "Vencimiento manual"
        public bool EsVencimientoManual { get; set; } = false;
        
        // Flag para impedir eliminación si está en uso
        public bool EstaEnUso { get; set; } = false;
        
        // Campos de auditoría
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
    }
} 