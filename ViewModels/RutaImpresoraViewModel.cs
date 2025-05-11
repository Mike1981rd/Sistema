using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels
{
    public class RutaImpresoraViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [DisplayName("Predeterminado")]
        public bool Default { get; set; } = false;

        [Required]
        [DisplayName("Estado")]
        public bool Estado { get; set; } = true;

        // Campos requeridos por la base de datos
        [Required]
        public int EmpresaId { get; set; }
    }
} 