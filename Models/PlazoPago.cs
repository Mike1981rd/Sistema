using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.Models
{
    public class PlazoPago
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre del plazo es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El número de días es obligatorio")]
        [Range(0, 365, ErrorMessage = "Los días deben estar entre 0 y 365")]
        public int? Dias { get; set; }
        
        // Indica si es un plazo especial como "Vencimiento manual"
        public bool EsVencimientoManual { get; set; } = false;
        
        // Flag para impedir eliminación si está en uso
        public bool EstaEnUso { get; set; } = false;
        
        // Campos de auditoría
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
    }
} 