using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class Retencion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la retención es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El porcentaje de retención es obligatorio")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100")]
        [Display(Name = "Porcentaje")]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Porcentaje { get; set; }

        [Required(ErrorMessage = "El tipo de retención es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; } = string.Empty;

        [StringLength(255)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        // Cuentas contables
        [StringLength(50)]
        [Display(Name = "Cuenta Contable para Ventas")]
        public string? CuentaContableVentas { get; set; }

        [StringLength(50)]
        [Display(Name = "Cuenta Contable para Compras")]
        public string? CuentaContableCompras { get; set; }

        [StringLength(50)]
        [Display(Name = "Cuenta Contable para Retenciones Asumidas")]
        public string? CuentaContableRetencionesAsumidas { get; set; }
        
        // Estado
        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
        
        // Auditoría
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
} 