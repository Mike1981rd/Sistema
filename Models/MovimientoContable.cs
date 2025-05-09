using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class MovimientoContable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Entrada de Diario")]
        public int EntradaDiarioId { get; set; }

        [Required]
        [Display(Name = "Cuenta Contable")]
        public int CuentaContableId { get; set; }

        [Display(Name = "Contacto")]
        public int? ContactoId { get; set; }

        [StringLength(1)]
        [Display(Name = "Tipo de Contacto")]
        public string? TipoContacto { get; set; }

        [StringLength(30)]
        [Display(Name = "Número de Documento")]
        public string? NumeroDocumento { get; set; }

        [StringLength(200)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required]
        [Display(Name = "Débito")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debito { get; set; } = 0;

        [Required]
        [Display(Name = "Crédito")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Credito { get; set; } = 0;

        // Propiedades de navegación
        [ForeignKey("EntradaDiarioId")]
        public virtual EntradaDiario? EntradaDiario { get; set; }

        [ForeignKey("CuentaContableId")]
        public virtual CuentaContable? CuentaContable { get; set; }

        [ForeignKey("ContactoId")]
        public virtual Cliente? Contacto { get; set; }
    }
} 