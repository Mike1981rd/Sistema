using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class NumeracionEntradaDiario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tipo de Entrada")]
        public int TipoEntradaDiarioId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        [Display(Name = "Prefijo")]
        public string Prefijo { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Número Actual")]
        public int NumeroActual { get; set; } = 1;

        [Required]
        [Display(Name = "Es Preferida")]
        public bool EsPreferida { get; set; } = false;

        // Propiedades de navegación
        [ForeignKey("TipoEntradaDiarioId")]
        public virtual TipoEntradaDiario? TipoEntradaDiario { get; set; }

        public virtual ICollection<EntradaDiario> EntradasDiario { get; set; } = new List<EntradaDiario>();
    }
} 