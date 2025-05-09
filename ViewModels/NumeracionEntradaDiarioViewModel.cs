using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels
{
    public class NumeracionEntradaDiarioViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El tipo de entrada es obligatorio")]
        [DisplayName("Tipo de entrada de diario")]
        public int TipoEntradaDiarioId { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "El prefijo es obligatorio")]
        [DisplayName("Prefijo")]
        [StringLength(10)]
        public string Prefijo { get; set; }
        
        [Required(ErrorMessage = "El número inicial es obligatorio")]
        [DisplayName("Número Inicial")]
        [Range(1, int.MaxValue, ErrorMessage = "El número debe ser mayor a 0")]
        public int NumeroActual { get; set; }
        
        [DisplayName("Preferida")]
        public bool EsPreferida { get; set; }
        
        public SelectList TiposEntrada { get; set; }
    }
} 