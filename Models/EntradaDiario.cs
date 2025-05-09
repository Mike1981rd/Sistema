using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class EntradaDiario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required]
        [Display(Name = "Tipo de Entrada")]
        public int TipoEntradaId { get; set; }

        [Required]
        [Display(Name = "Numeración")]
        public int NumeracionId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public EstadoEntradaDiario Estado { get; set; } = EstadoEntradaDiario.Abierto;

        [Required]
        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Display(Name = "Fecha de Modificación")]
        public DateTime? FechaModificacion { get; set; }

        // Propiedades de navegación
        [ForeignKey("TipoEntradaId")]
        public virtual TipoEntradaDiario? TipoEntrada { get; set; }

        [ForeignKey("NumeracionId")]
        public virtual NumeracionEntradaDiario? Numeracion { get; set; }

        public virtual ICollection<MovimientoContable> Movimientos { get; set; } = new List<MovimientoContable>();
    }

    public enum EstadoEntradaDiario
    {
        Abierto,
        Anulado
    }
} 