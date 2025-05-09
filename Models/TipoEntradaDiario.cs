using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.Models
{
    public class TipoEntradaDiario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        // Propiedades de navegación
        public virtual ICollection<EntradaDiario> EntradasDiario { get; set; } = new List<EntradaDiario>();
        
        public virtual ICollection<NumeracionEntradaDiario> Numeraciones { get; set; } = new List<NumeracionEntradaDiario>();
    }
} 