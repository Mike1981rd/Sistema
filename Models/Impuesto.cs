using System;
using System.ComponentModel.DataAnnotations;
using SistemaContable.Models.Enums;

namespace SistemaContable.Models
{
    public class Impuesto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre del impuesto es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El tipo de impuesto es obligatorio")]
        public TipoImpuesto Tipo { get; set; }
        
        // Solo requerido si el tipo no es Exento
        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100")]
        public decimal? Porcentaje { get; set; }
        
        [StringLength(500)]
        public string? Descripcion { get; set; }
        
        public bool EsAcreditable { get; set; } = true;
        
        [Required(ErrorMessage = "La cuenta contable de ventas es obligatoria")]
        public int CuentaContableVentasId { get; set; }
        
        [Required(ErrorMessage = "La cuenta contable de compras es obligatoria")]
        public int CuentaContableComprasId { get; set; }
        
        // Propiedades de navegación
        public virtual CuentaContable? CuentaContableVentas { get; set; }
        public virtual CuentaContable? CuentaContableCompras { get; set; }
        
        // Campos de auditoría
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        
        // Flag para indicar si el impuesto está en uso
        public bool EstaEnUso { get; set; } = false;
        
        // Flag para indicar si el impuesto está activo
        public bool Activo { get; set; } = true;
        
        // ID de la empresa a la que pertenece este impuesto
        [Required]
        public int EmpresaId { get; set; }
        public virtual Empresa? Empresa { get; set; }
    }
} 