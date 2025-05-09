using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels
{
    public class TipoEntradaDiarioViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El código es obligatorio")]
        [DisplayName("Código")]
        [StringLength(10)]
        public string Codigo { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Nombre { get; set; }
    }
} 